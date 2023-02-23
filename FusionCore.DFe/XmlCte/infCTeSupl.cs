using System;
using System.Xml.Serialization;

namespace FusionCore.DFe.XmlCte
{
    [Serializable]
    public class InfCTeSupl
    {
        [XmlElement(ElementName = "qrCodCTe")]
        public string QrCodCTe { get; set; }
    }
}