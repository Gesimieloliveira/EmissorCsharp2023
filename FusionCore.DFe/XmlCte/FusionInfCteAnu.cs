using System;
using System.Xml.Serialization;

namespace FusionCore.DFe.XmlCte
{
    [Serializable]
    public class FusionInfCteAnu
    {
        [XmlElement(ElementName = "chCte")]
        public string Chave { get; set; }

        [XmlElement(ElementName = "dEmi")]
        public string DeclaracaoEmitidaEm { get; set; }
    }
}