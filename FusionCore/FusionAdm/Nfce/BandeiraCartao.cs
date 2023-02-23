using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Nfce
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum BandeiraCartao
    {
        [Description("Visa")]
        Visa = 1,

        [Description("Mastercard")]
        Mastercard = 2,

        [Description("American Express")]
        AmericanExpress = 3,

        [Description("Sorocred")]
        Sorocred = 4,

        [Description("Diners Club")]
        DinersClub = 5,

        [Description("Elo")]
        Elo = 6,

        [Description("Hipercard")]
        Hipercard = 7,

        [Description("Aura")]
        Aura = 8,

        [Description("Cabal")]
        Cabal = 9,

        [Description("Outros")]
        Outros = 99
    }
}