using System.Security.Cryptography.X509Certificates;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Fiscal.TyneTypes;
using FusionCore.FusionNfce.Helper;

namespace FusionCore.FusionNfce.Autorizacao
{
    public class AssinadorNFCe
    {
        private readonly X509Certificate2 _x509Certificate2;
        private readonly NfceEmissaoHistorico _emissaoHistorico;
        private readonly Nfce _nfce;

        public AssinadorNFCe(X509Certificate2 x509Certificate2, NfceEmissaoHistorico emissaoHistorico, Nfce nfce)
        {
            _x509Certificate2 = x509Certificate2;
            _emissaoHistorico = emissaoHistorico;
            _nfce = nfce;
        }

        public NfceEmissaoHistorico Assinar()
        {
            var assinadorHelper = new AssinaNfceHelper(_x509Certificate2, _emissaoHistorico, _nfce);
            var xmlAssinado = assinadorHelper.GeraXmlAssinado();

            var emissaoHistorico = _emissaoHistorico.ToBuilder().ComXmlDeEnvio(new XmlEnvio(xmlAssinado));

            return emissaoHistorico;
        }
    }
}