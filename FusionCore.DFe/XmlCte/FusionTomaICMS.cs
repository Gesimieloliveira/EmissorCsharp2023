using System;
using System.Xml.Serialization;

namespace FusionCore.DFe.XmlCte
{
    [Serializable]
    public class FusionTomaICMS
    {
        [XmlElement("refNFe")]
        public string RefNFe { get; set; }

        [XmlElement("refNF")]
        public FusionRefNF FusionRefNF { get; set; }

        [XmlElement("refCte")]
        public string RefCte { get; set; }
    }
}