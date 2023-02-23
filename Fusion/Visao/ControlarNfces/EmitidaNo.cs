using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace Fusion.Visao.ControlarNfces
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum EmitidaNo
    {
        [Description("Todos Locais")]
        TodosLocais,

        [Description("Terminal PDV Offline")]
        TerminalOffline,

        [Description("Faturamento")]
        Faturamento
    }
}