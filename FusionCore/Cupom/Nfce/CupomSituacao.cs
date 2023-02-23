using System.ComponentModel;

namespace FusionCore.Cupom.Nfce
{
    public enum CupomSituacao
    {
        [Description("Autorizado")]
        Autorizado,

        [Description("Autorizado: Contingência")]
        AutorizadoOffline,

        [Description("Cancelado")]
        Cancelado,

        [Description("Denegada")]
        Denegada,

        [Description("Aberta")]
        Aberta,

        [Description("Rejeição: Contingência")]
        RejeicaoOffline,

        [Description("Rejeição")]
        Rejeicao
    }
}