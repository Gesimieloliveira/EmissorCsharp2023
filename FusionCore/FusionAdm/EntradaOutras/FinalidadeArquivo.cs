using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.EntradaOutras
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum FinalidadeArquivo
    {
        [Description("Normal")]
        Normal = 1,

        [Description("Retificação total do arquivo")]
        RetificacaoTotalArquivo = 2,

        [Description("Retificação aditiva do arquivo")]
        RetificacaoAditivaArquivo = 3,

        [Description("Desfazimento")]
        Desfazimento = 5
    }
}