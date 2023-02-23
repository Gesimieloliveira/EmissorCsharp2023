using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace FusionCore.Extencoes
{
    public static class ExtCertificadoDigital
    {
        public static bool IsA3(this X509Certificate2 x509cert)
        {
            if (x509cert == null)
                return false;

            bool result = false;

            try
            {
                RSACryptoServiceProvider service = x509cert.PrivateKey as RSACryptoServiceProvider;

                if (service != null)
                {
                    if (service.CspKeyContainerInfo.Removable &&
                    service.CspKeyContainerInfo.HardwareDevice)
                        result = true;
                }
            }
            catch
            {
                //assume que é false
                result = false;
            }

            return result;
        }
    }
}