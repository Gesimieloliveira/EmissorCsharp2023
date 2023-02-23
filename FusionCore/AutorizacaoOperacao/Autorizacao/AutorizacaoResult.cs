using FusionCore.CadastroUsuario;

namespace FusionCore.AutorizacaoOperacao.Autorizacao
{
    public class AutorizacaoResult
    {
        private AutorizacaoResult(bool sucesso, string mensagemErro, IUsuario usuario)
        {
            Sucesso = sucesso;
            MensagemErro = mensagemErro;
            Usuario = usuario;
        }

        public bool Sucesso { get; private set; }
        public string MensagemErro { get; private set; }
        public IUsuario Usuario { get; private set; }

        public static AutorizacaoResult CriarSucesso(IUsuario usuario)
        {
            return new AutorizacaoResult(true, null, usuario);
        }

        public static AutorizacaoResult CriarErro(string mensagemErro)
        {
            return new AutorizacaoResult(false, mensagemErro, null);
        }
    }
}
