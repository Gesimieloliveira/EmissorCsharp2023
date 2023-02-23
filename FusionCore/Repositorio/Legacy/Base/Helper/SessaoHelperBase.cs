using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using FusionCore.Helpers.AssemblyUtils;
using FusionCore.Helpers.AssemblyUtils.Leitura;
using FusionCore.Repositorio.Exceptions;
using FusionCore.Repositorio.Legacy.Contratos.Base.Sessao;
using FusionCore.Setup;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Event;
using static NHibernate.Event.ListenerType;

// ReSharper disable CoVariantArrayConversion

namespace FusionCore.Repositorio.Legacy.Base.Helper
{
    public abstract class SessaoHelperBase : ISessaoHelper
    {
        private readonly ISessionFactory _sessaoFactory;

        protected SessaoHelperBase()
        {
            _sessaoFactory = CriarSesssaoFactory();
        }

        public string ConnectionString { get; set; }
        public bool IsOpen => _sessaoFactory?.IsClosed == false;
        public virtual string AssemblyStorageName { get; } = string.Empty;

        private ISessionFactory CriarSesssaoFactory()
        {
            lock (typeof(SessaoHelperBase))
            {
                try
                {
                    var config = new Configuration();

                    config.AddProperties(ObterMapaDeConfiguracao());
                    config.AddAssembly(AssemblyHelper.LerDoAssemblyChamou(new NomeAssembly()));

                    if (!string.IsNullOrEmpty(AssemblyStorageName))
                    {
                        config.AddAssembly(AssemblyStorageName);
                    }

                    //listeners
                    config.AppendListeners(PreInsert, ListenerPreInnsert()); ;
                    config.AppendListeners(PostInsert, ListenerPostInsert());
                    config.AppendListeners(PreUpdate, ListenerPreUpdate());
                    config.AppendListeners(PostUpdate, ListenerPostUpdate());
                    config.AppendListeners(PreDelete, ListenerPreDelete());
                    config.AppendListeners(PostDelete, ListenerPostDelete());

                    return config.BuildSessionFactory();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 4060)
                    {
                        throw new DatabaseNotFoundException(ex);
                    }

                    throw;
                }
                catch (Exception ex)
                {
                    throw new SessaoHelperException(ex);
                }
            }
        }

        public ISession AbrirSessao()
        {
            try
            {
                var sessaoAtual = _sessaoFactory.OpenSession();
                sessaoAtual.FlushMode = FlushMode.Commit;

                var query = sessaoAtual.CreateSQLQuery("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
                query.ExecuteUpdate();

                sessaoAtual.Flush();
                return sessaoAtual;
            }
            catch (Exception e)
            {
                throw new SessaoHelperException(e);
            }
        }

        public IStatelessSession AbrStatelessSession()
        {
            try
            {
                var sessao = _sessaoFactory.OpenStatelessSession();

                var query = sessao.CreateSQLQuery("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED");
                query.ExecuteUpdate();

                return sessao;
            }
            catch (Exception e)
            {
                throw new SessaoHelperException(e);
            }
        }

        public void Fechar()
        {
            _sessaoFactory.Close();
        }

        private IDictionary<string, string> ObterMapaDeConfiguracao()
        {
            ConnectionString = ObterConfiguracaoDaConexao().CriarStringDeConexao();

            IDictionary<string, string> property = new Dictionary<string, string>();

            property.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            property.Add("connection.isolation", "ReadUncommitted");
            property.Add("dialect", "NHibernate.Dialect.MsSql2008Dialect");
            property.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
            property.Add("connection.connection_string", ConnectionString);
            property.Add("cache.use_second_level_cache", "false");
            property.Add("adonet.batch_size", "1");

            var converter = $"FusionCore.Repositorio.Legacy.Base.Execao.MsSqlExceptionConverter, {nameof(FusionCore)}";
            property.Add("sql_exception_converter", converter);

#if DEBUG
            property.Add("format_sql", "true");
#endif

            return property;
        }

        protected virtual IPreInsertEventListener[] ListenerPreInnsert()
        {
            return new IPreInsertEventListener[] { };
        }

        protected virtual IPostInsertEventListener[] ListenerPostInsert()
        {
            return new IPostInsertEventListener[] { };
        }

        protected virtual IPreUpdateEventListener[] ListenerPreUpdate()
        {
            return new IPreUpdateEventListener[] { };
        }

        protected virtual IPostUpdateEventListener[] ListenerPostUpdate()
        {
            return new IPostUpdateEventListener[] { };
        }

        protected virtual IPreDeleteEventListener[] ListenerPreDelete()
        {
            return new IPreDeleteEventListener[] {};
        }

        private IPostDeleteEventListener[] ListenerPostDelete()
        {
            return new IPostDeleteEventListener[] {};
        }

        protected abstract IConexaoCfg ObterConfiguracaoDaConexao();
    }
}