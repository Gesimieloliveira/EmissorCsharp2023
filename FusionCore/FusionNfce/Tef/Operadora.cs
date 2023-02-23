using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionNfce.Tef
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum Operadora
    {
        [Description("Pay&GO")] PayGo = 1,
        [Description("Tef Express")] TefExpress = 2,
        [Description("Cappta")] Cappta = 3,
        [Description("Tef Dial Homologacao")] TefDialHomologacao = 99 
    }
}