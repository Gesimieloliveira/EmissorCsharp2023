using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.CteEletronico.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum Modal
    {
        [Description("Rodoviário")]
        Rodoviario = 1
    }
}