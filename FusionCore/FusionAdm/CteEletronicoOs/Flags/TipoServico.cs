using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.CteEletronicoOs.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoServico
    { 
        [Description("Excesso de Bagagem")]
        ExcessoBagagem = 7,

        [Description("Transporte de Pessoas")]
        TransportePessoas = 5,

        [Description("Transporte de Valores")]
        TransporteValores
    }
}