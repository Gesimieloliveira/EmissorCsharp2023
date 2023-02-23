using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.CteEletronico.Emissao;

namespace FusionCore.FusionAdm.CteEletronico.CCe
{
    public class CteCartaCorrecao
    {
        public int Id { get; set; }
        public Cte Cte { get; set; }
        public DateTime OcorreuEm { get; set; }
        public int StatusRetorno { get; set; }
        public string Protocolo { get; set; }
        public byte SequenciaEvento { get; set; }
        public string XmlEnvio { get; set; }
        public string XmlRetorno { get; set; }

        public IList<CteInformacaoCorrecao> CteInformacaoCorrecaos { get; set; }
        public string ChaveId { get; set; }
    }
}