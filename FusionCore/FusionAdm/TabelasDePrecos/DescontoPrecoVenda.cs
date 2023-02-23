using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using System;

namespace FusionCore.FusionAdm.TabelasDePrecos
{
    public class DescontoPrecoVenda : ICalculoAjustePreco
    {
        public decimal Calcular(IProdutoTabelaPreco produto, decimal percentualAjuste)
        {
            return Calcular(produto.PrecoVenda, percentualAjuste);
        }

        public decimal Calcular(decimal preco, decimal percentualAjuste)
        {
            if (percentualAjuste == 0) return preco;

            return preco - preco * percentualAjuste / 100;
        }

        public decimal CalcularPercentualAjuste(decimal novoPrecoVenda, decimal precoVenda)
        {
            return decimal.Round((precoVenda - novoPrecoVenda) / precoVenda * 100, 2);
        }

        public void ThrowValidaCalcularPercentualAjuste(decimal novoPrecoVenda, decimal precoVenda)
        {
            if (novoPrecoVenda > precoVenda)
                throw new ArgumentException(
                    $"Novo Preço Venda: {novoPrecoVenda} deve ser menor que o Preço Venda: {precoVenda}");
        }
    }
}