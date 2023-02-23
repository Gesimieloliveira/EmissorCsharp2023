using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionPdv.Modelos.FormaPagamento;

namespace FusionPdv.Modelos.Pagamento
{
    public class PagamentoCartaoPos : IPagamento
    {
        public PagamentoCartaoPos()
        {
            FormaPagamento = CarregarFormasDePagamento.PegarFormaDePagamento(CarregarFormasDePagamento.CartaoPos);
        }

        public decimal Valor { get; set; }
        public IFormaPagamento FormaPagamento { get; set; }
        public bool Pagou { get; set; }

        public void Calcula(VendaEcfDt vendaEcfDt)
        {
            vendaEcfDt.TotalRecebido += Valor;
        }
    }
}