using NHibernate;

namespace FusionCore.Repositorio.Legacy.Contratos.Base.Comando
{
    public interface IComando
    {
        void ExecutaComando(ISession sessao);
    }
}