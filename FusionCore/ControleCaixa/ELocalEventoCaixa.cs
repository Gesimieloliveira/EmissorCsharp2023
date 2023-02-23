using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.ControleCaixa
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum ELocalEventoCaixa
    {
        [Description("Gestão")]
        Gestao = 0,

        [Description("Terminal NFC-e")]
        Terminal = 1
    }
}