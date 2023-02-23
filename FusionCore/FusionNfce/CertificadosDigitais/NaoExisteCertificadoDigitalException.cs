using System;

namespace FusionCore.FusionNfce.CertificadosDigitais
{
    public class NaoExisteCertificadoDigitalException : InvalidOperationException
    {
        public NaoExisteCertificadoDigitalException(string message) : base(message)
        {
        }
    }
}