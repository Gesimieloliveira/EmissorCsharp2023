using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.FusionAdm.PedidoVenda.Fabricas
{
    public static class FactoryRepositorioPedidoVenda
    {
        public static RepositorioPedidoVenda CriaRepositorio(ISession sessao)
        {
            return new RepositorioPedidoVenda(sessao);
        }
    }
}