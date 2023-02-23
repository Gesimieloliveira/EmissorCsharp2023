using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.GerenciarManifestacoesEletronicas
{
    [TypeConverter(typeof (EnumTypeDescriptionConverter))]
    public enum TipoDfe
    {
        [Description("Resumo NF-e")]
        ResumoNfe = 1,

        [Description("Evento NF-e")]
        ProcEventoNfe = 2,

        [Description("Resumo Evento")]
        ResEvento = 3,

        [Description("NF-e Processada")]
        NfeProc = 4,

        [Description("Outros")]
        Outros = 99
    }
}