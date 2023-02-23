using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionNfce.Visao.Principal.CodigoAtivacaoSAT
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum OpcaoCodigoAtivacao
    {
        [Description("Código de Ativação")]
        CodigoAtivacao = 1,
        [Description("Código de Ativação de Emergência")]
        CodigoAtivacaoEmergencia = 2
    }
}