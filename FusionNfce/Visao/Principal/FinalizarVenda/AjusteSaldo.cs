using System;
using FusionCore.FusionNfce.Pagamento;
using FusionNfce.Visao.Principal.FinalizarVenda.FormasPagamento.Contratos;

namespace FusionNfce.Visao.Principal.FinalizarVenda
{
    public class AjusteSaldo : IFormaPagamento
    {
        public static string FormaPagamentoCodigo = "10";

        public AjusteSaldo(decimal valor, string descricao)
        {
            Valor = valor;
            Descricao = descricao;
        }

        public int Id => 10;
        public int IdRepositorio { get; set; }
        public string Descricao { get; }
        public decimal Valor { get; private set; }

        public void DescontaTroco(decimal troco)
        {
            throw new InvalidOperationException("Crédiario não é permitido troco.");
        }

        public FormaPagamentoNfce FormaPagamentoNfce { get; set; }
    }
}