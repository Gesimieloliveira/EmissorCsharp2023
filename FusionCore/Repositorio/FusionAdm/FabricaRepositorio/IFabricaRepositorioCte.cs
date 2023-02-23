using NHibernate;

namespace FusionCore.Repositorio.FusionAdm.FabricaRepositorio
{
    public interface IFabricaRepositorioCte
    {
        IRepositorioCartaCorrecaoCte CriaRepositorioCartaCorrecao(ISession sessao);
    }
}