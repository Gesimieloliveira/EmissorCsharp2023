using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.EmissorFiscal;

namespace FusionCore.FusionNfce.ConfiguracaoTerminal
{
    public class ConfiguracaoTerminalNfce
    {
        public ConfiguracaoTerminalNfce()
        {
            TipoEmissao = TipoEmissao.Normal;
            JustificativaUltimaContigencia = string.Empty;
        }

        public byte Id { get; set; }
        public NfceEmissorFiscal EmissorFiscal => ObterEmissorFiscalEmUso();

        public IList<NfceEmissorFiscal> EmissorFiscalLista { get; set; } = new List<NfceEmissorFiscal>();
        public int IntervaloSync { get; set; }
        public string BindTerminal { get; set; }
        public byte TerminalOfflineId { get; set; }

        public TipoEmissao TipoEmissao { get; set; }
        public DateTime? DataUltimaContingencia { get; set; }
        public string JustificativaUltimaContigencia { get; set; }
        public string ObservacaoPadrao { get; set; }


        private NfceEmissorFiscal ObterEmissorFiscalEmUso()
        {
            var emissorFiscal = EmissorFiscalLista.FirstOrDefault(x =>
                x.EmUso == true && x.TerminalOfflineId == TerminalOfflineId);

            if (emissorFiscal != null) return emissorFiscal;

            return EmissorFiscalLista.FirstOrDefault();
        }
    }
}