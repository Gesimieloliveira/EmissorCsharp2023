using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.CteEletronico.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoProprietario
    {
        [Description("TAC - Agregado")]
        TacAgregado = 0,
        [Description("TAC - Independente")]
        TacIndependente = 1,
        [Description("Outros")]
        Outros = 2
    }
}