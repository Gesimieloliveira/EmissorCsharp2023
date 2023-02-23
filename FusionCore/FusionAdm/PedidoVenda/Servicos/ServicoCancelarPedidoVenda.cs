using FusionCore.FusionAdm.PedidoVenda.Fabricas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.PedidoVenda.Servicos
{
    public class ServicoCancelarPedidoVenda
    {
        private readonly PedidoVenda _pedidoVenda;
        private readonly UsuarioDTO _usuarioLogado;

        public ServicoCancelarPedidoVenda(PedidoVenda pedidoVenda, UsuarioDTO usuarioLogado)
        {
            _pedidoVenda = pedidoVenda;
            _usuarioLogado = usuarioLogado;
        }

        public void Cancelar(string motivoCancelamento)
        {
            _pedidoVenda.Cancelar(motivoCancelamento);
            SalvarPedidoVendaERetornarEstoque();
        }

        private void SalvarPedidoVendaERetornarEstoque()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = FactoryRepositorioPedidoVenda.CriaRepositorio(sessao);

                repositorio.CancelarEstoque(_pedidoVenda, _usuarioLogado);
                repositorio.SalvarAlteracoes(_pedidoVenda);

                transacao.Commit();
            }
        }
    }
}