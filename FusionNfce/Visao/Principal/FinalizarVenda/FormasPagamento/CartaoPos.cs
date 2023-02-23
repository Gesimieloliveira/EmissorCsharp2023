using FusionCore.FusionNfce.Pagamento;
using FusionNfce.Visao.Principal.FinalizarVenda.FormasPagamento.Contratos;

namespace FusionNfce.Visao.Principal.FinalizarVenda.FormasPagamento
{
    public class CartaoPos : IFormaPagamento
    {
        public CartaoPos(decimal valor)
        {
            Valor = valor;
        }

        public int Id => 2;
        public int IdRepositorio { get; set; }

        public string Descricao => "Cartão Pos";

        public decimal Valor { get; private set; }

        public void DescontaTroco(decimal troco)
        {
            Valor = Valor - troco;
        }

        public FormaPagamentoNfce FormaPagamentoNfce { get; set; }
    }
}
