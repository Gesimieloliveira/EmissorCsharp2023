using System.Xml.Serialization;

namespace FusionCore.DFe.XmlCte
{
    public interface IAssinavel
    {
        [XmlElement(ElementName = "Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        FusionAssinaturaDigital AssinaturaDigital { get; set; }
    }
}