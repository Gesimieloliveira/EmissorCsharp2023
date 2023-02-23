using System;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionNfce.Fiscal
{
    public class NfceInutilizacao
    {
        public NfceInutilizacao()
        {
            InutilizacaoEm = DateTime.Now;
        }

        public int Id { get; set; }
        public int CodigoUfSolicitante { get; set; }
        public byte Ano { get; set; }
        public string CnpjEmitente { get; set; }
        public ModeloDocumento ModeloDocumento { get; set; }
        public short Serie { get; set; }
        public int NumeroInicial { get; set; }
        public int NumeroFinal { get; set; }
        public string Justificativa { get; set; }
        public string Protocolo { get; set; }
        public DateTime InutilizacaoEm { get; set; }
        public string Uuid { get; set; }
        public bool Sincronizado { get; set; }
    }
}