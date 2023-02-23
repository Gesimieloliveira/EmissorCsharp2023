using FusionCore.FusionNfce.Listeners;
using FusionCore.FusionNfce.Setup.Conexao;
using FusionCore.Repositorio.Legacy.Base.Helper;
using FusionCore.Setup;
using NHibernate.Event;

namespace FusionCore.FusionNfce.Sessao
{
    public class SessaoNfce : SessaoHelperBase
    {
        public static IConexaoCfg Conexao { get; set; }
        public override string AssemblyStorageName { get; } = "FusionNfce.Storage";

        protected override IConexaoCfg ObterConfiguracaoDaConexao()
        {
            var conexao = new ManipulaConexao().LerArquivo();

            Conexao = conexao.ConexaoNfce.ToCfg();

            return Conexao;
        }

        protected override IPostInsertEventListener[] ListenerPostInsert()
        {
            return new IPostInsertEventListener[]
            {
                new SyncLancamentoCaixaListener(),
                new SyncCaixaInidividualListener(),
                new SyncEventoOperacaoAutorizadaListener()
            };
        }

        protected override IPostUpdateEventListener[] ListenerPostUpdate()
        {
            return new IPostUpdateEventListener[]
            {
                new SyncCaixaInidividualListener()
            };
        }
    }
}