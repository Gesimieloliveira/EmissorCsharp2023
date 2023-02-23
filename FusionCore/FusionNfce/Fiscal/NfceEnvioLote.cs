using System;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionNfce.Fiscal
{
    public class NfceEnvioLote
    {
        public int Id { get; set; }
        public TipoAmbiente Ambiente { get; set; }
        public string VersaoAplicacao { get; set; }
        public short CodigoStatus { get; set; }
        public string Motivo { get; set; }
        public byte CodigoUf { get; set; }
        public DateTime? DataEHoraDoProcessamento { get; set; }
        public string NumeroRecibo { get; set; }
        public int TempoMedio { get; set; }
        public bool ComErro { get; set; }
    }
}