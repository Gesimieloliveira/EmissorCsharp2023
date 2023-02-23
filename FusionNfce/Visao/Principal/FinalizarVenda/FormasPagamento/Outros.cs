using FusionCore.FusionNfce.Pagamento;
using FusionNfce.Visao.Principal.FinalizarVenda.FormasPagamento.Contratos;

namespace FusionNfce.Visao.Principal.FinalizarVenda.FormasPagamento
{
    public class Outros : IFormaPagamento
    {
        public Outros(decimal valor)
        {
            Valor = valor;
        }

        public int Id => 99;
        public int IdRepositorio { get; set; }
        public string Descricao => "Outros";
        public decimal Valor { get; private set; }

        public void DescontaTroco(decimal troco)
        {
            Valor = Valor - troco;
        }

        public FormaPagamentoNfce FormaPagamentoNfce { get; set; }
    }
}