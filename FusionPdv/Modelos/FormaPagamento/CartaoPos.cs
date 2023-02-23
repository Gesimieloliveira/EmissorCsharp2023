using System;

namespace FusionPdv.Modelos.FormaPagamento
{
    public class CartaoPos : IFormaPagamento
    {
        public int Id => 3;
        public string Nome => "Cartão POS";
        public string CodigoEcf { get; set; }
        public bool Ecf => true;
        public DateTime? AlteradoEm { get; set; } = DateTime.Now;
    }
}