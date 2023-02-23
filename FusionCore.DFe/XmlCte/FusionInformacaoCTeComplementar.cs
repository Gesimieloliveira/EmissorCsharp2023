using System;
using System.Xml.Serialization;

namespace FusionCore.DFe.XmlCte
{
    [Serializable]
    public class FusionInformacaoCTeComplementar
    {
        [XmlElement("chCTe")]
        public string ChaveCteComplementado { get; set; }
    }
}