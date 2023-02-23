using FusionCore.FusionNfce.Produto;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.TabelasDePrecos.NfceSync
{
    public class AjusteDiferenciadoNfce : EntidadeBase<int>
    {
        public int Id { get; set; }
        public TabelaPrecoNfce TabelaPreco { get; set; }
        public ProdutoNfce Produto { get; set; }
        public decimal PercentualAjuste { get; set; }

        public decimal NovoPreco => Calcula();

        private decimal Calcula()
        {
            if (PercentualAjuste == 0)
                return Produto.PrecoVenda;

            var novoPreco = FabricaCalculoPeloTipoAjuste.ObterCalculadoraDeAjuste(TabelaPreco.TipoAjustePreco).Calcular(Produto, PercentualAjuste);

            return novoPreco;
        }

        protected override int ChaveUnica => Id;
    }
}