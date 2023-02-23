using FusionCore.FusionAdm.TabelasDePrecos;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Produto.Models
{
    public class ProdutoTabelaPrecoViewModel : ViewModel
    {
        public int TabelaPrecoId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public string TabelaPrecoDescricao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public TipoAjustePreco TipoAjuste
        {
            get => GetValue<TipoAjustePreco>();
            set => SetValue(value);
        }

        public decimal PercentualAjuste
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal PercentualDiferenciado
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal PrecoAjustado
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public int ProdutoId
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public void CalcularPreco(int produtoId, decimal precoVenda)
        {
            var calculadora = FabricaCalculoPeloTipoAjuste.ObterCalculadoraDeAjuste(TipoAjuste);
            var ajuste = PercentualAjuste;

            if (produtoId == ProdutoId)
            {
                ajuste = PercentualDiferenciado > 0 ? PercentualDiferenciado : PercentualAjuste;
            }

            var novoPreco = calculadora.Calcular(precoVenda, ajuste);

            PrecoAjustado = novoPreco;
            PercentualAjuste = ajuste;
        }
    }
}