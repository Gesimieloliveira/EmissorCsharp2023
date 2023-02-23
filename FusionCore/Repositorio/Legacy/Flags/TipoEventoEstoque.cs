using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.Repositorio.Legacy.Flags
{
    [TypeConverter(typeof (EnumTypeDescriptionConverter))]
    public enum TipoEventoEstoque
    {
        [Description("Entrada")] Entrada = 1,
        [Description("Saida")] Saida = 2
    }
}