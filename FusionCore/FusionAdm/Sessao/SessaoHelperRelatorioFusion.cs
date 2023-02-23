using FusionCore.FusionAdm.Setup.Conexao;
using FusionCore.Repositorio.Legacy.Base.Helper;
using FusionCore.Setup;

namespace FusionCore.FusionAdm.Sessao
{
    public class SessaoHelperRelatorioFusion : SessaoHelperBase
    {
        public override string AssemblyStorageName { get; } = "Fusion.Storage";

        protected override IConexaoCfg ObterConfiguracaoDaConexao()
        {
            var conexaoHelper = new ConfiguradorConexao();
            var conexao = conexaoHelper.LerArquivo();

            var cfg = conexao.ToCfg();

            cfg.BancoDados = "FusionRelatorio";

            return cfg;
        }
    }
}