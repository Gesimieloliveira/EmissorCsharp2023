using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.Seguranca.Licenciamento.Dominio
{
    [DataContract]
    [TypeConverter(typeof(EnumDescriptionConverter))]
    public enum TipoLicenca
    {
        [EnumMember]
        [Description("Fusion Starter")]
        FusionStarter = 3,

        [EnumMember]
        [Description("Fusion Gestor")]
        FusionGestor = 4,

        [EnumMember]
        [Description("Fusion CT-e")]
        FusionCTe = 5,

        [EnumMember]
        [Description("Fusion Acesso Adicional")]
        FusionAdicional = 6,

        [EnumMember]
        [Description("Fusion Chave Revalidação")]
        ChaveRevalidacao = 7,

        [EnumMember]
        [Description("Fusion Autorizacao de Revalidacao")]
        AutorizacaoRevalidacao = 8,

        [EnumMember]
        [Description("Fusion MDF-e")]
        FusionMDFe = 9,

        [EnumMember]
        [Description("Fusion CT-e OS")]
        FusionCTeOS = 10
    }

    public static class TipoLicencaEx
    {
        public static bool IsConcedeAcesso(this TipoLicenca tipo)
        {
            return tipo == TipoLicenca.FusionStarter ||
                   tipo == TipoLicenca.FusionCTe ||
                   tipo == TipoLicenca.FusionMDFe ||
                   tipo == TipoLicenca.FusionCTeOS;
        }

        public static bool IsUnica(this TipoLicenca tipo)
        {
            return tipo == TipoLicenca.FusionStarter ||
                   tipo == TipoLicenca.FusionGestor ||
                   tipo == TipoLicenca.FusionCTe ||
                   tipo == TipoLicenca.FusionMDFe ||
                   tipo == TipoLicenca.FusionCTeOS;
        }

        public static string GetStringValue(this TipoLicenca modulo)
        {
            switch (modulo)
            {
                case TipoLicenca.FusionStarter:
                    return "FS";
                case TipoLicenca.FusionGestor:
                    return "FG";
                case TipoLicenca.FusionCTe:
                    return "FC";
                case TipoLicenca.FusionAdicional:
                    return "AA";
                case TipoLicenca.ChaveRevalidacao:
                    return "CR";
                case TipoLicenca.AutorizacaoRevalidacao:
                    return "AR";
                case TipoLicenca.FusionMDFe:
                    return "FM";
                case TipoLicenca.FusionCTeOS:
                    return "CO";
            }

            throw new InvalidOperationException("Tipo de licença inválido para versão atual do Fusion");
        }

        public static short GetBitLogico(this TipoLicenca modulo)
        {
            switch (modulo)
            {
                case TipoLicenca.FusionStarter:
                    return 1;
                case TipoLicenca.FusionGestor:
                    return 2;
                case TipoLicenca.FusionCTe:
                    return 4;
                case TipoLicenca.FusionMDFe:
                    return 8;
                case TipoLicenca.FusionCTeOS:
                    return 16;
            }

            return 0;
        }
    }
}
