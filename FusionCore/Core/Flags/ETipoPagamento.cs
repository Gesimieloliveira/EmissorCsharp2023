using System.ComponentModel;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionLibrary.Wpf.Conversores;

namespace FusionCore.Core.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum ETipoPagamento
    {
        [Description("DINHEIRO")]
        Dinheiro = 0,

        [Description("A PRAZO")]
        CreditoLoja = 1,

        [Description("CARTÃO CRÉDITO")]
        CartaoCredito = 2,

        [Description("CARTÃO DÉBITO")]
        CartaoDebito = 3,

        [Description("PIX")]
        Pix = 4,
    }

}