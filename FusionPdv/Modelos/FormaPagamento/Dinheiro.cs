using System;

namespace FusionPdv.Modelos.FormaPagamento
{
    public class Dinheiro : IFormaPagamento
    {
        public int Id => 1;

        public string Nome => "Dinheiro";

        public string CodigoEcf { get; set; } = "";

        public bool Ecf => true;

        public DateTime? AlteradoEm { get; set; } = DateTime.Now;
    }
}
