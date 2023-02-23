using FusionCore.FusionPdv.Setup.BD;
using FusionCore.Repositorio.Legacy.Base.Helper;
using FusionCore.Setup;

namespace FusionCore.FusionPdv.Sessao
{
    public class SessaoTransmissaoLocalEcf : SessaoHelperBase
    {
        public override string AssemblyStorageName { get; } = "Fusion.Storage";
        private readonly ConexaoSetup _setup = new ConexaoSetup();

        protected override IConexaoCfg ObterConfiguracaoDaConexao()
        {
            var container = _setup.LerArquivoConexao();

            return container.ConexaoPdv.ToCfg();
        }
    }
}