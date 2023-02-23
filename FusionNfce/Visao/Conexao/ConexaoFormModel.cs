using System;
using System.ComponentModel.DataAnnotations;
using FusionCore.FusionNfce.Setup.Conexao;
using FusionCore.FusionNfce.Setup.Conexao.Entidade;
using FusionCore.Setup;
using FusionLibrary.VisaoModel;

namespace FusionNfce.Visao.Conexao
{
    public class ConexaoFormModel : ViewModel
    {
        private readonly ManipulaConexao _manipulador;
        private ConexaoBancoDados _conexao;

        [Required(ErrorMessage = @"Servidor SQL da NFC-e é obrigatório!")]
        public string ServidorNfce
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Porta do SQL da NFC-e é obrigatório!")]
        public int PortaNfce
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Instância do SQL da NFC-e é obrigatório!")]
        public string InstanciaNfce
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Usuário do SQL da NFC-e é obrigatório")]
        public string UsuarioNfce
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Senha do SQL da NFC-e é obrigatório")]
        public string SenhaNfce
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Banco de Dados do SQL da NFC-e é obrigatório")]
        public string BancoDadosNfce
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Servidor do SQL do GESTOR é obrigatório")]
        public string ServidorAdm
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Porta do SQL do GESTOR é obrigatório")]
        public int? PortaAdm
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        public string InstanciaAdm
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Usuário do SQL do GESTOR é obrigatório")]
        public string UsuarioAdm
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Senha do SQL do GESTOR é obrigatório")]
        public string SenhaAdm
        {
            get => GetValue();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Banco de Dados do SQL do GESTOR é obrigatório")]
        public string BancoDadosAdm
        {
            get => GetValue();
            set => SetValue(value);
        }

        public ConexaoFormModel()
        {
            _manipulador = new ManipulaConexao();
            Inicializar();
        }

        private void Inicializar()
        {
            if (_manipulador.ArquivoExiste() == false)
            {
                _manipulador.CriarArquivo();
            }

            _conexao = _manipulador.LerArquivo();

            AtualizaView(_conexao);
        }

        private void AtualizaView(ConexaoBancoDados conexao)
        {
            ServidorNfce = conexao.ConexaoNfce.Servidor;
            PortaNfce = conexao.ConexaoNfce.Porta;
            InstanciaNfce = conexao.ConexaoNfce.Instancia;
            UsuarioNfce = conexao.ConexaoNfce.Usuario;
            SenhaNfce = conexao.ConexaoNfce.Senha;
            BancoDadosNfce = conexao.ConexaoNfce.BancoDados;

            ServidorAdm = conexao.ConexaoAdm.Servidor;
            PortaAdm = conexao.ConexaoAdm.Porta;
            InstanciaAdm = conexao.ConexaoAdm.Instancia;
            UsuarioAdm = conexao.ConexaoAdm.Usuario;
            SenhaAdm = conexao.ConexaoAdm.Senha;
            BancoDadosAdm = conexao.ConexaoAdm.BancoDados;
        }

        public void Salvar()
        {
            ThrowExceptionSeExistirErros();

            _conexao.ConexaoNfce.Servidor = ServidorNfce;
            _conexao.ConexaoNfce.Porta = short.Parse(PortaNfce.ToString());
            _conexao.ConexaoNfce.Instancia = InstanciaNfce;
            _conexao.ConexaoNfce.Usuario = UsuarioNfce;
            _conexao.ConexaoNfce.Senha = SenhaNfce;
            _conexao.ConexaoNfce.BancoDados = BancoDadosNfce;

            _conexao.ConexaoAdm.Servidor = ServidorAdm;
            _conexao.ConexaoAdm.Porta = short.Parse(PortaAdm.ToString());
            _conexao.ConexaoAdm.Instancia = InstanciaAdm;
            _conexao.ConexaoAdm.Usuario = UsuarioAdm;
            _conexao.ConexaoAdm.Senha = SenhaAdm;
            _conexao.ConexaoAdm.BancoDados = BancoDadosAdm;

            _manipulador.SalvaXml(_conexao);
        }

        public void TestaConexaoServidor()
        {
            var cfg = new ConexaoCfg
            {
                Instancia = InstanciaAdm,
                Servidor = ServidorAdm,
                Porta = PortaAdm ?? 0,
                BancoDados = BancoDadosAdm,
                Senha = SenhaAdm,
                Usuario = UsuarioAdm
            };

            var dbutility = new DatabaseUtility(cfg);
            var teste = dbutility.TesteConexao();

            if (!teste.IsValido)
            {
                throw new InvalidOperationException($"Não foi possível conectar ao servidor: {teste.DetalheFalha}");
            }
        }
    }
}