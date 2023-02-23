using System;
using System.Collections.Generic;

namespace FusionCore.FusionAdm.CteEletronicoOs.Emissao
{
    public class CteOsCartaCorrecao
    {
        public int Id { get; set; }
        public CteOs CteOs { get; set; }
        public DateTime OcorreuEm { get; set; }
        public int StatusRetorno { get; set; }
        public string Protocolo { get; set; }
        public byte SequenciaEvento { get; set; }
        public string XmlEnvio { get; set; }
        public string XmlRetorno { get; set; }

        public IList<CteOsInformacaoCorrecao> CteOsInformacaoCorrecaos { get; set; }
        public string ChaveId { get; set; }
    }
}