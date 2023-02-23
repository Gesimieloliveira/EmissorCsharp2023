using System;
using FusionNfce.Visao.Principal.FinalizarVenda.FormasPagamento.Contratos;

namespace FusionNfce.Visao.Principal.FinalizarVenda.FormasPagamento
{
    public enum FormaPagamento
    {
        Dinheiro = 0,
        CartaoPos = 1,
        Crediario = 2,
        AjusteSaldo = 3,
        Nenhum = 4,
        Desconto = 5,
        Acrescimo = 6,
        CartaoDebito = 7,
        CartaoCredito = 8,
        CartaoTef = 9,
        Outros = 99,
        Pix = 10,
    }

    public static class FormaPagamentoExt
    {
        public static IFormaPagamento GetFormaPagamento(this FormaPagamento formaPagamento, decimal valor)
        {
            switch (formaPagamento)
            {
                case FormaPagamento.Dinheiro:
                    return new Dinheiro(valor);
                case FormaPagamento.CartaoPos:
                    return new CartaoPos(valor);
                case FormaPagamento.Crediario:
                    return new Crediario(valor);
                case FormaPagamento.CartaoDebito:
                    return new CartaoDebito(valor);
                case FormaPagamento.CartaoCredito:
                    return new CartaoCredito(valor);
                case FormaPagamento.Outros:
                    return new Outros(valor);

                case FormaPagamento.Desconto:
                    return new AjusteSaldo(valor, "Desconto");
                case FormaPagamento.Acrescimo:
                    return new AjusteSaldo(valor, "Acréscimo");
                case FormaPagamento.AjusteSaldo:
                    return new AjusteSaldo(valor, "Ajuste de Saldo");
                case FormaPagamento.CartaoTef:
                    return new CartaoTef(valor);
                case FormaPagamento.Pix:
                    return new Pix(valor);
            }

            throw new InvalidOperationException("Forma de pagamento inválida");
        }
    }
}