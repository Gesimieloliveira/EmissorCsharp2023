using System;
using FusionCore.Repositorio.Base;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.Vendas.Faturamentos
{
    public class FParcela : Entidade
    {
        private FParcela()
        {
            //nhibernate
        }

        public FParcela(short numero, DateTime vencimento, decimal valor)
        {
            Numero = numero;
            Vencimento = vencimento;
            Valor = valor;
        }

        public int Id { get; private set; }
        protected override int ReferenciaUnica => Id;
        public FPagamento Pagamento { get; private set; }
        public short Numero { get; private set; }
        public DateTime Vencimento { get; private set; }
        public decimal Valor { get; private set; }

        public void AnexarPagamento(FPagamento especie)
        {
            if (Pagamento != null)
            {
                throw new InvalidOperationException("Parcela já anexada a uma espécie");
            }

            Pagamento = especie;
        }
    }
}