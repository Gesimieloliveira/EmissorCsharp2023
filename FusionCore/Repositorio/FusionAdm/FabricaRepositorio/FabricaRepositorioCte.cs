using NHibernate;

namespace FusionCore.Repositorio.FusionAdm.FabricaRepositorio
{
    public class FabricaRepositorioCte : IFabricaRepositorioCte
    {
        public IRepositorioCartaCorrecaoCte CriaRepositorioCartaCorrecao(ISession sessao)
        {
            return new RepositorioCte(sessao);
        }
    }
}