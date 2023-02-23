using FusionCore.FusionAdm.Setup.Conexao;

namespace Fusion.Conversor.Core.BancoDados
{
    public class ConexaoFacade
    {
        private readonly ConfiguradorConexao _configurador = new ConfiguradorConexao();

        public DadosConexao GetDadosConexao()
        {
            return _configurador.LerArquivo();
        }
    }
}