using System;
using System.Xml.Serialization;

namespace FusionCore.DFe.XmlCte
{
    [Serializable]
    [XmlRoot(Namespace = "http://www.portalfiscal.inf.br/cte", ElementName = "CTe")]
    public class FusionCTe : IAssinavel
    {
        public FusionCTe()
        {
            InformacoesCTe = new FusionInformacoesCTe();
        }

        [XmlElement(ElementName = "infCte")]
        public FusionInformacoesCTe InformacoesCTe { get; set; }

        [XmlElement(ElementName = "infCTeSupl")]
        public InfCTeSupl InfCTeSupl { get; set; }

        [XmlElement(ElementName = "Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public FusionAssinaturaDigital AssinaturaDigital { get; set; }
    }
}