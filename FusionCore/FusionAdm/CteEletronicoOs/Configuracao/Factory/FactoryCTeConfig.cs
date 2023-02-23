using System;
using DFe.CertificadosDigitais.Implementacao;
using DFe.Utils.Flags;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionLibrary.Helper.Criptografia;
using TipoEmissao = FusionCore.FusionAdm.CteEletronico.Flags.TipoEmissao;

namespace FusionCore.FusionAdm.CteEletronicoOs.Configuracao.Factory
{
    public static class FactoryCTeConfig
    {
        public static CTeOsConfig CriarCteConfig(EmissorFiscal emissorFiscal, TipoEmissao tipoEmissao)
        {
            var cteConfig = new CTeOsConfig();
            cteConfig.Inicializar(emissorFiscal, tipoEmissao);

            return cteConfig;
        }

        public static DFeCertificadoDigital CriarCertificadoDigital(EmissorFiscal emissorFiscal)
        {
            var configCert = new DFeConfigCertificadoDigital
            {
                Serial = SimetricaCrip.Descomputar(emissorFiscal.SerialNumberCertificado),
                LocalArquivo = emissorFiscal.ArquivoCertificado,
                Senha = SimetricaCrip.Descomputar(emissorFiscal.SenhaCertificado)
            };

            switch (emissorFiscal.TipoCertificadoDigital)
            {
                case TipoCertificadoDigital.A1Arquivo:
                    configCert.TipoCertificado = TipoCertificado.A1Arquivo;
                    break;
                case TipoCertificadoDigital.A1Repositorio:
                    configCert.TipoCertificado = TipoCertificado.A1Repositorio;
                    break;
                case TipoCertificadoDigital.A3:
                    configCert.TipoCertificado = TipoCertificado.A3;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            return new DFeCertificadoDigital(configCert);
        }
    }
}