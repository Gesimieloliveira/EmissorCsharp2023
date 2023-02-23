using System;

namespace FusionCore.FusionNfce.VendasPendentesMensais
{
    public class VendaPendenteMensal
    {
        public int Id { get; set; }
        public bool IsResolvido { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.Now;
        public int Ano { get; set; }
        public int Mes { get; set; }

        public bool IsNaoResolvido => NaoEstaResolvido();

        private bool NaoEstaResolvido()
        {
            return !IsResolvido;
        }
    }
}