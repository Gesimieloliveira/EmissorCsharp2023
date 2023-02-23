using System;
using System.Linq;
using Fusion.Controles.Objetos;
using Fusion.Visao.NotaFiscalEletronica.Principal.Controles.Events;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Produtos;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Controles
{
    public class ItemContexto : ViewModel
    {
        private readonly ISessaoManager _sessaoManager = new SessaoManagerAdm();
        private ProdutoDTO _produto;
        private PerfilCfopDTO _cfopDoPerfil;
        private bool _autoCalcularTotaisItem;

        public event EventHandler<ITabelaPreco> AtualizaTabelaPreco; 

        public ItemContexto()
        {
            SetValue(true, nameof(IsNovo));
            SetValue(1.00M, nameof(Quantidade));
        }

        public bool IsNovo
        {
            get => GetValue<bool>();
            private set => SetValue(value);
        }

        public ProdutoCombo ProdutoSelecionado
        {
            get => GetValue<ProdutoCombo>();
            set
            {
                SetValue(value);
                CarregarProdutoSelecionado();
            }
        }

        public PerfilCfopDTO CfopSelecionado
        {
            get => GetValue<PerfilCfopDTO>();
            set
            {
                SetValue(value);

                if (CodigoCfop != value?.Codigo)
                {
                    SetValue(value?.Codigo, nameof(CodigoCfop));
                }
            }
        }

        public string CodigoCfop
        {
            get => GetValue<string>();
            private set => SetValue(value);
        }

        public string CodigoBarras
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string SiglaUnidade
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public decimal Quantidade
        {
            get => GetValue<decimal>();
            set
            {
                if (value == Quantidade) return;

                SetValue(decimal.Round(value, 4));
                DefinirTotal();
            }
        }

        public decimal ValorUnitario
        {
            get => GetValue<decimal>();
            set
            {
                if (value == ValorUnitario) return;

                SetValue(decimal.Round(value, 10));
                DefinirTotal();
            }
        }

        public decimal PorcentagemDesconto
        {
            get => GetValue<decimal>();
            set
            {
                if (value == PorcentagemDesconto) return;

                SetValue(decimal.Round(value, 10));

                DefinirDesconto();
                DefinirTotal();
            }
        }

        public decimal TotalDesconto
        {
            get => GetValue<decimal>();
            set
            {
                if (value == TotalDesconto) return;

                SetValue(decimal.Round(value, 2));

                DefinirPorcentagemDesconto();
                DefinirTotal();
            }
        }

        public decimal Total
        {
            get => GetValue<decimal>();
            set
            {
                if (value == Total) return;

                SetValue(decimal.Round(value, 2));
                SetValue(0M, nameof(TotalDesconto));
                SetValue(0M, nameof(PorcentagemDesconto));

                DefinirValorUnitario();
            }
        }

        public string Observacao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string CodBeneficioFiscal
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string NumeroPedido
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int? NumeroItemPedido
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        public TipoOperacao TipoOperacao
        {
            get => GetValue<TipoOperacao>();
            set => SetValue(value);
        }

        public DestinoOperacao DestinoOperacao
        {
            get => GetValue<DestinoOperacao>();
            set => SetValue(value);
        }

        public FinalidadeEmissao FinalidadeEmissao
        {
            get => GetValue<FinalidadeEmissao>();
            set => SetValue(value);
        }

        public bool AutoCalcularTotaisItem
        {
            get => _autoCalcularTotaisItem;
            set
            {
                _autoCalcularTotaisItem = value;
                PropriedadeAlterada();

                SetValue(1.0M, nameof(Quantidade));
                SetValue(0M, nameof(TotalDesconto));
                SetValue(0M, nameof(PorcentagemDesconto));
                CarregaValorUnitarioProduto();
                SetValue(CalculoTotalBruto(), nameof(Total));
            }
        }

        public event EventHandler<ProdutoAlteradoEvent> ProdutoAlterado;

        private void DefinirValorUnitario()
        {
            if (NaoCalcularAutomaticamente()) return;

            if (Quantidade == 0)
            {
                SetValue(Total, nameof(ValorUnitario));
                return;
            }

            var unitario = decimal.Round(Total / Quantidade, 10);

            SetValue(unitario, nameof(ValorUnitario));
        }

        private bool NaoCalcularAutomaticamente()
        {
            return AutoCalcularTotaisItem == false;
        }

        private void DefinirDesconto()
        {
            if (NaoCalcularAutomaticamente()) return;

            var tBruto = CalculoTotalBruto();
            var tDesconto = decimal.Round(tBruto * (PorcentagemDesconto / 100), 2);

            SetValue(tDesconto, nameof(TotalDesconto));
        }

        private void DefinirPorcentagemDesconto()
        {
            if (NaoCalcularAutomaticamente()) return;

            if (Total == 0)
            {
                SetValue(0.00M, nameof(PorcentagemDesconto));
                return;
            }

            var pDesconto = (TotalDesconto / CalculoTotalBruto()) * 100;

            SetValue(decimal.Round(pDesconto, 10), nameof(PorcentagemDesconto));
        }

        private void DefinirTotal()
        {
            if (NaoCalcularAutomaticamente()) return;

            var bruto = CalculoTotalBruto();
            var total = bruto - TotalDesconto;

            SetValue(decimal.Round(total, 2), nameof(Total));
        }

        private decimal CalculoTotalBruto()
        {
            return Quantidade > 0
                ? decimal.Round(Quantidade * ValorUnitario, 2)
                : decimal.Round(ValorUnitario, 2);
        }

        public void Com(ItemNfe item)
        {
            SetValue(false, nameof(IsNovo));

            _produto = item.Produto;

            var combo = new ProdutoCombo(_produto, item.CodigoUtilizado);

            SetValue(item.AutoCalcularTotaisItem, nameof(AutoCalcularTotaisItem));
            SetValue(combo, nameof(ProdutoSelecionado));
            SetValue(item.CodigoBarras, nameof(CodigoBarras));
            SetValue(item.Cfop, nameof(CfopSelecionado));
            SetValue(item.Cfop.Codigo, nameof(CodigoCfop));
            SetValue(item.SiglaUnidade, nameof(SiglaUnidade));
            SetValue(item.CodigoBeneficioFiscal, nameof(CodBeneficioFiscal));
            SetValue(item.NumeroPedido, nameof(NumeroPedido));
            SetValue(item.NumeroItemPedido, nameof(NumeroItemPedido));
            SetValue(item.Observacao, nameof(Observacao));
            SetValue(item.Quantidade, nameof(Quantidade));
            SetValue(item.ValorUnitario, nameof(ValorUnitario));
            SetValue(item.TotalDescontoItem, nameof(TotalDesconto));
            SetValue(item.PorcentagemDescontoItem, nameof(PorcentagemDesconto));
            SetValue(item.TotalItem, nameof(Total));
        }

        private void CarregarProdutoSelecionado()
        {
            if (ProdutoSelecionado == null)
            {
                LimparDadosProduto();
                return;
            }

            _produto = ProdutoSelecionado.CarregaProduto();

            CarregaValorUnitarioProduto();
            SiglaUnidade = _produto.ProdutoUnidadeDTO.Sigla;

            PreencherBarras();
            PreencherCfop();

            ProdutoAlterado?.Invoke(this, new ProdutoAlteradoEvent(_produto, this));
        }

        private void CarregaValorUnitarioProduto()
        {
            if (_produto == null) return;

            ValorUnitario = _produto.PrecoVenda;
        }

        private void LimparDadosProduto()
        {
            _produto = null;

            SetValue<object>(null, nameof(CfopSelecionado));
            SetValue(string.Empty, nameof(CodigoCfop));
            SetValue(string.Empty, nameof(SiglaUnidade));
            SetValue(string.Empty, nameof(CodigoBarras));
            SetValue(string.Empty, nameof(Observacao));
            SetValue(string.Empty, nameof(CodBeneficioFiscal));
            SetValue(0.00M, nameof(ValorUnitario));
            SetValue(0.00M, nameof(PorcentagemDesconto));
            SetValue(0.00M, nameof(TotalDesconto));
            SetValue(0.00M, nameof(Total));

            if (Quantidade != 1)
            {
                SetValue(1.00M, nameof(Quantidade));
            }
        }

        private void PreencherBarras()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioProduto(sessao);
                var aliases = repositorio.BuscarAliasesPorProduto(_produto);

                var firstAlias = aliases.FirstOrDefault(i => i.IsCodigoBarras);

                CodigoBarras = firstAlias?.Alias;
            }
        }

        private void PreencherCfop()
        {
            if (_cfopDoPerfil != null)
            {
                CfopSelecionado = _cfopDoPerfil;
                return;
            }

            CfopSelecionado = _produto.RegraTributacaoSaida.CfopParaNfeFrom(DestinoOperacao);
        }

        public void PriorizarCfopDoPerfil(PerfilCfopDTO cfop)
        {
            _cfopDoPerfil = cfop;
            CfopSelecionado = cfop;
        }

        public void ThrowExceptionSeInvalido()
        {
            if (ProdutoSelecionado == null)
                throw new InvalidOperationException("Preciso de um Produto");

            if (CfopSelecionado == null)
                throw new InvalidOperationException("Preciso de um CFOP");

            if (Quantidade == 0 && FinalidadeEmissao != FinalidadeEmissao.Complementar)
                throw new InvalidOperationException("Quantidade 0,00 apenas para Nota Complementar");

            if (Total < 0)
                throw new InvalidOperationException("Total não pode ser negativo");
        }

        public void AplicarAlteracoesEm(ItemNfe item)
        {
            ProdutoAjuda.ValidarUnidadeMedidaPodeFracionar(_produto, Quantidade);
            item.Cfop = CfopSelecionado;
            item.CodigoBeneficioFiscal = CodBeneficioFiscal ?? string.Empty;
            item.NumeroPedido = NumeroPedido ?? string.Empty;
            item.NumeroItemPedido = NumeroItemPedido ?? 0;
            item.Observacao = Observacao ?? string.Empty;
            item.ComMercadoria(_produto, Quantidade, ValorUnitario, TotalDesconto, Total);
            item.ComCodigoUtilizado(ProdutoSelecionado.CodigoUtilizado);
            item.ComBarras(CodigoBarras);
        }

        public void SelecionarCfopPeloCodigo(string codigo)
        {
            if (codigo == CfopSelecionado?.Codigo)
            {
                return;
            }

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var rcfop = new RepositorioPerfilCfop(sessao);
                var cfop = rcfop.PeloCodigo(codigo);

                CfopSelecionado = cfop;
            }
        }

        public void AtualizarTabelaPreco(TabelaPreco tabelaPreco)
        {
            OnAtualizaTabelaPreco(tabelaPreco);
        }

        protected virtual void OnAtualizaTabelaPreco(ITabelaPreco e)
        {
            AtualizaTabelaPreco?.Invoke(this, e);
        }
    }
}