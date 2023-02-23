using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Sessao;
using FusionLibrary.Helper.Criptografia;

namespace FusionCore.AutorizacaoOperacao.Autorizacao
{
    public class AutorizarUsuarioNfce : IAutorizarUsuario
    {
        private readonly ISessaoManager _sessaoManager;

        public AutorizarUsuarioNfce(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public AutorizacaoResult Autorizar(string login, string senha, Permissao permissao)
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioUsuarioNfce(sessao);

                var usuario = repositorio.BuscaUsuarioPeloLogin(login);

                if (usuario == null)
                {
                    return AutorizacaoResult.CriarErro("Usuário ou senha inválidos!");
                }

                var senhaCriptografada = SenhaHelper.CriptografarSenha(senha);

                if (usuario.Senha != senhaCriptografada)
                {
                    return AutorizacaoResult.CriarErro("Usuário ou senha inválidos!");
                }

                if (usuario.VerificaPermissao.IsTemPermissao(permissao))
                {
                    return AutorizacaoResult.CriarSucesso(usuario);
                }

                return AutorizacaoResult.CriarErro("Usuário sem permissão!");
            }
        }
    }
}
