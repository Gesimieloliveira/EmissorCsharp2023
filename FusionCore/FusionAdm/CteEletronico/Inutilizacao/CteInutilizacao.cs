using System;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.CteEletronico.Inutilizacao
{
    public class CteInutilizacao
    {
        public CteInutilizacao()
        {
            InutilizacaoEm = DateTime.Now;
        }

        public int Id { get; set; }
        public byte CodigoUfSolicitante { get; set; }
        public byte Ano { get; set; }
        public string CnpjEmitente { get; set; }
        public ModeloDocumento ModeloDocumento { get; set; }
        public short Serie { get; set; }
        public int NumeroInicial { get; set; }
        public int NumeroFinal { get; set; }
        public string Justificativa { get; set; }
        public string Protocolo { get; set; }
        public string XmlEnvio { get; set; }
        public string XmlRetorno { get; set; }
        public DateTime InutilizacaoEm { get; set; }
    }
}