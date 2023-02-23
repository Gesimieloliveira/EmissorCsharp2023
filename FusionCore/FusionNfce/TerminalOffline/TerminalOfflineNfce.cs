using System.Collections.Generic;
using FusionCore.FusionNfce.EmissorFiscal;

namespace FusionCore.FusionNfce.TerminalOffline
{
    public class TerminalOfflineNfce
    {
        public byte Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public int IntervaloSync { get; set; }
        public string BindTerminal { get; set; }
        public IList<NfceEmissorFiscal> ListaEmissorNfce { get; set; } = new List<NfceEmissorFiscal>();
    }
}
