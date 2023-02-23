using FusionCore.FusionNfce.Setup.Conexao;
using FusionCore.Repositorio.Legacy.Base.Helper;
using FusionCore.Setup;

namespace FusionCore.FusionNfce.Sessao
{
    public class SessaoServerNfce : SessaoHelperBase
    {
        public override string AssemblyStorageName { get; } = "Fusion.Storage";

        protected override IConexaoCfg ObterConfiguracaoDaConexao()
        {
            var conexao = new ManipulaConexao().LerArquivo();

            return conexao.ConexaoAdm.ToCfg();
        }
    }
}