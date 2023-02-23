using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Base;

namespace FusionCore.Vendas.Faturamentos
{
    public class FaturamentoVendedor : EntidadeBase<int>
    {
        //usado no nhbiernate
        private int _faturamentoVendaId;

        private FaturamentoVendedor() { }

        public FaturamentoVendedor(Vendedor vendedor, FaturamentoVenda venda) : this()
        {
            Vendedor = vendedor;
            Venda = venda;
        }

        protected override int ChaveUnica => _faturamentoVendaId;
        public FaturamentoVenda Venda { get; private set; }
        public Vendedor Vendedor { get; private set; }
        public string ObterNome => Vendedor.Nome;

        public void AlteraVendedor(Vendedor vendedor)
        {
            Vendedor = vendedor;
        }
    }
}