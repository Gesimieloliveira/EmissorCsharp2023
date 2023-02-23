using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.CteEletronicoOs.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoFretamento
    {
        [Description("1 - Eventual")]
        Eventual = 1,

        [Description("2 - Contínuo")]
        Continuo = 2,
    }
}