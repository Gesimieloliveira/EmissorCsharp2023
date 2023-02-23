using System.ComponentModel;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.ControleCaixa
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum TipoLancamentoCaixa
    {
        [Description("Direto no caixa loja")]
        LancamentoCaixaLoja = 0,

        [Description("No caixa individual")]
        LancamentoCaixaIndividual = 1
    }
}