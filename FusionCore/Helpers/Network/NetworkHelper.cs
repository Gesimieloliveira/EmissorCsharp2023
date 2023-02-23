using System.Net;

namespace FusionCore.Helpers.Network
{
    public static class NetworkHelper
    {
        public static void DefineCertificateValidation()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }
    }
}