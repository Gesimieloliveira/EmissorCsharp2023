using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using DFe.Ext;
using FusionCore.ControleCaixa;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Fiscal.Contratos;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.Fiscal.NF.Cancelar;
using FusionCore.FusionAdm.Fiscal.NF.Integridade;
using FusionCore.FusionAdm.Fiscal.NF.Pagamentos;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Helpers.Pessoa;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionCore.Tributacoes.Calculadoras;
using FusionLibrary.Helper.Criptografia;
using NHibernate;
using static System.Text.RegularExpressions.RegexOptions;

// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable ConvertToAutoProperty
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.FusionAdm.Fiscal.NF
{
    public sealed class Nfeletronica : Entidade, IDocumentoCancelavel, IVendaRegistravelEmCaixa
    {
        public static class Expressions
        {
            public static readonly Expression<Func<Nfeletronica, object>> Itens = x => x._itens;
        }

        private readonly IList<ItemNfe> _itens = new List<ItemNfe>();
        private readonly IList<ReferenciaNfe> _referencias = new List<ReferenciaNfe>();
        private readonly IList<ReferenciaCf> _referenciasCf = new List<ReferenciaCf>();
        private readonly IList<FormaPagamentoNfe> _pagamentos = new List<FormaPagamentoNfe>();
        private readonly ISessaoManager _sesaoManager;

        private Nfeletronica()
        {
            IncluiCobrancaNoXml = true;
            TipoOperacao = TipoOperacao.Saida;
            FormaPagamento = FormaPagamento.Avista;
            EmitidaEm = DateTime.Now;
            SaidaEm = DateTime.Now;
            TipoDanfe = TipoDanfe.NormalRetrato;
            FinalidadeEmissao = FinalidadeEmissao.Normal;
            InformacaoAdicional = string.Empty;
            ModalidadeFrete = ModalidadeFrete.SemFrete;
        }

        public Nfeletronica(EmitenteNfe emitente, UsuarioDTO usuarioCriacao) : this()
        {
            Emitente = emitente;
            UuidVenda = GuuidHelper.Computar("NF-e" + DateTime.Now.ToString("G"));
            UsuarioCriacao = usuarioCriacao;
        }

        public int Id { get; private set; }
        protected override int ReferenciaUnica => Id;
        public short PerfilId { get; set; }
        public ModeloDocumento Modelo { get; private set; } = ModeloDocumento.NFe;
        public DestinatarioNfe Destinatario { get; set; }
        public EmissaoFinalizadaNfe Finalizacao { get; private set; }
        public EmitenteNfe Emitente { get; private set; }
        public CancelamentoNfe Cancelamento { get; private set; }
        public IList<IVolume> Volumes { get; set; }
        public Cobranca Cobranca { get; set; }
        public Malote Malote { get; set; }
        public short SerieEmissao { get; set; }
        public int NumeroEmissao { get; set; }
        public DateTime EmitidaEm { get; set; }
        public DateTime? SaidaEm { get; set; }
        public bool TemEmissao => Finalizacao != null;
        public string InformacaoAdicional { get; set; }
        public string NaturezaOperacao { get; set; }
        public FormaPagamento FormaPagamento { get; set; }
        public TipoOperacao TipoOperacao { get; set; }
        public TipoDanfe TipoDanfe { get; set; }
        public FinalidadeEmissao FinalidadeEmissao { get; set; }
        public ModalidadeFrete ModalidadeFrete { get; set; }
        public decimal ValorDescontoFixo { get; set; }
        public decimal ValorSeguroFixo { get; set; }
        public decimal ValorFreteFixo { get; set; }
        public decimal ValorDespesasFixa { get; set; }
        public string UuidVenda { get; set; }
        public ExportacaoNfe Exportacao { get; set; }
        public TransportadoraNfe Transportadora { get; set; }
        public int ReferenciaId => Id;
        public int NumeroDocumento => NumeroEmissao;
        public string NumeroProtocolo => Finalizacao.Protocolo;
        public string NumeroChave => Finalizacao.Chave.Chave;
        public string CnpjCpfEmitente => Emitente.DocumentoUnico;
        public bool IncluirInformacaoIbpt { get; set; }
        public UsuarioDTO UsuarioCriacao { get; private set; }
        public bool IncluiCobrancaNoXml { get; set; }
        public StatusNfe StatusAtual { get; set; } = StatusNfe.Pendente;
        public bool PedidoInternoSistema { get; set; }
        public bool SemPagamento { get; set; }
        public LocalEntrega LocalEntrega { get; set; }

        public IEnumerable<ItemNfe> Itens => _itens;
        public IEnumerable<FormaPagamentoNfe> Pagamentos => _pagamentos;
        public IEnumerable<ReferenciaNfe> Referencias => _referencias;
        public IEnumerable<ReferenciaCf> ReferenciasCf => _referenciasCf;

        public bool SujeitoIcmsInterstadual
        {
            get
            {
                if (Destinatario.ResideExterior())
                {
                    return false;
                }

                if (TipoOperacao == TipoOperacao.Entrada || FinalidadeEmissao != FinalidadeEmissao.Normal)
                {
                    return false;
                }

                return Destinatario.ResideForaDoEstado();
            }
        }

        public decimal TotalBcIcms { get; private set; }
        public decimal TotalIcms { get; private set; }
        public decimal TotalBcSt { get; private set; }
        public decimal TotalSt { get; private set; }
        public decimal TotalBcFcpSt { get; private set; }
        public decimal TotalFcpSt { get; private set; }
        public decimal TotalFcp { get; private set; }
        public decimal TotalIpi { get; private set; }
        public decimal TotalCofins { get; private set; }
        public decimal TotalPis { get; private set; }
        public decimal TotalItens { get; private set; }
        public decimal TotalDescontoItens { get; private set; }
        public decimal TotalDescontoFinal { get; private set; }
        public decimal TotalFinal { get; private set; }
        public string DocumentoUnico => GetDocumentoUnico();

        private string GetDocumentoUnico()
        {
            return Emitente.DocumentoUnicoSemZeroAEsquerda;
        }

        public TipoEmissao TipoEmissaoCancelamento()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repostorio = new RepositorioContingenciaNfe(sessao);
                var sessaoManager = new SessaoManagerAdm();
                sessaoManager.GetSessaoAberta();
                var contingenciaAtiva = repostorio.ContingenciaAberta(Emitente.CarregarDadosEmissor(sessaoManager));

                return contingenciaAtiva?.TipoEmissao ?? TipoEmissao.Normal;
            }
        }

        public string ObterXmlAutorizado()
        {
            return Finalizacao.XmlAutorizado;
        }


        public void AdicionarItem(ItemNfe item)
        {
            if (_itens.Any(i => i == item))
            {
                return;
            }

            _itens.Add(item);
        }

        public void RemoveItem(ItemNfe item)
        {
            _itens.Remove(item);
        }

        public void AdicionaReferencia(ReferenciaNfe referencia)
        {
            _referencias.Add(referencia);
        }

        public void AdicionaReferencia(ReferenciaCf referencia)
        {
            _referenciasCf.Add(referencia);
        }

        public void RemoveReferencia(ReferenciaNfe referencia)
        {
            _referencias.Remove(referencia);
        }

        public void RemoveReferencia(ReferenciaCf referencia)
        {
            _referenciasCf.Remove(referencia);
        }

        public string ComputaInformacaoAdicional()
        { 
            var info = new StringBuilder();

            if (InformacaoAdicional.IsNotNullOrEmpty())
                info.Append($"{InformacaoAdicional}");

            var referencias = ComputaObsReferencias().Trim();
            var difal = ComputaObsDifal().Trim();
            var infCredito = ComputarObsCreditoIcms();

            if (!string.IsNullOrWhiteSpace(referencias))
                info.Append($" ;{referencias}");

            if (!string.IsNullOrWhiteSpace(difal))
                info.Append($" ;{difal}");

            if (!string.IsNullOrWhiteSpace(infCredito))
                info.Append($" ;{infCredito}");

            var infoAdicional = info.ToString();

            infoAdicional = new Regex(@"^( ;)", IgnoreCase).Replace(infoAdicional, "");
            infoAdicional = new Regex("[\"]", IgnoreCase).Replace(infoAdicional, "\\\"");
            infoAdicional = new Regex("(\r\n)", IgnoreCase).Replace(infoAdicional, @" ");

            return infoAdicional.ToUpper().TrimSefaz();
        }

        public bool ExisteItemQuePermiteCredito => Itens.Any(x => x.ImpostoIcms.Cst.PermiteCredito());
        public TabelaPreco TabelaPreco { get; set; }

        private string ComputarObsCreditoIcms()
        {
            if (!ExisteItemQuePermiteCredito) return string.Empty;

            var itensComCredito = Itens.Where(i => i.ImpostoIcms.ValorCredito > 0).ToArray();
            var valorCreditoSoma = itensComCredito.Sum(i => i.ImpostoIcms.ValorCredito);

            decimal aliquotaCredito;
            if (itensComCredito.Any(i => i.TotalItem == 0))
            {
                aliquotaCredito = itensComCredito.Average(i => i.ImpostoIcms.AliquotaCredito);
            }
            else
            {
                var valorTotalItens = itensComCredito.Sum(i => i.TotalItem);
                try
                {
                    aliquotaCredito = decimal.Round(valorCreditoSoma / valorTotalItens * 100, 4);
                }
                catch (DivideByZeroException)
                {
                    return string.Empty;
                }
            }

            return $"Permite crédito do ICMS no valor de R$ {valorCreditoSoma:N2}, correspondente à" +
                   $" alíquota de {aliquotaCredito:N2}% nos termos do Art. 23, da LC nº 123/06.";
        }

        private string ComputaObsReferencias()
        {
            var info = string.Empty;

            if (_referenciasCf.Count > 0)
            {
                info += "Cupom Fiscal Referênciado (NúmeroECF/COO):";
                info = _referenciasCf.Aggregate(info, (current, cf) => current + $" {cf.NumeroEcf}/{cf.NumeroCoo}");
            }

            if (_referencias.Count > 0)
            {
                info += "Nota Fiscal Referênciada (Chave):";
                info = _referencias.Aggregate(info, (c, nf) => c + $" {nf.ChaveReferenciada}");
            }

            return info;
        }

        private string ComputaObsDifal()
        {
            var info = string.Empty;

            var totalIcmsDestino = _itens.Sum(i => i.IcmsInterstadual?.ValorIcmsDestino);
            var totalFcp = _itens.Sum(i => i.IcmsInterstadual?.ValorCombatePobreza);
            var totalIcmsOrigem = _itens.Sum(i => i.IcmsInterstadual?.ValorIcmsOrigem);

            if (totalIcmsOrigem <= 0 && totalFcp <= 0 && totalIcmsOrigem <= 0)
            {
                return info;
            }

            info = "Valores totais do ICMS interestadual: DIFAL da UF destino "
                   + totalIcmsDestino
                   + " + FCP "
                   + totalFcp
                   + "; DIFAL da UF Origem "
                   + totalIcmsOrigem;

            return info;
        }

        public bool PossuiNumeroAlocado()
        {
            return NumeroEmissao > 0;
        }

        public void CalcularItens()
        {
            DistribuidorValoresFixo.Distribuir(this);

            var numero = 1;

            foreach (var i in _itens)
            {
                i.NumeroItem = numero++;
                i.CalcularImpostos();
            }

            TotalBcIcms = _itens.Sum(i => i.ImpostoIcms.ValorBcIcms);
            TotalBcSt = _itens.Sum(i => i.ImpostoIcms.ValorBcSt);
            TotalIcms = _itens.Sum(i => i.ImpostoIcms.ValorIcms);
            TotalSt = _itens.Sum(i => i.ImpostoIcms.ValorIcmsSt);
            TotalBcFcpSt = _itens.Sum(i => i.ImpostoIcms.ValorBcFcpSt);
            TotalFcpSt = _itens.Sum(i => i.ImpostoIcms.ValorFcpSt);
            TotalFcp = _itens.Sum(i => i.ImpostoIcms.ValorFcp);
            TotalIpi = _itens.Sum(i => i.Ipi.ValorIpi);
            TotalCofins = _itens.Sum(i => i.Cofins.ValorCofins);
            TotalPis = _itens.Sum(i => i.Pis.ValorPis);
            TotalItens = _itens.Sum(i => i.TotalBruto);
            TotalDescontoItens = _itens.Sum(i => i.TotalDescontoItem);
            TotalDescontoFinal = TotalDescontoItens + ValorDescontoFixo;
            TotalFinal = _itens.Sum(i => i.TotalFiscal);
        }

        public void PrepararParaEmissao(ISessaoManager sessaoManager)
        {
            IcmsDifal.Partilhar(this, sessaoManager);

            using (var sessao = sessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioPessoa(sessao);
                var repositorioNfe = new RepositorioNfe(sessao);

                var pessoa = repositorio.GetPeloId(Destinatario.GetPessoaId());

                Destinatario.Nome = pessoa.Nome;
                Destinatario.InscricaoEstadual = pessoa.InscricaoEstadual;
                Destinatario.DocumentoUnico = pessoa.GetDocumentoUnico();

                PreparaItemNfe(sessao);
                repositorioNfe.SalvarAlteracoes(this);

                transacao.Commit();
            }
        }

        private void PreparaItemNfe(ISession sessao)
        {
            var repositorio = new RepositorioProduto(sessao);

            foreach (var itemNfe in _itens)
            {
                var produto = itemNfe.Produto;

                repositorio.Refresh(produto);
                itemNfe.Ipi.CodigoEnquadramentoLegal = short.Parse(produto.EnquadramentoIpi.Id);
            }
        }

        public void ChecarCobranca()
        {
            if (Cobranca.IsNotNull())
            {
                throw new InvalidOperationException("Existe cobrança adicionada, para continuar delete as cobranças.");
            }
        }

        public string ComputaInformacaoAdicionalFisco()
        {
            var obsfisco = new List<string>();

            if (TotalFcp > 0 || TotalFcpSt > 0)
            {
                obsfisco.Add($"vFCP={TotalFcp:N2}, vFCPST={TotalFcpSt:N2}");
            }

            return string.Join(",", obsfisco);
        }

        public bool IsPago()
        {
            var isPago = _pagamentos.Sum(pag => pag.Valor) == TotalFinal;

            return isPago;
        }

        public bool PossuiPagamento()
        {
            return _pagamentos.Count > 0;
        }

        public void Finalizar(EmissaoFinalizadaNfe finalizacao)
        {
            Finalizacao = finalizacao;

            if (finalizacao.IsDenegado)
            {
                StatusAtual = StatusNfe.Denegada;
                return;
            }

            StatusAtual = StatusNfe.Autorizada;
        }

        public void Cancelar(CancelamentoNfe args)
        {
            if (!args.Status.EstaCancelado)
            {
                throw new InvalidOperationException($"Cancelamento inválido: {args.TextoResposta}");
            }

            StatusAtual = StatusNfe.Cancelada;
            Cancelamento = args;
        }

        public void Pagar(IReadOnlyList<FormaPagamentoNfe> pagamentos)
        {
            foreach (var pg in pagamentos)
            {
                pg.AnexarNfe(this);
                _pagamentos.Add(pg);
            }
        }

        public void PagarEmDinheiro(UsuarioDTO usuario)
        {
            if (PossuiPagamento())
            {
                throw new InvalidOperationException("NF-e já possui pagamentos, não é possível pagar em Dinheiro!");
            }

            var dinheiro = new DinheiroNfe(usuario, TotalFinal);
            dinheiro.AnexarNfe(this);

            _pagamentos.Add(dinheiro);
        }

        public void RemoverPagamentos()
        {
            _pagamentos.Clear();
        }

        public IEnumerable<OperacaoCaixa> ObterOperacoes()
        {
            if (TipoOperacao == TipoOperacao.Entrada ||
                FinalidadeEmissao == FinalidadeEmissao.Devolucao ||
                SemPagamento == true)
            {
                yield break;
            }

            foreach (var pg in Pagamentos)
            {
                yield return new OperacaoCaixa(pg.CriadoEm, pg.Especie, EOrigemFluxoCaixaIndividual.Nfe, pg.Valor);
            }
        }

        public void CalcularCreditoItens()
        {
            foreach (var itemNfe in _itens)
            {
                if (!itemNfe.AutoAjustarImposto)
                {
                    continue;
                }

                var credito = new CalculadoraCredito
                {
                    ValorTributavel = itemNfe.TotalTributavel,
                    Aliqutoa = itemNfe.ImpostoIcms.AliquotaCredito
                };

                itemNfe.ImpostoIcms.ValorCredito = credito.Calcula().Valor;
            }
        }

        public void RecalcularComtabelaPreco(RepositorioTabelaPreco repositorioTabelaPreco)
        {
            if (TabelaPreco == null)
            {
                foreach (var itemNfe in _itens)
                {
                    itemNfe.ComMercadoria(itemNfe.Produto, itemNfe.Quantidade, itemNfe.Produto.PrecoVenda, 0);
                }

                CalcularItens();
                return;
            }

            foreach (var itemNfe in _itens)
            {
                AtualizaPrecosComTabelaPreco.AjusteTabelaPreco(TabelaPreco,
                    repositorioTabelaPreco,
                    itemNfe.Produto,
                    new AtualizaPrecosCalculadosPorTabelaPreco(itemNfe, itemNfe));

                itemNfe.ComMercadoria(itemNfe.Produto, itemNfe.Quantidade, itemNfe.ValorUnitario, 0);
            }

            CalcularItens();
        }

        public decimal CalcularTotalIpi()
        {
            return Itens.Where(x => x.UsarIpiTagPropria == false).Sum(x => x.Ipi.ValorIpi);
        }

        public decimal CalcularTotalIpiDevolucao()
        {
            return Itens.Where(x => x.UsarIpiTagPropria == true).Sum(x => x.Ipi.ValorIpi);
        }

        public bool ContemIpiDevolucao()
        {
            return Itens.Any(x => x.UsarIpiTagPropria == true);
        }
    }
}