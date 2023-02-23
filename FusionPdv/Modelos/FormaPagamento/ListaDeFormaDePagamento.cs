using System.Collections.Generic;

namespace FusionPdv.Modelos.FormaPagamento
{
    public class ListaDeFormaDePagamento
    {
        private readonly IList<IFormaPagamento> _formaPagamentos;

        public ListaDeFormaDePagamento()
        {
            _formaPagamentos = new List<IFormaPagamento>();

            Add(new Dinheiro());
            Add(new CartaoTef());
            Add(new CartaoPos());
            Add(new Crediario());
        }

        public IList<IFormaPagamento> ObterFormaDePagamentos()
        {
            return _formaPagamentos;
        }

        private void Add(IFormaPagamento formaPagamento)
        {
            _formaPagamentos.Add(formaPagamento);
        }
    }
}
