using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.PedidoVenda
{
    public class PedidoVendedor : EntidadeBase<int>
    {
        //usado no nhbiernate
        private int _pedidoVendaId;

        private PedidoVendedor() { }
        
        public PedidoVendedor(Vendedor vendedor, PedidoVenda pedidoVenda) : this()
        {
            Vendedor = vendedor;
            PedidoVenda = pedidoVenda;
        }

        protected override int ChaveUnica => _pedidoVendaId;
        public PedidoVenda PedidoVenda { get; private set; }
        public Vendedor Vendedor { get; private set; }
        public string ObterNome => Vendedor.Nome;

        public void AlteraVendedor(Vendedor vendedor)
        {
            Vendedor = vendedor;
        }
    }
}