using System;

namespace FusionWPF.Parcelamento
{
    public class ParcelaGerada
    {
        public ParcelaGerada(byte numero, DateTime vencimento, decimal valor)
        {
            Numero = numero;
            Vencimento = vencimento;
            Valor = valor;
        }

        public byte Numero { get; }
        public DateTime Vencimento { get; }
        public decimal Valor { get; }
    }
}