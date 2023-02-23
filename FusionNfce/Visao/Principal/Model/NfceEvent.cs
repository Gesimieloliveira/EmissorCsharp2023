using System;
using FusionCore.FusionNfce.Fiscal;

namespace FusionNfce.Visao.Principal.Model
{
    public class NfceEvent : EventArgs
    {
        public NfceEvent(Nfce nfce)
        {
            Nfce = nfce;
        }

        public Nfce Nfce { get; set; }
    }
}