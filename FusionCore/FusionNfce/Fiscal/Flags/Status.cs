using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionNfce.Fiscal.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum Status
    {
        [Description("Aberta")]
        Aberta = 0,
        [Description("Cancelada")]
        Cancelada = 1,
        [Description("Transmitida")]
        Transmitida = 2,
        [Description("Pendente")]
        PendenteOffline = 3
    }
}