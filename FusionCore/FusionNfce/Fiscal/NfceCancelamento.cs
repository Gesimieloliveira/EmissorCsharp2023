using System;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionNfce.Fiscal
{
    public class NfceCancelamento
    {
        public int NfceId { get; set; }
        public Nfce Nfce { get; set; }
        public string DocumentoUnico { get; set; }
        public string VersaoAplicacao { get; set; }
        public TipoAmbiente Ambiente { get; set; }
        public byte CodigoUf { get; set; }
        public int StatusRetorno { get; set; }
        public string Chave { get; set; }
        public int TipoEvento { get; set; }
        public DateTime? OcorreuEm { get; set; }
        public string Protocolo { get; set; }
        public string Justificativa { get; set; }
    }
}