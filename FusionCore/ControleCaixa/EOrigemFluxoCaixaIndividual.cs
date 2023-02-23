using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.ControleCaixa
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum EOrigemFluxoCaixaIndividual
    {
        [Description("Faturamento")]
        Faturamento = 0,

        [Description("NF-e")]
        Nfe = 1,

        [Description("NFC-e")]
        Nfce = 2,

        [Description("Documento a Receber")]
        DocumentoReceber = 3,

        [Description("Documento a Pagar")]
        DocumentoPagar = 4,

        [Description("Lançamento avulso")]
        LancamentoAvulso = 5,

        [Description("Abertura de caixa")]
        AberturaCaixa = 6,

        [Description("Fechamento de caixa")]
        FechamentoCaixa = 7
    }
}