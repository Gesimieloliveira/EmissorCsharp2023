using System;

namespace FusionPdv.Modelos.FormaPagamento
{
    public class AjusteSaldo : IFormaPagamento
    {
        public int Id => 10;

        public string Nome => "Ajuste";

        public string CodigoEcf
        {
            get { throw new ArgumentException("Não pode ser utilizado"); }
            set { throw new ArgumentException("Não pode ser utilizado"); }
        }

        public bool Ecf => false;

        public DateTime? AlteradoEm { get; set; } = DateTime.Now;
    }
}
