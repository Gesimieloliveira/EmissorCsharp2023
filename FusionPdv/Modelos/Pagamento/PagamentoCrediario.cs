using System;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionPdv.Modelos.FormaPagamento;

namespace FusionPdv.Modelos.Pagamento
{
    public class PagamentoCrediario : IPagamento
    {
        public PagamentoCrediario()
        {
            FormaPagamento = CarregarFormasDePagamento.PegarFormaDePagamento(CarregarFormasDePagamento.Crediario);
        }

        public decimal Valor { get; set; }

        public IFormaPagamento FormaPagamento { get; set; }
        public bool Pagou
        {
            get { throw new ArgumentException("Não pode ser utilizado"); }
            set { throw new ArgumentException("Não pode ser utilizado"); }
        }

        public void Calcula(VendaEcfDt vendaEcfDt)
        {
            vendaEcfDt.TotalRecebido += Valor;
        }
    }
}