using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using DFe.Utils;
using DFe.Utils.Assinatura;
using FusionCore.Core.Net;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionNfce.Extencoes;
using FusionLibrary.Helper.Criptografia;

namespace FusionCore.FusionAdm.Fiscal.Fabricas
{
    public static class CertificadoDigitalFactory
    {
        public static X509Certificate2 Cria(EmissorFiscal emissor, bool manterCache = false)
        {
            try
            {
                var configuracao = new ConfiguracaoCertificado
                {
                    TipoCertificado = emissor.TipoCertificadoDigital.ToZeus(),
                    CacheId = emissor.Id.ToString(),
                    Arquivo = emissor.ArquivoCertificado,
                    ManterDadosEmCache = manterCache,
                    Senha = SimetricaCrip.Descomputar(emissor.SenhaCertificado),
                    Serial = SimetricaCrip.Descomputar(emissor.SerialNumberCertificado)
                };
                
                var certificado = CertificadoDigital.ObterCertificado(configuracao);

                if (certificado.NotAfter <= DateTime.Now)
                {
                   throw new InvalidOperationException("Certificado digital vencido");
                }

                ServicePointManager.SecurityProtocol = emissor.ProtocoloSeguranca.ToSecurityProtocol();

                return certificado;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Não foi possível carregar o certificado: {ex.Message}");
            }
        }
    }
}