using NHibernate;

namespace FusionCore.Repositorio.Legacy.Contratos.Base.Sessao
{
    public interface ISessaoHelper
    {
        string ConnectionString { get; }
        ISession AbrirSessao();
        IStatelessSession AbrStatelessSession();
        void Fechar();
    }
}