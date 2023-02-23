using System;
using System.Collections.Generic;
using FusionCore.Repositorio.Legacy.Base.Execao;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Contratos.Base.Comando;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Ativos
{
    public class RepositorioComun<T> : IDisposable where T : IEntidade
    {
        public ISession Sessao { get; set; }

        public RepositorioComun(ISession sessao)
        {
            Sessao = sessao;
        }

        public RepositorioComun()
        {
        }

        public void Dispose()
        {
            if (Sessao != null && Sessao.IsOpen)
                Sessao.Dispose();
        }

        public virtual T Salva(T entidade)
        {
            ChecaSessao();

            try
            {
                Sessao.SaveOrUpdate(entidade);
                AutoFlush();

                return entidade;
            }
            catch (Exception e)
            {
                throw HandleError(e);
            }
        }

        private void ChecaSessao()
        {
            if (Sessao == null)
                throw new RepositorioExeption("Sessão não foi adicionada no repositorio");
            if (Sessao.IsOpen == false)
                throw new RepositorioExeption("Sessão está fechada");
        }

        private static Exception HandleError(Exception exception)
        {
            if (exception is RepositorioExeption)
                return exception;

            if (exception is InvalidOperationException)
                return exception;

            return new RepositorioExeption(exception);
        }

        private void AutoFlush()
        {
            if (Sessao.IsOpen && !Sessao.Transaction.IsActive)
                Sessao.Flush();
        }

        public virtual T Mescla(T entidade)
        {
            ChecaSessao();

            try
            {
                var objectMerge = entidade as object;
                Sessao.Merge(objectMerge);
                AutoFlush();
                return (T) objectMerge;
            }
            catch (Exception e)
            {
                throw HandleError(e);
            }
        }

        public virtual T Altera(T entidade)
        {
            ChecaSessao();

            try
            {
                Sessao.Update(entidade);
                AutoFlush();

                return entidade;
            }
            catch (Exception e)
            {
                throw HandleError(e);
            }
        }

        public virtual T Persiste(T entidade)
        {
            ChecaSessao();

            try
            {
                Sessao.Persist(entidade);
                AutoFlush();

                return entidade;
            }
            catch (Exception e)
            {
                throw HandleError(e);
            }
        }

        public virtual void Deleta(T entidade)
        {
            ChecaSessao();

            try
            {
                Sessao.Delete(entidade);
                AutoFlush();
            }
            catch (Exception e)
            {
                throw HandleError(e);
            }
        }

        public virtual IList<T> Busca(IBuscaListagem<T> estrategia)
        {
            ChecaSessao();

            try
            {
                var lista = estrategia.Busca(Sessao);
                return lista;
            }
            catch (Exception e)
            {
                throw HandleError(e);
            }
        }

        public virtual T Busca(IBuscaUnico<T> estrategia)
        {
            ChecaSessao();

            try
            {
                var registro = estrategia.Busca(Sessao);
                return registro;
            }
            catch (Exception e)
            {
                throw HandleError(e);
            }
        }

        public virtual void Executa(IComando comando)
        {
            ChecaSessao();

            try
            {
                comando.ExecutaComando(Sessao);
            }
            catch (Exception ex)
            {
                throw HandleError(ex);
            }
        }

        public virtual T Executa(IComandoDTO<T> comando)
        {
            ChecaSessao();

            try
            {
                var resultado = comando.ExecutaComando(Sessao);
                return resultado;
            }
            catch (Exception ex)
            {
                throw HandleError(ex);
            }
        }
    }
}