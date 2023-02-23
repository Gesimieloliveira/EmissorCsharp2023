using FusionCore.FusionAdm.Listeners;
using FusionCore.FusionAdm.Setup.Conexao;
using FusionCore.Repositorio.Legacy.Base.Helper;
using FusionCore.Setup;
using NHibernate.Event;

namespace FusionCore.FusionAdm.Sessao
{
    public class SessaoHelperAdm : SessaoHelperBase
    {
        public override string AssemblyStorageName { get; } = "Fusion.Storage";

        protected override IConexaoCfg ObterConfiguracaoDaConexao()
        {
            var conexaoHelper = new ConfiguradorConexao();
            var conexao = conexaoHelper.LerArquivo();
            var cfg = conexao.ToCfg();

            return cfg;
        }

        public IConexaoCfg GetConexaoCfg()
        {
            return ObterConfiguracaoDaConexao();
        }

        protected override IPreInsertEventListener[] ListenerPreInnsert()
        {
            return new IPreInsertEventListener[]
            {
                new EstoqueListener()
            };
        }

        protected override IPreUpdateEventListener[] ListenerPreUpdate()
        {
            return new IPreUpdateEventListener[]
            {
                new EstoqueListener()
            };
        }

        protected override IPostUpdateEventListener[] ListenerPostUpdate()
        {
            return new IPostUpdateEventListener[]
            {
                new SincronizavelListener()
            };
        }

        protected override IPostInsertEventListener[] ListenerPostInsert()
        {
            return new IPostInsertEventListener[]
            {
                new SincronizavelListener()
            };
        }

        protected override IPreDeleteEventListener[] ListenerPreDelete()
        {
            return new IPreDeleteEventListener[]
            {
                new EstoqueListener()
            };
        }
    }
}