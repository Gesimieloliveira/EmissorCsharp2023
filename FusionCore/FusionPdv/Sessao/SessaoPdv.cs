using FusionCore.FusionPdv.Setup.BD;
using FusionCore.Repositorio.Legacy.Base.Helper;
using FusionCore.Setup;

namespace FusionCore.FusionPdv.Sessao
{
    public class SessaoPdv : SessaoHelperBase
    {
        public override string AssemblyStorageName { get; } = "Fusion.Storage";

        public static IConexaoCfg Conexao { get; set; }

        public static string FabricaDois = "Fabrica2";
        public static string FabricaTres = "Fabrica3";
        public static string FabricaSessaoPdv = nameof(SessaoPdv);
        private readonly ConexaoSetup _setup = new ConexaoSetup();

        protected override IConexaoCfg ObterConfiguracaoDaConexao()
        {
            var container = _setup.LerArquivoConexao();
            var conexao = container.ConexaoPdv;

            return conexao.ToCfg();
        }
    }
}