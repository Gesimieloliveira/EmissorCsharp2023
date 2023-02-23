namespace FusionCore.FusionAdm.TabelasDePrecos.Dtos
{
    public class AjusteDiferenciadoDto
    {
        public decimal PercentualAjuste { get; set; }
        public TipoAjustePreco TipoAjustePreco { get; set; }

        public decimal Calcula(IProdutoTabelaPreco produto)
        {
            if (PercentualAjuste == 0)
                return produto.PrecoVenda;

            var novoPreco = FabricaCalculoPeloTipoAjuste.ObterCalculadoraDeAjuste(TipoAjustePreco).Calcular(produto, PercentualAjuste);

            return novoPreco;
        }
    }
}