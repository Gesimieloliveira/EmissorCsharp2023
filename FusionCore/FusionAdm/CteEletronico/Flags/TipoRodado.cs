using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.CteEletronico.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoRodado
    {
        [Description("Não Aplicável")]
        NaoAplicavel,
        [Description("Truck")]
        Truck,
        [Description("Toco")]
        Toco,
        [Description("Cavalo Mecânico")]
        CavaloMecanico,
        [Description("Van")]
        Van,
        [Description("Utilitário")]
        Utilitario,
        [Description("Outros")]
        Outros
    }
}