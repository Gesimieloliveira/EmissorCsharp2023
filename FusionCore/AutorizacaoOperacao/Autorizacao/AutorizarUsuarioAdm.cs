using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using FusionLibrary.Helper.Criptografia;

namespace FusionCore.AutorizacaoOperacao.Autorizacao
{
    public class AutorizarUsuarioAdm : IAutorizarUsuario
    {
        private readonly ISessaoManager _sessaoManager;       

        public AutorizarUsuarioAdm(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public AutorizacaoResult Autorizar(string login, string senha, Permissao permissao)
        {
            
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioUsuario(sessao);

                var usuario = repositorio.PeloLogin(login);

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
