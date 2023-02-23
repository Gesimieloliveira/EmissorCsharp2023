using System;

namespace FusionPdv.Modelos.FormaPagamento
{
    public class Crediario : IFormaPagamento
    {
        public int Id => 4;
        public string Nome => "Crediário";
        public string CodigoEcf { get; set; }
        public bool Ecf => true;
        public DateTime? AlteradoEm { get; set; } = DateTime.Now;
    }
}