using FusionCore.Core.Flags;
using FusionLibrary.Wpf.Conversores;
using System;
using System.ComponentModel;

namespace FusionCore.FusionAdm.Financeiro.Flags
{
    [TypeConverter(typeof(EnumTypeDescriptionConverter))]
    public enum ETipoRecebimento
    {
        [Description("DINHEIRO")]
        Dinheiro = 0,

        [Description("CARTÃO CRÉDITO")]
        CartaoCredito = 1,

        [Description("CARTÃO DÉBITO")]
        CartaoDebito = 2,

        [Description("PIX")]
        Pix = 3,
    }

    public static class ExtTipoRecebimento
    {
        public static ETipoPagamento ToCaixa(this ETipoRecebimento tipoRecebimento)
        {
            switch(tipoRecebimento)
            {
                case ETipoRecebimento.Dinheiro: return ETipoPagamento.Dinheiro;
                case ETipoRecebimento.CartaoCredito: return ETipoPagamento.CartaoCredito;
                case ETipoRecebimento.CartaoDebito: return ETipoPagamento.CartaoDebito;
                case ETipoRecebimento.Pix: return ETipoPagamento.Pix;
                default: throw new Exception("Tipo recebimento inválido");
            }
        }
    }
}
