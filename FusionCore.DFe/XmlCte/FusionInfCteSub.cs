using System;
using System.Xml.Serialization;

namespace FusionCore.DFe.XmlCte
{
    [Serializable]
    public class FusionInfCteSub
    {
        [XmlElement(ElementName = "chCte")]
        public string ChaveCTeOriginal { get; set; }

        [XmlElement(ElementName = "refCteAnu")]
        public string ChaveCTeDeAnulacao { get; set; }

        [XmlElement(elementName: "tomaICMS")]
        public FusionTomaICMS FusionTomaIcms { get; set; }

        [XmlElement("indAlteraToma")]
        public FusionIndicadorAlteracaoTomador? FusionIndicadorAlteracaoTomador { get; set; }

        public bool FusionIndicadorAlteracaoTomadorSpecified => FusionIndicadorAlteracaoTomador.HasValue;
    }
}