using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.CteEletronico.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoEmissao
    {
        [Description("Normal")]
        Normal = 1,

        [Description("Contingência")]
        Contingencia = 2
    }
}