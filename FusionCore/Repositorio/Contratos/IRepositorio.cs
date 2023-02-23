using System;
using System.Collections.Generic;
using NHibernate;

namespace FusionCore.Repositorio.Contratos
{
    public interface IRepositorio<T, in TId> : IDisposable
    {
        T GetPeloId(TId id);
        IList<T> BuscaTodos();
        ITransaction BeginTransaction();
        void Evict(T entidade);
    }
}