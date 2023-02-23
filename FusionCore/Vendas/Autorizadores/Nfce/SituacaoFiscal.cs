using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.Vendas.Autorizadores.Nfce
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum SituacaoFiscal
    {
        [Description("Aberta")]
        Aberta = 0,

        [Description("Cancelado")]
        Cancelado = 1,

        [Description("Autorizada")]
        Autorizada = 2,

        [Description("Rejeição: Contingência")]
        RejeicaoOffline = 3,

        [Description("Autorizada: Contingência")]
        AutorizadaSemInternet = 4,

        [Description("Autorizada: Denegada")]
        AutorizadaDenegada = 5,

        [Description("Rejeição")]
        Rejeicao = 6
    }
}