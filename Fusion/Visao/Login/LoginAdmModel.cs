using System;
using System.Reflection;
using Fusion.Exceptions;
using Fusion.Sessao;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionAdm.Setup.Usuario;
using FusionCore.Helpers.AssemblyUtils;
using FusionCore.MigracaoFluente;
using FusionCore.Relatorios;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.Usuario;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.Helper.Criptografia;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Login
{
    public sealed class LoginAdmModel : ViewModel
    {
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;
        private string _login;
        private string _senha;
        private string _versao;

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                PropriedadeAlterada();
            }
        }

        public string Senha
        {
            get { return _senha; }
            set
            {
                _senha = value;
                PropriedadeAlterada();
            }
        }

        public string Versao
        {
            get { return _versao; }
            set
            {
                if (value == _versao) return;
                _versao = value;
                PropriedadeAlterada();
            }
        }

        public LoginAdmModel()
        {
#if DEBUG
            Login = "admin";
            Senha = "agil4";
#endif

            Versao = AssemblyHelper.LerVersao3Digitos(Assembly.GetExecutingAssembly());
        }

        public void ConfigurarUsuario()
        {
            var setup = new UsuarioSetupHelper();
            setup.Inicializar();

            if (setup.UsuarioAdminExiste())
            {
                return;
            }

            setup.CriarUsuarioAdmin();
        }

        public void Logar()
        {
            var cfgConexaoAdm = SessaoHelperFactory.GetConexaoCfg();

            var bancoDeDadosMigradorFluenteAdm = MigracaoFactory.CriaMigrador(cfgConexaoAdm, MigracaoTag.Adm);
            var bancoDeDadosMigradorFluenteRelatorio = MigracaoFactory.CriaMigrador(CriaIConexaoCfg.CriaIConexaoCfgRelatorio(cfgConexaoAdm), MigracaoTag.Relatorio);

            if (bancoDeDadosMigradorFluenteAdm.PrecisaAtualizar || bancoDeDadosMigradorFluenteRelatorio.PrecisaAtualizar)
            {
                throw new PrecisaAtualizarDatabaseException("Preciso atualizar o banco de dados!");
            }

            ChecarVersaoBd(bancoDeDadosMigradorFluenteAdm);

            if (Login == null)
            {
                throw new InvalidOperationException("Preciso que informe um login");
            }

            if (Senha == null)
            {
                throw new InvalidOperationException("Preciso que informe uma senha");
            }

            VerificaSeExisteProdutoSemTabelaEstoque();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioComun<UsuarioDTO>(sessao);
                var usuario = repositorio.Busca(new UsuarioPeloLogin(Login));

                if (usuario == null || !SenhaHelper.SenhaIgual(Senha, usuario.Senha))
                {
                    throw new InvalidOperationException("Credencial inválida");
                }

                _sessaoSistema.UsuarioLogado = usuario;
            }
        }

        private static void ChecarVersaoBd(IMigracao bancoDeDadosMigradorFluenteAdm)
        {
            #if DEBUG
                return;
            #endif

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                new ChecarVersaoBdAdm(bancoDeDadosMigradorFluenteAdm, sessao).Checar();
            }
        }

        private void VerificaSeExisteProdutoSemTabelaEstoque()
        {
            int quantidadeSemEstoque;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioProduto(sessao);
                quantidadeSemEstoque = repositorio.QuantidadeProdutoSemTabelaEstoque();
            }

            if (quantidadeSemEstoque > 0)
            {
                throw new InvalidOperationException("Existem produtos que não contem Estoque, verificar isso com o suporte.");
            }
        }
    }
}