using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.Vendas.Faturamentos
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum SituacaoFiscal
    {
        [Description("Não enviado")]
        NaoEnviado = 0,

        [Description("Cancelado")]
        Cancelado = 1,

        [Description("Autorizado")]
        Autorizado = 2,

        [Description("Rejeição: Contingência")]
        ComRejeicaoOffline = 3,

        [Description("Autorizado: Contingência")]
        AutorizadoSemInternet = 4,

        [Description("Autorizado: Denegada")]
        AutorizadoDenegada = 5,

        [Description("Rejeição")]
        ComRejeicao = 6
    }
}