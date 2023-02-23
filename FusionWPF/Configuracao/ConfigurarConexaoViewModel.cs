using System;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionAdm.Setup.Conexao;
using FusionCore.Setup;
using FusionLibrary.VisaoModel;

namespace FusionWPF.Configuracao
{
    public sealed class ConfigurarConexaoViewModel : ViewModel
    {
        private DadosConexao _dadosConexao;
        private readonly ConfiguradorConexao _configuradorConexao;

        public ConfigurarConexaoViewModel(ConfiguradorConexao configurador)
        {
            _configuradorConexao = configurador;
        }

        public string BancoDados
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string Servidor
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string Instancia
        {
            get => GetValue();
            set => SetValue(value);
        }

        public int? Porta
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        public string Usuario
        {
            get => GetValue();
            set => SetValue(value);
        }

        public string Senha
        {
            get => GetValue();
            set => SetValue(value);
        }

        public void CarregarArquivo()
        {
            try
            {
                _dadosConexao = _configuradorConexao.LerArquivo();

                Servidor = _dadosConexao.Servidor;
                Instancia = _dadosConexao.Instancia;
                BancoDados = _dadosConexao.BancoDados;
                Usuario = _dadosConexao.Usuario;
                Senha = _dadosConexao.Senha;
                Porta = _dadosConexao.Porta;
            }
            catch (ArquivoNaoExisteException)
            {
                Servidor = "LOCALHOST";
                Instancia = "FUSION";
                BancoDados = "FusionAdm";
                Usuario = "sa";
                Senha = "Fusion@ag4";
                Porta = 1433;
            }
        }

        public void SalvarConfiguracao()
        {
            ThrowExceptionSeModeloInvalido();
            ThrowExceptionSeConexaoInvalida();
            SalvarConfiguracaoConexao();
        }

        private void ThrowExceptionSeConexaoInvalida()
        {
            var cfg = new ConexaoCfg
            {
                Servidor = Servidor,
                Porta = Porta ?? 1433,
                Instancia = Instancia,
                BancoDados = BancoDados,
                Usuario = Usuario,
                Senha = Senha
            };

            var dbUtility = new DatabaseUtility(cfg);
            var test = dbUtility.TesteConexao();

            if (test.IsValido)
            {
                return;
            }

            throw new InvalidOperationException($"Falha ao conectar no banco de dados: {test.DetalheFalha}");
        }

        private void SalvarConfiguracaoConexao()
        {
            if (_dadosConexao == null)
            {
                _dadosConexao = new DadosConexao();
            }

            _dadosConexao.Servidor = Servidor;
            _dadosConexao.Usuario = Usuario;
            _dadosConexao.Senha = Senha;
            _dadosConexao.Instancia = Instancia;
            _dadosConexao.BancoDados = BancoDados;
            _dadosConexao.Porta = Porta ?? 1433;

            _configuradorConexao.ArmazenaEmArquivo(_dadosConexao);

            SessaoHelperFactory.Fechar();
        }

        private void ThrowExceptionSeModeloInvalido()
        {
            if (string.IsNullOrEmpty(Servidor))
            {
                throw new InvalidOperationException("Servidor é obrigatório");
            }

            if (string.IsNullOrEmpty(Usuario))
            {
                throw new InvalidOperationException("Usuário é obrigatório");
            }

        }
    }
}