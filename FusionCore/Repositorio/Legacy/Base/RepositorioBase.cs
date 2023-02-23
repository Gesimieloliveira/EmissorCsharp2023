using System;
using System.Collections.Generic;
using FusionCore.Repositorio.Legacy.Base.Execao;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Util;

namespace FusionCore.Repositorio.Legacy.Base
{
    public abstract class RepositorioBase<T>
        where T : IEntidade
    {
        protected RepositorioBase(ISession sessao)
        {
            Sessao = sessao;
        }

        public ISession Sessao { get; set; }

        public virtual T Salvar(T entidade)
        {
            try
            {
                Sessao.SaveOrUpdate(entidade);
                AutoFlush();

                return entidade;
            }
            catch (Exception e)
            {
                throw HandlerException(e);
            }
        }

        protected static Exception HandlerException(Exception exception)
        {
            return new RepositorioExeption(exception);
        }

        private void AutoFlush()
        {
            if (Sessao.IsOpen && !Sessao.Transaction.IsActive)
                Sessao.Flush();
        }

        public virtual T Mesclar(T entidade)
        {
            try
            {
                var objectMerge = entidade as object;
                Sessao.Merge(objectMerge);
                AutoFlush();
                return (T) objectMerge;
            }
            catch (Exception e)
            {
                throw HandlerException(e);
            }
        }

        public virtual T Alterar(T entidade)
        {
            try
            {
                Sessao.Update(entidade);
                AutoFlush();

                return entidade;
            }
            catch (Exception e)
            {
                throw HandlerException(e);
            }
        }

        public virtual T Persistir(T entidade)
        {
            try
            {
                Sessao.Persist(entidade);
                AutoFlush();

                return entidade;
            }
            catch (Exception e)
            {
                throw HandlerException(e);
            }
        }

        public virtual void PersistirLista(IList<T> entidade)
        {
            try
            {
                entidade.ForEach(e => { Sessao.Persist(e); });
                AutoFlush();
            }
            catch (Exception e)
            {
                throw HandlerException(e);
            }
        }

        public virtual void SalvarLista(IList<T> entidade)
        {
            try
            {
                entidade.ForEach(e => Salvar(e));
                AutoFlush();
            }
            catch (Exception e)
            {
                throw HandlerException(e);
            }
        }

        public virtual T Recarregar(T entidade)
        {
            try
            {
                Sessao.Refresh(entidade);
                return entidade;
            }
            catch (Exception e)
            {
                throw new RepositorioExeption(e);
            }
        }

        public virtual void Deletar(T entidade)
        {
            try
            {
                Sessao.Delete(entidade);
                AutoFlush();
            }
            catch (Exception e)
            {
                throw HandlerException(e);
            }
        }

        public virtual IList<T> Busca(T entidade)
        {
            try
            {
                var exemplo = Example.Create(entidade).EnableLike(MatchMode.Anywhere)
                    .IgnoreCase().ExcludeNulls().ExcludeZeroes();

                return Sessao.CreateCriteria(typeof (T)).Add(exemplo).List<T>();
            }
            catch (Exception e)
            {
                throw HandlerException(e);
            }
        }

        public virtual IList<T> BuscaTodos()
        {
            try
            {
                var resultado = Sessao.CreateCriteria(typeof (T)).List<T>();
                return resultado;
            }
            catch (Exception e)
            {
                throw HandlerException(e);
            }
        }

        public virtual T BuscaPorId(object id)
        {
            try
            {
                var resultado = Sessao.Get<T>(id);
                return resultado;
            }
            catch (Exception e)
            {
                throw HandlerException(e);
            }
        }
    }
}