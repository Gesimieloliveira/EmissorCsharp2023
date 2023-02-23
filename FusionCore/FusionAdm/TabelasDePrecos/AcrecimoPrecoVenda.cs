using System;

namespace FusionCore.FusionAdm.TabelasDePrecos
{
    public class AcrecimoPrecoVenda : ICalculoAjustePreco
    {
        public decimal Calcular(IProdutoTabelaPreco produto, decimal percentualAjuste)
        {
            return Calcular(produto.PrecoVenda, percentualAjuste);
        }

        public decimal Calcular(decimal preco, decimal percentualAjuste)
        {
            if (percentualAjuste == 0) return preco;

            return preco * percentualAjuste / 100 + preco;
        }

        public decimal CalcularPercentualAjuste(decimal novoPrecoVenda, decimal precoVenda)
        {
            return decimal.Round((novoPrecoVenda - precoVenda) / precoVenda * 100, 2);
        }

        public void ThrowValidaCalcularPercentualAjuste(decimal novoPrecoVenda, decimal precoVenda)
        {
            if (novoPrecoVenda < precoVenda)
                throw new ArgumentException(
                    $"Novo Preço Venda: {novoPrecoVenda} deve ser maior que o Preço Venda: {precoVenda}");
        }
    }
}