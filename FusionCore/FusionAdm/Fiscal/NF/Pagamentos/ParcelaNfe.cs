using System;

namespace FusionCore.FusionAdm.Fiscal.NF.Pagamentos
{
    public class ParcelaNfe
    {
        private ParcelaNfe()
        {
            //nhibernate
        }

        public ParcelaNfe(byte numero, DateTime vencimento, decimal valor) : this()
        {
            Numero = numero;
            Vencimento = vencimento;
            Valor = valor;
        }

        public int Id { get; set; }
        public Aprazo Prazo { get; set; }
        public byte Numero { get; set; }
        public DateTime Vencimento { get; set; }
        public decimal Valor { get; set; }
    }
}