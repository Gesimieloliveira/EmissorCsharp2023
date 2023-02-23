using System;
using System.Collections.Generic;
using FusionCore.Repositorio.Contratos;
using FusionCore.Repositorio.Legacy.Base.Helper;
using NHibernate;

namespace FusionCore.Repositorio.FusionAdm
{
    [Serializable]
    public abstract class Repositorio<T, TId> : RepositorioBase, IRepositorio<T, TId> where T : class
    {
        protected Repositorio(ISession sessao) : base(sessao)
        {
        }

        public void Dispose()
        {
            if (Sessao?.IsOpen == true)
            {
                Sessao.Close();
            }
        }

        public virtual T GetPeloId(TId id)
        {
            return Sessao.Get<T>(id);
        }

        public virtual IList<T> BuscaTodos()
        {
            return Sessao.QueryOver<T>().List();
        }

        public void Refresh(T entidade)
        {
            Sessao.Refresh(entidade);
        }

        public void Evict(T entidade)
        {
            Sessao.Evict(entidade);
        }

        public ITransaction BeginTransaction()
        {
            return Sessao.BeginTransaction();
        }

        protected void ThrowExceptionSeNaoExisteTransacao()
        {
            if (Sessao.Transaction.IsActive)
            {
                return;
            }

            throw new SessaoHelperException("Necessário uma transação de dados ativa para executar essa operação");
        }

        protected void Flush()
        {
            if (Sessao.Transaction.IsActive == false)
            {
                Sessao.Flush();
            }
        }

        public void Commit()
        {
            if (Sessao.Transaction.IsActive)
            {
                Sessao.Transaction.Commit();
            }
        }
    }
}
