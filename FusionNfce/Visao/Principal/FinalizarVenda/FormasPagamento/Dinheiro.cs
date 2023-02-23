using FusionCore.FusionNfce.Pagamento;
using FusionNfce.Visao.Principal.FinalizarVenda.FormasPagamento.Contratos;

namespace FusionNfce.Visao.Principal.FinalizarVenda.FormasPagamento
{
    public class Dinheiro : IFormaPagamento
    {
        public Dinheiro(decimal valor)
        {
            Valor = valor;
        }

        public int Id => 1;
        public int IdRepositorio { get; set; }
        public string Descricao => "Dinheiro";
        public decimal Valor { get; private set; }

        public void DescontaTroco(decimal troco)
        {
            Valor = Valor - troco;
        }

        public FormaPagamentoNfce FormaPagamentoNfce { get; set; }
    }
}