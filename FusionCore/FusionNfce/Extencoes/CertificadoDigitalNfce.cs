using System.Net;
using System.Security.Cryptography.X509Certificates;
using DFe.Utils.Assinatura;
using FusionCore.FusionNfce.Sessao.Sistema;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class CertificadoDigitalNfce
    {
        public static X509Certificate2 CriaNfceCertificate2(bool manterCache = false)
        {
            var dadosSefaz = SessaoSistemaNfce.GetDadosSefaz();
            dadosSefaz.Certificado.ManterDadosEmCache = manterCache;
            ServicePointManager.SecurityProtocol = dadosSefaz.GetProtocoloSeguranca();

            return CertificadoDigital.ObterCertificado(dadosSefaz.Certificado);
        }
    }
}