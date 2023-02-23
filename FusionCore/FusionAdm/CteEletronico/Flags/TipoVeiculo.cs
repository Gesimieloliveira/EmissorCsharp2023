using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.CteEletronico.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoVeiculo
    {
        [Description("Tração")]
        Tracao,
        [Description("Reboque")]
        Reboque
    }
}