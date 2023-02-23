using System;
using NHibernate;

namespace FusionCore.Repositorio
{
    public interface IUnitOfWork : IDisposable
    {
        ISession GetSession();
        void Commit();
        void Rollback();
    }
}