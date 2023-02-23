using FusionCore.Extencoes;
using FusionCore.FusionAdm.Fiscal.Fabricas;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.FusionAdm.TabelasDePrecos.NfceSync;
using FusionCore.FusionNfce.Cfop;
using FusionCore.FusionNfce.Fiscal.Tributacoes;
using FusionCore.FusionNfce.Produto;
using FusionCore.FusionNfce.Venda;
using FusionCore.Helpers.Basico;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Tributacoes.Flags;

// ReSharper disable ConvertToAutoPropertyWithPrivateSetter
// ReSharper disable ConvertToAutoProperty

namespace FusionCore.FusionNfce.Fiscal
{
    public class NfceItem : Entidade, IAtualizaPrecoVenda
    {
        private decimal _valorTotal;
        private decimal _desconto;
        private decimal _acrescimo;
        private decimal _quantidade;
        private decimal _valorUnitario;
        private decimal _descontoAlteraItem;
        private INfceImpostoIcms _impostoIcms;
        private NfceImpostoCofins _impostoCofins;
        private NfceImpostoPis _impostoPis;

        protected NfceItem()
        {

        }

        public int Id { get; set; }
        public Nfce Nfce { get; set; }
        public ProdutoNfce Produto { get; set; }
        public CfopNfce Cfop { get; set; }
        public short NumeroItem { get; set; }
        public string Gtin { get; set; }
        public string CodigoNcm { get; set; }
        public string CodigoCest { get; set; }
        public string Nome { get; set; }
        public string SiglaUnidade { get; set; }
        public string SiglaUnidadeTributavel { get; set; } = string.Empty;
        public bool Cancelado { get; set; }
        public decimal ValorTributoEstadual { get; set; }
        public decimal ValorTributoFederal { get; set; }
        public decimal ValorTributoAproximado { get; set; }
        public decimal PrecoCusto { get; set; }
        public decimal PrecoVenda { get; set; }
        public string Observacao { get; set; }

        public NfceImpostoCofins ImpostoCofins
        {
            get => _impostoCofins;
            set
            {
                _impostoCofins = value;
                if (value != null)
                    _impostoCofins.Item = this;
            }
        }

        public NfceImpostoPis ImpostoPis
        {
            get => _impostoPis;
            set
            {
                _impostoPis = value;
                if (value != null)
                    _impostoPis.Item = this;
            }
        }

        public decimal ValorTotal => _valorTotal;
        public decimal Desconto => _desconto;
        public decimal Acrescimo => _acrescimo;
        public decimal Quantidade => _quantidade;
        public decimal ValorUnitario => _valorUnitario;
        public decimal DescontoAlteraItem => _descontoAlteraItem;
        public decimal DescontoTotal => Desconto + DescontoAlteraItem;

        public string CodigoBarrasOuCodigo => GetCodigoBarrasOuCodigo();

        public INfceImpostoIcms ImpostoIcms
        {
            get => _impostoIcms;
            set
            {
                _impostoIcms = value;
                _impostoIcms.Item = this;
            }
        }

        public decimal TotalUnitario => ValorTotal - DescontoAlteraItem;

        protected override int ReferenciaUnica => Id;

        private string GetCodigoBarrasOuCodigo()
        {
            return ExtValidaListasEColecoes.IsNotNullOrEmpty(Gtin) ? Gtin : Produto.Id.ToString();
        }

        public static NfceItem ConstroiNfceItem(
            ItemEspera itemEspera,
            int count,
            Nfce nfce,
            IRepositorioIbptNfce repositorioIbpt,
            RepositorioTabelaPrecoNfce repositorioTabelaPrecoNfce
        ) {
            var produto = itemEspera.Produto;

            var csosn = produto.RegraSaida.SituacaoTributariaCsosn;
            var cst = produto.RegraSaida.SituacaoTributariaIcms;
            var cfop = produto.RegraSaida.Cfop;
            var impostoIcms = CriaTributacaoCst.CriaCsosnNfce(nfce.RegimeTributario != RegimeTributario.SimplesNacional
                ? cst : csosn, produto);

            var impostoPis = CriaTributacaoCst.CriaPisNfce(produto);
            var impostoCofins = CriaTributacaoCst.CriaCofinsNfce(produto);

            impostoIcms.OrigemMercadoria = produto.OrigemMercadoria;

            var preco = PrecoItem.Factory(produto.Quantidade, produto.PrecoVenda);

            const string semGtin = "SEM GTIN";
            var gtin = produto.IsTemCodigoDeBarras() ? produto.ObterPrimeiroCodigoDeBarras().Alias : semGtin;

            if (gtin != semGtin)
            {
                gtin = Gs1GtinHelper.EhUmGtinValido(gtin) ? gtin : semGtin;
            }



            var item = new NfceItem
            {
                CodigoCest = produto.Cest,
                CodigoNcm = produto.Ncm,
                Nome = produto.Nome.TrimSefaz(120),
                Gtin = gtin,
                SiglaUnidade = produto.UnidadeMedida.Sigla,
                Produto = produto,
                NumeroItem = short.Parse(count.ToString()),
                Nfce = nfce,
                ImpostoIcms = impostoIcms,
                Cfop = cfop,
                PrecoCusto = produto.PrecoCompra,
                PrecoVenda = produto.PrecoVenda,
                ImpostoCofins = impostoCofins,
                ImpostoPis = impostoPis,
                Observacao = produto.UsarObservacaoNoItemFiscal ? produto.Observacao : string.Empty
            };

            AtualizaPrecosComTabelaPreco.AjusteTabelaPreco(nfce.TabelaPreco
                , repositorioTabelaPrecoNfce
                , produto
                , new AtualizaPrecosCalculadosPorTabelaPreco(item, preco));

            if (produto.UnidadeMedidaTributavel != null)
            {
                item.SiglaUnidadeTributavel = produto.UnidadeMedidaTributavel.Sigla;
                item.QuantidadeUnidadeTributavel = produto.QuantidadeUnidadeTributavel;
            }

            item.DefinirPreco(preco);

            item.CalculaIbpt(repositorioIbpt);
            item.ImpostoIcms.CST = item.ImpostoIcms.CST;

            return item;
        }

        public decimal QuantidadeUnidadeTributavel { get; set; }

        public void CalculaIbpt(IRepositorioIbptNfce repositorioIbpt)
        {
            ValorTributoEstadual = 0;
            ValorTributoFederal = 0;

            var ibpt = repositorioIbpt.GetPeloNcm(CodigoNcm);

            if (ibpt == null) return;

            ValorTributoEstadual = ibpt.ImpostoEstadualAproximado(this);
            ValorTributoFederal = ibpt.ImpostoFederalAproximado(this);
            ValorTributoAproximado = ValorTributoEstadual + ValorTributoFederal;
        }

        public decimal GetValorBaseCalculo()
        {
            return ValorTotal;
        }

        public void DefinirPreco(PrecoItem preco)
        {
            _quantidade = preco.Quantidade;
            _valorUnitario = preco.ValorUnitario;
            _desconto = preco.Desconto;
            _acrescimo = preco.Acrescimo;
            _valorTotal = preco.TotalBruto;
            _descontoAlteraItem = preco.DescontoAlteraItem;
        }

        public PrecoItem FactoryPrecoItem()
        {
            var bruto = ValorUnitario * Quantidade;
            var liquido = bruto + Acrescimo - (Desconto + DescontoAlteraItem);
            var totalUnitarioSemDescontoGeral = (ValorUnitario * Quantidade) - DescontoAlteraItem;;

            return PrecoItem.Factory(Quantidade, ValorUnitario, Desconto, Acrescimo, bruto, liquido, _descontoAlteraItem, totalUnitarioSemDescontoGeral);
        }

        public string GetSiglaUfConsumo()
        {
            if (Nfce.Destinatario == null)
                return Nfce.Emitente.Empresa.Cidade.SiglaUf;

            if (Nfce.Destinatario != null && Nfce.Destinatario.Cidade == null)
                return Nfce.Emitente.Empresa.Cidade.SiglaUf;

            return Nfce.Destinatario.Cidade.SiglaUf;
        }

        public string ObterSiglaUnidadeTributavel()
        {
            return SiglaUnidadeTributavel.IsNotNullOrEmpty() ? SiglaUnidadeTributavel : SiglaUnidade;
        }

        public void AtualizarPrecoVenda(decimal novoPrecoVenda)
        {
            PrecoVenda = novoPrecoVenda;
        }
    }
}