using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.FusionAdm.CteEletronico.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoPeriodoHora
    {
        [Description("Sem hora definida")]
        SemHoraDefinida,
        [Description("No horário")]
        NoHorario,
        [Description("Até o horário")]
        AteOHorario,
        [Description("A partir do horário")]
        APartirDoHorario,
        [Description("No intervalo de tempo")]
        NoIntervaloDeTempo
    }
}