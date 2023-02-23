namespace FusionCore.FusionAdm.TabelasDePrecos
{
    public interface ICalculoAjustePreco
    {
        decimal Calcular(IProdutoTabelaPreco produto, decimal percentualAjuste);
        decimal Calcular(decimal preco, decimal percentualAjuste);
        decimal CalcularPercentualAjuste(decimal novoPrecoVenda, decimal precoVenda);
        void ThrowValidaCalcularPercentualAjuste(decimal novoPrecoVenda, decimal precoVenda);
    }
}