using System;

namespace FusionPdv.Modelos.FormaPagamento
{
    public class CartaoTef : IFormaPagamento
    {
        public int Id => 2;

        public string Nome => "Cartão TEF";

        public string CodigoEcf { get; set; }

        public bool Ecf => true;

        public DateTime? AlteradoEm { get; set; } = DateTime.Now;
    }
}
