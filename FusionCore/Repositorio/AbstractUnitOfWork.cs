using System.Data;
using NHibernate;

namespace FusionCore.Repositorio
{
    public abstract class AbstractUnitOfWork : IUnitOfWork
    {
        private ISession _session;

        public ISession GetSession()
        {
            if (_session == null || _session.IsOpen)
            {
                return _session;
            }

            _session = CriarSessaoBancoDados();
            _session.BeginTransaction(IsolationLevel.ReadCommitted);

            return _session;
        }

        public void Commit()
        {
            if (_session?.Transaction.IsActive == true)
            {
                _session.Transaction.Commit();
            }
        }

        public void Rollback()
        {
            if (_session?.Transaction.IsActive == true)
            {
                _session.Transaction.Rollback();
            }
        }

        public void Dispose()
        {
            Rollback();
            _session?.Dispose();
        }

        protected abstract ISession CriarSessaoBancoDados();
    }
}