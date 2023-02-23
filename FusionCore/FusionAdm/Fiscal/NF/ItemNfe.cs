using System.Linq;
using FusionCore.Core.Estoque;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Flags;
using OpenAC.Net.Core.Extensions;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

namespace FusionCore.FusionAdm.Fiscal.NF
{
    public class ItemNfe : Entidade, IMovimentavel, IAtualizaValorUnitario, IAtualizaPrecoVenda
    {
        private bool _movimentarEstoqueConfiguracao = true;

        private ItemNfe()
        {
            //constructor for nhibernate
            Observacao = string.Empty;
            AutoAjustarImposto = true;
        }

        public ItemNfe(Nfeletronica nfe) : this()
        {
            Nfe = nfe;
            NumeroItem = Nfe.Itens.Any() ? Nfe.Itens.Max(i => i.NumeroItem) + 1 : 1;
        }

        public int Id { get; private set; }
        public Nfeletronica Nfe { get; private set; }
        public PerfilCfopDTO Cfop { get; set; }
        public bool MovimentaEstoque { get; set; }
        public bool PartilharIcms { get; set; }
        public int NumeroItem { get; set; }
        public string NumeroPedido { get; set; } = string.Empty;
        public int NumeroItemPedido { get; set; }
        public decimal ValorDespesasFixaRateio { get; set; }
        public decimal ValorDescontoFixoRateio { get; set; }
        public decimal ValorFreteFixoRateio { get; set; }
        public decimal ValorSeguroFixoRateio { get; set; }
        public ProdutoDTO Produto { get; private set; }
        public string SiglaUnidade { get; private set; }
        public string SiglaUnidadeTributavel { get; private set; }
        public decimal QuantidadeUnidadeTributavel { get; private set; }
        public string CodigoUtilizado { get; set; }
        public string CodigoBarras { get; set; }
        public decimal PrecoCusto { get; private set; }
        public decimal PrecoVenda { get; private set; }
        public decimal ValorUnitario { get; private set; }
        public decimal Quantidade { get; private set; }
        public decimal TotalBruto { get; private set; }
        public decimal TotalDescontoItem { get; private set; }
        public decimal PorcentagemDescontoItem { get; private set; }
        public decimal TotalItem { get; private set; }
        public decimal TotalTributavel => TotalItem - ValorDescontoFixoRateio;
        public decimal TotalFiscal { get; private set; }
        public ImpostoPis Pis { get; set; }
        public ImpostoCofins Cofins { get; set; }
        public ImpostoIpi Ipi { get; set; }
        public ImpostoIcms ImpostoIcms { get; set; }
        public IcmsInterstadual IcmsInterstadual { get; set; }
        protected override int ReferenciaUnica => Id;
        public string Observacao { get; set; }
        public bool HasObservacao => !string.IsNullOrWhiteSpace(Observacao);
        public string CodigoBeneficioFiscal { get; set; }
        public bool AutoAjustarImposto { get; set; }

        public bool MovimentarEstoqueConfiguracao
        {
            get => _movimentarEstoqueConfiguracao;
            set
            {
                MovimentaEstoque = value;
                _movimentarEstoqueConfiguracao = value;
            }
        }

        public bool AutoAtivarCreditoItem { get; set; }
        public bool AutoCalcularTotaisItem { get; set; } = true;
        public bool UsarIpiTagPropria { get; set; }

        public void ComMercadoria(
            ProdutoDTO produto,
            decimal quantidade,
            decimal valorUnitario,
            decimal totalDesconto,
            decimal? totalManual = null)
        {
            Produto = produto;
            SiglaUnidade = produto.ProdutoUnidadeDTO.Sigla;
            QuantidadeUnidadeTributavel = 0;
            SiglaUnidadeTributavel = string.Empty;

            if (produto.ProdutoUnidadeTributavel != null)
            {
                SiglaUnidadeTributavel = produto.ProdutoUnidadeTributavel.Sigla;
                QuantidadeUnidadeTributavel = produto.QuantidadeUnidadeTributavel;
            }

            PrecoCusto = produto.PrecoCusto == 0 ? produto.PrecoCompra : produto.PrecoCusto;
            PrecoVenda = produto.PrecoVenda;

            AlterarQuantidade(quantidade);
            ValorUnitario = decimal.Round(valorUnitario, 10);
            TotalDescontoItem = decimal.Round(totalDesconto, 2);

            if (AutoCalcularTotaisItem == false)
            {
                TotalBruto = totalManual.Value;
                TotalItem = totalManual.Value;
                return;
            }

            CalcularTotais();
            CalcularPorcentagemDesconto();
        }

        private void AlterarQuantidade(decimal quantidade)
        {
            Quantidade = decimal.Round(quantidade, 4);
        }

        public void ComBarras(string codigoBarras)
        {
            if (codigoBarras == null || codigoBarras.ToUpper() == "SEM GTIN")
            {
                CodigoBarras = string.Empty;
                return;
            }

            CodigoBarras = codigoBarras;
        }

        public void ComCodigoUtilizado(string codigo)
        {
            CodigoUtilizado = codigo ?? string.Empty;
        }

        public void CalcularPorcentagemDesconto()
        {
            if (TotalBruto <= 0)
            {
                PorcentagemDescontoItem = 0M;
                return;
            }

            var p = (TotalDescontoItem / TotalBruto) * 100;

            PorcentagemDescontoItem = decimal.Round(p, 10);
        }

        public EstoqueModel CriaMovimentoInclusao()
        {
            var model = new EstoqueModel(
                Produto,
                Quantidade,
                SessaoEstoque.UsuarioEvento,
                OrigemEventoEstoque.ItemAdicionadoNfe
            );

            model.Inverso = Nfe.TipoOperacao == TipoOperacao.Saida;

            return model;
        }

        public EstoqueModel CriaMovimentoRemocao()
        {
            var model = new EstoqueModel(
                Produto,
                Quantidade,
                SessaoEstoque.UsuarioEvento,
                OrigemEventoEstoque.ItemRemovidoNfe
            );

            model.Inverso = Nfe.TipoOperacao == TipoOperacao.Saida;

            return model;
        }

        public bool IsNovo()
        {
            return Id <= 0;
        }

        public string GetSiglaUfDestino()
        {
            return Nfe.Destinatario.Endereco.Localizacao.SiglaUF;
        }

        public void CalcularImpostos()
        {
            CalcularTotais();

            if (!AutoAjustarImposto)
            {
                CalcularValorFiscal();
                return;
            }

            Ipi.AjustarIpi();
            ImpostoIcms.AjustarImposto();
            Pis.AjustarPis();
            Cofins.AjustarCofins();

            CalcularValorFiscal();
        }

        private void CalcularTotais()
        {
            if (AutoCalcularTotaisItem == false) return;

            TotalBruto = Quantidade > 0
                ? decimal.Round(ValorUnitario * Quantidade, 2)
                : decimal.Round(ValorUnitario, 2);

            TotalItem = TotalBruto - TotalDescontoItem;
        }

        private void CalcularValorFiscal()
        {
            TotalFiscal =
                TotalTributavel
                + ValorFreteFixoRateio
                + ValorSeguroFixoRateio
                + ValorDespesasFixaRateio
                + ImpostoIcms.ValorIcmsSt
                + ImpostoIcms.ValorFcpSt
                + Ipi.ValorIpi;
        }

        public string UnidadeTributavelParaNfe()
        {
            if (string.IsNullOrWhiteSpace(SiglaUnidadeTributavel))
            {
                return SiglaUnidade;
            }

            return SiglaUnidadeTributavel;
        }

        public decimal QuantidadeTributavelParaNfe()
        {
            if (string.IsNullOrWhiteSpace(SiglaUnidadeTributavel))
            {
                return Quantidade;
            }

            return Quantidade * QuantidadeUnidadeTributavel;
        }

        public decimal ValorTributavelParaNfe()
        {
            if (string.IsNullOrWhiteSpace(SiglaUnidadeTributavel))
            {
                return ValorUnitario;
            }

            return (TotalBruto / QuantidadeTributavelParaNfe()).RoundABNT(10);
        }

        public void AtualizarValorUnitario(decimal novoValorUnitario)
        {
            ValorUnitario = novoValorUnitario;
        }

        public void AtualizarPrecoVenda(decimal novoPrecoVenda)
        {
            PrecoVenda = novoPrecoVenda;
        }
    }
}