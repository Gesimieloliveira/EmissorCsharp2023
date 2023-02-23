using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.ControleCaixa
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum EOrigemFluxoContaCaixa
    {
        [Description("Não definido")]
        NaoDefinido = 0,

        [Description("Abertura de caixa individual")]
        AberturaDeCaixaIndividual = 1,

        [Description("Fechamento de caixa individual")]
        FechamentoDeCaixaIndividual = 2,

        [Description("Lançamento Avulso")]
        LancamentoAvulso = 3
    }
}