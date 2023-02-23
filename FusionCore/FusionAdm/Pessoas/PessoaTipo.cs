using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.Pessoas
{
    [TypeConverter(typeof (EnumTypeDescriptionConverter))]
    public enum PessoaTipo
    {
        [Description("Física")] Fisica,
        [Description("Jurídica")] Juridica,
        [Description("Extrangeiro")] Extrangeiro
    }

    public static class PessoaTipoExt
    {
        public static string GetCodigo(this PessoaTipo pessoaTipo)
        {
            switch (pessoaTipo)
            {
                case PessoaTipo.Extrangeiro:
                    return "E";
                case PessoaTipo.Fisica:
                    return "F";
                case PessoaTipo.Juridica:
                    return "J";
                default:
                    return string.Empty;
            }
        }
    }
}