using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.ControleCaixa;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF.Cancelar;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.FusionAdm.TabelasDePrecos.NfceSync;
using FusionCore.FusionNfce.Fiscal.Flags;
using FusionCore.FusionNfce.Fiscal.SatFiscal;
using FusionCore.FusionNfce.Fiscal.Tributacoes;
using FusionCore.FusionNfce.Pagamento;
using FusionCore.FusionNfce.Preferencias;
using FusionCore.FusionNfce.Usuario;
using FusionCore.FusionNfce.Vendedores;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Tributacoes.Flags;
using MotorTributarioNet.Facade;
using MotorTributarioNet.Flags;
using MotorTributarioNet.Impostos.Csts;
using NHibernate.Util;

namespace FusionCore.FusionNfce.Fiscal
{
    public class Nfce : Entidade, IDocumentoCancelavel, IVendaRegistravelEmCaixa, IDadosParaEnvioEmailNfce
    {

        private IList<NfceItem> _itens;
        private IList<FormaPagamentoNfce> _formaPagamentos;
        private decimal _totalDesconto;
        private decimal _totalProdutosServicos;
        private decimal _totalNfce;
        private decimal _valorTributoAproximado;
        private decimal _totalAcrescimo;
        public int Id { get; set; }
        public ModeloDocumento Modelo { get; set; }
        public string NaturezaOperacao { get; set; }
        public DateTime EmitidaEm { get; set; }
        public DateTime EntradaSaidaEm { get; set; }
        public TipoOperacao TipoOperacao { get; set; }
        public TipoDanfe TipoDanfe { get; set; }
        public FinalidadeEmissao FinalidadeEmissao { get; set; }
        public DestinoOperacao DestinoOperacao { get; set; }
        public IndicadorOperacaoFinal IndicadorConsumidorFinal { get; set; }
        public IndicadorComprador IndicadorComprador { get; set; }
        public ModalidadeFrete ModalidadeFrete { get; set; }
        public FormaPagamento FormaPagamento { get; set; }
        public string InformacaoAdicional { get; set; }
        public decimal TotalBaseCalculo { get; set; }
        public decimal TotalIcms { get; set; }
        public decimal TotalBaseCalculoCofins { get; set; }
        public decimal TotalCofins { get; set; }
        public decimal TotalBaseCalculoPis { get; set; }
        public decimal TotalPis { get; set; }
        public decimal TotalIcmsDesonerado { get; set; }
        public decimal TotalBaseCalculoSt { get; set; }
        public decimal TotalSt { get; set; }
        public byte TerminalOfflineId { get; set; }
        public string Uuid { get; set; }
        public Status Status { get; private set; }
        public string Observacao { get; set; }
        public string UuidVenda { get; set; }
        public FinalizaEmissaoSat FinalizaEmissaoSat { get; set; }
        public NfceContingencia Contingencia { get; set; }
        public DateTime CriadoEm { get; set; }
        public UsuarioNfce UsuarioCriacao { get; set; }
        public RegimeTributario RegimeTributario { get; set; } = RegimeTributario.SimplesNacional;
        public int CodigoNumerico { get; set; }
        public int NumeroFiscal { get; set; }
        public short Serie { get; set; }
        public TipoEmissao TipoEmissao { get; set; } = TipoEmissao.Normal;
        public TabelaPrecoNfce TabelaPreco { get; set; }
        public decimal ValorTributoAproximado
        {
            get { return _itens?.Where(i => i.Cancelado == false).ToList().Sum(i => i.ValorTributoAproximado) ?? 0; }
            set { _valorTributoAproximado = value; }
        }

        public decimal TotalProdutosServicos
        {
            get { return _itens?.Where(i => i.Cancelado == false).ToList().Sum(i => i.ValorTotal) ?? 0; }
            private set { _totalProdutosServicos = value; }
        }

        public decimal TotalDesconto
        {
            get { return DescontoNoTotalSoma() + _itens?.Where(i => i.Cancelado == false).ToList().Sum(i => i.DescontoAlteraItem) ?? 0; }
            private set { _totalDesconto = value; }
        }

        public decimal? DescontoNoTotalSoma()
        {
            return _itens?.Where(i => i.Cancelado == false).ToList().Sum(i => i.Desconto) ?? 0;
        }

        public decimal TotalAcrescimo
        {
            get { return _itens?.Where(i => i.Cancelado == false).ToList().Sum(i => i.Acrescimo) ?? 0; }
            private set { _totalAcrescimo = value; }
        }

        public decimal TotalNfce
        {
            get
            {
                if (_itens == null || _itens.Count == 0)
                {
                    return 0;
                }

                var total = _itens.Where(i => i.Cancelado == false)
                    .Sum(i => i.ValorTotal);

                return decimal.Round(total + TotalAcrescimo - TotalDesconto, 2);
            }

            private set { _totalNfce = value; }
        }

        public string XmlAutorizado => Emissao != null ? Emissao.XmlAutorizado : null;
        public bool Cancelada => Status == Status.Cancelada;
        public byte[] Logo => Emitente.Empresa.LogoMarca;
        public decimal Troco { get; set; }
        public NfceEmissao Emissao { get; set; }
        public NfceDestinatario Destinatario { get; set; }
        public NfceEmitente Emitente { get; set; }
        public NfceCancelamento Cancelamento { get; set; }
        public CancelamentoSat CancelamentoSat { get; set; }
        public NfceCobranca Cobranca { get; set; }

        public int ReferenciaId => Id;
        public int NumeroDocumento => NumeroFiscal;
        public string NumeroProtocolo => Emissao.Protocolo;
        public string NumeroChave => Emissao.Chave;
        public string CnpjCpfEmitente => Emitente.Empresa.Cnpj;

        public TipoEmissao TipoEmissaoCancelamento()
        {
            return TipoEmissao.Normal;
        }

        public string ObterXmlAutorizado()
        {
            return Emissao.XmlAutorizado;
        }

        public Nfce()
        {
            _formaPagamentos = new List<FormaPagamentoNfce>();
            _itens = new List<NfceItem>();

            CriadoEm = DateTime.Now;
            EmitidaEm = DateTime.Now;
            EntradaSaidaEm = DateTime.Now;
            Status = Status.Aberta;
        }

        public IList<NfceItem> ObterOsItens()
        {
            return _itens?.Where(i => i.Cancelado == false).ToList();
        }

        public IList<NfceItem> ObterTodosItens()
        {
            return _itens.ToList();
        }

        public void AddItens(IEnumerable<NfceItem> itens)
        {
            if (_itens == null) _itens = new List<NfceItem>();

            itens.ForEach(_itens.Add);
        }

        public IList<FormaPagamentoNfce> ObterOsPagamentos()
        {
            return _formaPagamentos;
        }

        public decimal ObterTotalPago()
        {
            var total = 0.0m;

            _formaPagamentos.ForEach(fp =>
            {
                if (fp.IsAjuste) return;

                total += fp.ValorPagamento;
            });

            return total;
        }

        public void AddFormaPagamento(FormaPagamentoNfce formaPagamento)
        {
            if (_formaPagamentos == null) _formaPagamentos = new List<FormaPagamentoNfce>();

            _formaPagamentos.Add(formaPagamento);
        }

        public void DistribuiDesconto(decimal valorDesconto)
        {
            var total = ObterOsItens().Sum(i => i.ValorTotal);

            var k = valorDesconto/total;

            ObterOsItens().ForEach(i =>
            {
                var preco = i.FactoryPrecoItem();

                preco.Desconto = decimal.Round(i.ValorTotal * k + i.Desconto, 2);
                i.DefinirPreco(preco);
            });

            AjustarDescontoUltimoItem(valorDesconto);
        }

        public void EstornarDesconto()
        {
            ObterOsItens().ForEach(i =>
            {
                var preco = i.FactoryPrecoItem();
                preco.Desconto = 0;
                i.DefinirPreco(preco);
            });
        }

        public void EstornarAcrescimo()
        {
            ObterOsItens().ForEach(i =>
            {
                var preco = i.FactoryPrecoItem();

                preco.Acrescimo = 0;
                i.DefinirPreco(preco);
            });
        }

        private void AjustarDescontoUltimoItem(decimal valorDesconto)
        {
            var ultimoItem = ObterOsItens().LastOrDefault();

            if (ultimoItem == null) return;

            var somaDesconto = ObterOsItens().Sum(s => s.Desconto);

            var precoItem = ultimoItem.FactoryPrecoItem();
            precoItem.Desconto += valorDesconto - somaDesconto;
            ultimoItem.DefinirPreco(precoItem);
        }

        public void DistribuiAcrescimo(decimal valorAcrescimo)
        {
            var total = ObterOsItens().Sum(i => i.ValorTotal);

            var k = valorAcrescimo / total;

            ObterOsItens().ForEach(i =>
            {
                var preco = i.FactoryPrecoItem();

                preco.Acrescimo = decimal.Round(i.ValorTotal * k + i.Acrescimo, 2);
                i.DefinirPreco(preco);
            });

            AjustarAcrescimoUltimoItem(valorAcrescimo);
        }

        private void AjustarAcrescimoUltimoItem(decimal valorAcrescimo)
        {
            var ultimoItem = ObterOsItens().LastOrDefault();

            if (ultimoItem == null) return;

            var somaAcrescimo = ObterOsItens().Sum(s => s.Acrescimo);

            var precoItem = ultimoItem.FactoryPrecoItem();
            precoItem.Acrescimo += valorAcrescimo - somaAcrescimo;
            ultimoItem.DefinirPreco(precoItem);
        }

        protected override int ReferenciaUnica => Id;
        public bool Sincronizado { get; set; }
        public bool Denegada { get; set; }
        public VendedorNfce Vendedor { get; set; }

        public void RemoveFormasPagamento()
        {
            _formaPagamentos = new List<FormaPagamentoNfce>();
        }

        public void ZerarDescontoItensRateado()
        {
            _itens.ForEach(p =>
            {
                var precoItem = p.FactoryPrecoItem();
                precoItem.Desconto = 0;
                p.DefinirPreco(precoItem);
            });
        }

        public bool ExisteCobranca()
        {
            return Cobranca != null;
        }

        public bool ExisteCobrancaQueGeraFinancerio()
        {
            return Cobranca?.TipoDocumento?.RegistraFinanceiro == true;
        }

        public bool NaoExisteCobranca()
        {
            return !ExisteCobranca();
        }

        public void RemoveCobranca()
        {
            Cobranca = null;
        }

        public void ZerarAcrescimoItensRateado()
        {
            _itens.ForEach(p =>
            {
                var precoItem = p.FactoryPrecoItem();
                precoItem.Acrescimo = 0;
                p.DefinirPreco(precoItem);
            });
        }

        public IList<FormaPagamentoNfce> ObterFormaPagamentoNfces()
        {
            return _formaPagamentos;
        }

        public bool IsTemFormaPagamentoTef()
        {
            return _formaPagamentos.Any(x => x.IdFormaPagamento == "09");
        }

        public void CalculaImpostos(IRepositorioNfce repositorioNfce)
        {
            if (RegimeTributario == RegimeTributario.SimplesNacional) return;

            foreach (var item in ObterOsItens())
            {
                var impostoIcms = item.ImpostoIcms;
                var impostoCofins = item.ImpostoCofins;
                var impostoPis = item.ImpostoPis;

                var produtoTributavel = new ProdutoTributavelCalculadora
                {
                    QuantidadeProduto = item.Quantidade,
                    Documento = Documento.NFe,
                    Desconto = item.DescontoTotal,
                    OutrasDespesas = item.Acrescimo,
                    PercentualIcms = impostoIcms.AliquotaIcms,
                    ValorProduto = item.ValorUnitario,
                    PercentualCofins = item.ImpostoCofins.Aliquota,
                    PercentualPis = item.ImpostoPis.Aliquota
                };

                CalculaCST(impostoIcms, produtoTributavel, item);

                CalculaCofins(impostoCofins, produtoTributavel);

                CalculaPis(impostoPis, produtoTributavel);

                repositorioNfce.SalvarItem(item);
            }
        }

        private static void CalculaPis(NfceImpostoPis impostoPis, ProdutoTributavelCalculadora produtoTributavel)
        {
            switch (impostoPis.Pis.Id)
            {
                case "01":
                case "02":
                case "49":
                    var facade = new FacadeCalculadoraTributacao(produtoTributavel);
                    var resultadoCalculoPis = facade.CalculaPis();

                    impostoPis.BaseCalculo = decimal.Round(resultadoCalculoPis.BaseCalculo, 2);
                    impostoPis.Valor = decimal.Round(resultadoCalculoPis.Valor, 2);
                    break;
            }
        }

        private static void CalculaCofins(NfceImpostoCofins impostoCofins, ProdutoTributavelCalculadora produtoTributavel)
        {
            switch (impostoCofins.Cofins.Id)
            {
                case "01":
                case "02":
                case "49":
                    var facade = new FacadeCalculadoraTributacao(produtoTributavel);
                    var resultadoCalculoCofins = facade.CalculaCofins();

                    impostoCofins.BaseCalculo = decimal.Round(resultadoCalculoCofins.BaseCalculo, 2);
                    impostoCofins.Valor = decimal.Round(resultadoCalculoCofins.Valor, 2);
                    break;
            }
        }

        private static void CalculaCST(INfceImpostoIcms imposto, ProdutoTributavelCalculadora produtoTributavel, NfceItem item)
        {
            switch (imposto.CST.Id)
            {
                case "00":
                    produtoTributavel.Cst = Cst.Cst00;
                    var cst00 = new Cst00(item.ImpostoIcms.OrigemMercadoria.ToMotorTributario());
                    cst00.Calcula(produtoTributavel);

                    imposto.BcIcms = decimal.Round(cst00.ValorBcIcms, 2);
                    imposto.ValorIcms = decimal.Round(cst00.ValorIcms, 2);

                    break;
                case "20":
                    produtoTributavel.PercentualReducao = imposto.ReducaoBcIcms;
                    produtoTributavel.Cst = Cst.Cst20;
                    var cst20 = new Cst20(item.ImpostoIcms.OrigemMercadoria.ToMotorTributario());
                    cst20.Calcula(produtoTributavel);

                    imposto.BcIcms = decimal.Round(cst20.ValorBcIcms, 2);
                    imposto.ValorIcms = decimal.Round(cst20.ValorIcms, 2);
                    break;

                case "90":
                    produtoTributavel.PercentualReducao = imposto.ReducaoBcIcms;
                    produtoTributavel.Cst = Cst.Cst90;
                    var cst90 = new Cst90(item.ImpostoIcms.OrigemMercadoria.ToMotorTributario());
                    cst90.Calcula(produtoTributavel);

                    imposto.BcIcms = decimal.Round(cst90.ValorBcIcms, 2);
                    imposto.ValorIcms = decimal.Round(cst90.ValorIcms, 2);
                    break;
            }
        }

        public IEnumerable<OperacaoCaixa> ObterOperacoes()
        {
            foreach (var pg in _formaPagamentos)
            {
                if (pg.IdFormaPagamento == FormaPagamentoNfce.AjusteSaldo)
                {
                    continue;
                }

                var op = new OperacaoCaixa(DateTime.Now, ConverterEspecie(pg), EOrigemFluxoCaixaIndividual.Nfce, pg.ValorPagamento);

                pg.IsGerouRegistroCaixa = true;

                yield return op;
            }
        }

        private ETipoPagamento ConverterEspecie(FormaPagamentoNfce pg)
        {
            if (pg.IdFormaPagamento == FormaPagamentoNfce.Dinheiro)
                return ETipoPagamento.Dinheiro;

            if (pg.IdFormaPagamento == FormaPagamentoNfce.Crediario)
                return ETipoPagamento.CreditoLoja;

            if (pg.IdFormaPagamento == FormaPagamentoNfce.CartaoDebito)
                return ETipoPagamento.CartaoDebito;

            if (pg.IdFormaPagamento == FormaPagamentoNfce.CartaoCredito)
                return ETipoPagamento.CartaoCredito;

            if (pg.IdFormaPagamento == FormaPagamentoNfce.Outros)
                return ETipoPagamento.Dinheiro;

            if (pg.IdFormaPagamento == FormaPagamentoNfce.CartaoPos)
                return ETipoPagamento.CartaoCredito;

            if (pg.IdFormaPagamento == FormaPagamentoNfce.CartaoTef)
                return ETipoPagamento.CartaoCredito;

            if (pg.IdFormaPagamento == FormaPagamentoNfce.Pix)
                return ETipoPagamento.Pix;

            return ETipoPagamento.Dinheiro;
        }

        public void RemoverItem(NfceItem nfceItem)
        {
            _itens.Remove(nfceItem);
        }

        public void RecalcularComTabelaPreco(RepositorioTabelaPrecoNfce repositorioTabelaPrecoNfce
            , IRepositorioIbptNfce repositorioIbptNfce)
        {
            _itens.ForEach(item =>
            {
                item.PrecoVenda = item.Produto.PrecoVenda;
                var precoItem = item.FactoryPrecoItem();

                precoItem.DescontoAlteraItem = 0.0m;

                if (item.Nfce.TabelaPreco == null)
                    precoItem.ValorUnitario = item.PrecoVenda;

                if (item.Nfce.TabelaPreco != null)
                    AtualizaPrecosComTabelaPreco.AjusteTabelaPreco(
                        item.Nfce.TabelaPreco
                        , repositorioTabelaPrecoNfce
                        , item.Produto
                        , new AtualizaPrecosCalculadosPorTabelaPreco(item, precoItem));

                item.DefinirPreco(precoItem);

                item.CalculaIbpt(repositorioIbptNfce);
            });
        }

        public bool IsMultiPagamento()
        {
            return ObterOsPagamentos().Count(p => p.IdFormaPagamento == FormaPagamentoNfce.CartaoPos) > 1;
        }

        public void TransmitidaComSucesso()
        {
            Status = Status.Transmitida;
        }

        public void FoiCancelada()
        {
            Status = Status.Cancelada;
        }

        public void PendenteOfflineRejeitada()
        {
            Status = Status.PendenteOffline;
        }
    }
}