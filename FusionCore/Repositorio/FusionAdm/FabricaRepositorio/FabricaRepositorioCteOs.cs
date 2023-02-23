using NHibernate;

namespace FusionCore.Repositorio.FusionAdm.FabricaRepositorio
{
    public class FabricaRepositorioCteOs : IFabricaRepositorioCte
    {
        public IRepositorioCartaCorrecaoCte CriaRepositorioCartaCorrecao(ISession sessao)
        {
            return new RepositorioCteOs(sessao);
        }
    }
}