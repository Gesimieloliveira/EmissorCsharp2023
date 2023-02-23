using System.Data;
using NHibernate;

namespace FusionCore.Sessao
{
    public interface ISessaoManager
    {
        bool FactoryIsOpen { get; }
        string ConnectionString { get; }
        void CarregarFactory();
        void FecharFactory();
        ISession CriaSessao();
        ISession CriaSessao(IsolationLevel level);
        ISession GetSessaoAberta();
        IStatelessSession CriaStatelessSession();
    }
}