using System.Security.Cryptography.X509Certificates;
using CertificadoDigitalZeus = DFe.Utils.CertificadoDigitalUtils;


namespace FusionCore.Helpers.CertificadoDigital
{
    public static class ManipulaCertificadoDigital
    {
        public static X509Certificate2 ObterDoRepositorio()
        {
            return CertificadoDigitalZeus.ListareObterDoRepositorio();
        }
    }
}