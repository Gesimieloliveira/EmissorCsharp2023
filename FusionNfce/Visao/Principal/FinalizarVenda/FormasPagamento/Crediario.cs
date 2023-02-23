using System;
using FusionCore.FusionNfce.Pagamento;
using FusionNfce.Visao.Principal.FinalizarVenda.FormasPagamento.Contratos;

namespace FusionNfce.Visao.Principal.FinalizarVenda.FormasPagamento
{
    public class Crediario : IFormaPagamento
    {
        public static int IdCrediario = 3;

        public Crediario(decimal valor)
        {
            Valor = valor;
        }

        public int Id => 3;
        public int IdRepositorio { get; set; }
        public string Descricao => "Crediário";
        public decimal Valor { get; private set; }
        public void DescontaTroco(decimal troco)
        {
            throw new InvalidOperationException("Crédiario não é permitido troco.");
        }

        public FormaPagamentoNfce FormaPagamentoNfce { get; set; }
    }
}