using System;

namespace FusionCore.FusionNfce.Fiscal
{
    public class NfceContingencia
    {
        public int Id { get; set; }
        public string Motivo { get; set; }
        public DateTime EntrouEm { get; set; }
        public bool Ativa { get; set; }
    }
}