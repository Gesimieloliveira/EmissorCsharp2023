using System;
using System.Xml.Serialization;

namespace FusionCore.DFe.XmlCte
{
    [Serializable]
    public class FusionInformacaoModalCTe
    {
        [XmlAttribute(AttributeName = "versaoModal")]
        public string Versao { get; set; } = "3.00";

        [XmlElement(ElementName = "rodo")]
        public FusionRodoviarioCTe Rodoviario { get; set; }

        public FusionInformacaoModalCTe()
        {
            Rodoviario = new FusionRodoviarioCTe();
        }
    }

    [Serializable]
    public class FusionRodoviarioCTe
    {
        [XmlElement(ElementName = "RNTRC")]
        public string Rntrc { get; set; }
    }
}