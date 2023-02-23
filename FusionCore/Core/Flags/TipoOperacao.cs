using System;
using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.Core.Flags
{
    [Serializable]
    [TypeConverter(typeof (EnumTypeDescriptionConverter))]
    public enum TipoOperacao
    {
        [Description("Entrada")] Entrada = 0,
        [Description("Saída")] Saida = 1
    }
}
