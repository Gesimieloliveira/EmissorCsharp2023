using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace FusionCore.Xml
{
    public class AssinadorSefaz
    {
        private readonly X509Certificate2 _certificado;

        public AssinadorSefaz(X509Certificate2 certificado)
        {
            _certificado = certificado;
        }

        public XmlDocument Assina(string xmlstring, string idref)
        {
            var documento = XmlFactory.Cria(xmlstring);
            var reference = new Reference { Uri = "#" + idref };

            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigC14NTransform());

            var assinatura = new SignedXml(documento) { SigningKey = _certificado.PrivateKey };

            assinatura.AddReference(reference);

            var keyInfo = new KeyInfo();

            keyInfo.AddClause(new KeyInfoX509Data(_certificado));
            assinatura.KeyInfo = keyInfo;
            assinatura.ComputeSignature();

            documento.FirstChild.AppendChild(assinatura.GetXml());

            return documento;
        }
    }
}