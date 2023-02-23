using System.Security.Cryptography.X509Certificates;
using FusionCore.Xml;

namespace FusionCore.FusionAdm.CteEletronico.Assinatura
{
    public static class AssinaturaDigital
    {
        public static string AssinaDocumento(string xmlString, string id, X509Certificate2 certificado)
        {
            var assinador = new AssinadorSefaz(certificado);
            var docAssinado = assinador.Assina(xmlString, id);

            return docAssinado.OuterXml;
        }
    }
}