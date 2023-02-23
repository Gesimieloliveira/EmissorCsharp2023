using System;

namespace FusionCore.FusionAdm.MdfeEletronico.EventoPagamento.Entidades
{
    public class MdfeParcela
    {
        public MdfeParcela(int numero, DateTime dataDeVencimento, decimal valor)
        {
            Numero = numero;
            DataDeVencimento = dataDeVencimento;
            Valor = valor;
        }

        public MdfeParcela() : this(0, DateTime.Now, 0)
        {
        }


        public int Id { get; set; }
        public InformacaoPagamento InformacaoPagamento { get; set; }
        public int Numero { get; set; }
        public DateTime DataDeVencimento { get; set; }
        public decimal Valor { get; set; }
    }
}