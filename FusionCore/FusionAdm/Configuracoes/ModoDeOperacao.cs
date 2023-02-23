using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Configuracoes
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum ModoDeOperacao
    {
        [Description("Preço")] Preco,
        [Description("Peso")] Peso
    }
}