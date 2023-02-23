using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.Cartoes
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum CartaoBandeira
    {
        [Description("Nemnhum")]
        Nemnhum = 0,

        [Description("VISA")]
        Visa = 1,

        [Description("MASTERCARD")]
        Mastercard = 2,

        [Description("AMERICAN EXPRESS")]
        AmericanExpress = 3,

        [Description("SOROCRED")]
        Sorocred = 4,

        [Description("DINERS CLUB")]
        DinersClub = 5,

        [Description("ELO")]
        Elo = 6,

        [Description("HIPERCARD")]
        Hipercard = 7,

        [Description("AURA")]
        Aura = 8,

        [Description("CABAL")]
        Cabal = 9,

        [Description("Alelo")]
        Alelo = 10,

        [Description("Banes Card")]
        BanesCard = 11,

        [Description("CalCard")]
        CalCard = 12,

        [Description("Credz")]
        Credz = 13,

        [Description("Discover")]
        Discover = 14,

        [Description("GoodCard")]
        GoodCard = 15,

        [Description("GreenCard")]
        GreenCard = 16,

        [Description("Hiper")]
        Hiper = 17,

        [Description("JcB")]
        JcB = 18,

        [Description("Mais")]
        Mais = 19,

        [Description("MaxVan")]
        MaxVan = 20,

        [Description("Policard")]
        Policard = 21,

        [Description("RedeCompras")]
        RedeCompras = 22,

        [Description("Sodexo")]
        Sodexo = 23,

        [Description("ValeCard")]
        ValeCard = 24,

        [Description("Verocheque")]
        Verocheque = 25,

        [Description("VR")]
        VR = 26,

        [Description("Ticket")]
        Ticket = 27,

        [Description("OUTROS")]
        Outros = 99
    }
}