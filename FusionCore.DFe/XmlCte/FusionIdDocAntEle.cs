using System;
using System.Xml.Serialization;

namespace FusionCore.DFe.XmlCte
{
    [Serializable]
    public class FusionIdDocAntEle
    {
        [XmlElement(ElementName = "chCTe")]
        public string Chave { get; set; }
    }
}