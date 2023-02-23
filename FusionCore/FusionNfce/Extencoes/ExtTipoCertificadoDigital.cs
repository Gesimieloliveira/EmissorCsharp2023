using System;
using DFe.Utils;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtTipoCertificadoDigital
    {
        public static TipoCertificado ToZeus(this TipoCertificadoDigital tipoCertificadoDigital)
        {
            switch (tipoCertificadoDigital)
            {
                case TipoCertificadoDigital.A1Arquivo:
                    return TipoCertificado.A1Arquivo;
                case TipoCertificadoDigital.A1Repositorio:
                    return TipoCertificado.A1Repositorio;
                case TipoCertificadoDigital.A3:
                    return TipoCertificado.A3;
                default: throw new InvalidOperationException("Tipo certificado inválido");
            }
        }
    }
}