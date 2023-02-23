using FusionCore.FusionNfce.Pagamento;
using FusionNfce.Visao.Principal.FinalizarVenda.FormasPagamento.Contratos;

namespace FusionNfce.Visao.Principal.FinalizarVenda.FormasPagamento
{
    public class CartaoTef : IFormaPagamento
    {
        public CartaoTef(decimal valor)
        {
            Valor = valor;
        }

        public int Id => 9;
        public int IdRepositorio { get; set; }

        public string Descricao => "Cartão TEF";

        public decimal Valor { get; private set; }

        public void DescontaTroco(decimal troco)
        {
            Valor = Valor - troco;
        }

        public FormaPagamentoNfce FormaPagamentoNfce { get; set; }
    }
}