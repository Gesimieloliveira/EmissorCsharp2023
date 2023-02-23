using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionNfce.Pagamento
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoCartaoPos
    {
        [Description("Crédito")]
        Credito,

        [Description("Débito")]
        Debito
    }
}