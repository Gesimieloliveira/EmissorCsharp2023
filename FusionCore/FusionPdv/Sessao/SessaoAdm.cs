using FusionCore.Excecoes.Sessao;
using FusionCore.FusionPdv.Setup.BD;
using FusionCore.Repositorio.Legacy.Base.Helper;
using FusionCore.Setup;
using ConexaoCfg = FusionCore.Setup.ConexaoCfg;

namespace FusionCore.FusionPdv.Sessao
{
    public class SessaoAdm : SessaoHelperBase
    {
        public override string AssemblyStorageName { get; } = "Fusion.Storage";
        private readonly ConexaoSetup _setup = new ConexaoSetup();

        protected override IConexaoCfg ObterConfiguracaoDaConexao()
        {
            var container = _setup.LerArquivoConexao();
            var conexao = container.ConexaoAdm;

            var bancoDeDados = conexao.BancoDados;
            var instancia = $"{conexao.Host}\\{conexao.Instancia}";
            var usuarioBd = conexao.Usuario;
            var senhaBd = conexao.Senha;

            if (string.IsNullOrEmpty(bancoDeDados) || string.IsNullOrEmpty(instancia)
                || string.IsNullOrEmpty(usuarioBd) || string.IsNullOrEmpty(senhaBd))
            {
                throw new ConexaoInvalidaException("Não existe conexão valida no arquivo de Conexoes.agil4");
            }

            return new ConexaoCfg
            {
                BancoDados = bancoDeDados,
                Instancia = instancia,
                Usuario = usuarioBd,
                Senha = senhaBd
            };
        }
    }
}