using System;
using System.Threading;
using System.Windows.Media;
using System.Windows.Threading;
using FontAwesome.WPF;
using FusionCore.FusionPdv.Configuracoes;
using FusionCore.FusionPdv.Financeiro;
using FusionCore.FusionPdv.Sessao;
using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.AssemblyUtils;
using FusionCore.Helpers.AssemblyUtils.Leitura;
using FusionCore.Helpers.Sincronizador;
using FusionCore.Repositorio.FusionPdv;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Base.Execao;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionLibrary.Helper.Criptografia;
using FusionLibrary.VisaoModel;
using FusionPdv.Servicos.ArquivoAuxiliar;
using NHibernate;

namespace FusionPdv.Visao.Principal
{
    public class LoginModel : ModelBase
    {
        private string _login;
        private string _senha;
        private string _versao;
        private DispatcherTimer _timerStatusServidor;
        private FontAwesomeIcon _statusServidor;
        private Brush _corStatus;
        private readonly Login _loginTela;

        public string Versao
        {
            get { return _versao; }
            set
            {
                _versao = value;
                PropriedadeAlterada();
            }
        }

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

        public FontAwesomeIcon StatusServidor
        {
            get { return _statusServidor; }
            set
            {
                _statusServidor = value;
                PropriedadeAlterada();
            }
        }

        public Brush CorStatus
        {
            get { return _corStatus; }
            set
            {
                _corStatus = value;
                PropriedadeAlterada();
            }
        }

        public LoginModel(Login login)
        {
            _loginTela = login;
            Versao = AssemblyHelper.LerDoAssemblyPrincipal(new Versao3Digito());
            IniciaTimerStatusServico();
            VerificaStatusServidor();
        }

        private void IniciaTimerStatusServico()
        {
            _timerStatusServidor = new DispatcherTimer(DispatcherPriority.Background, _loginTela.Dispatcher);
            _timerStatusServidor.Tick += DispatcherTimer_StatusServidor;
            _timerStatusServidor.Interval = new TimeSpan(0, 0, 0, 15, 0);
            _timerStatusServidor.Start();
        }

        private void DispatcherTimer_StatusServidor(object sender, EventArgs e)
        {
            VerificaStatusServidor();
        }

        private void VerificaStatusServidor()
        {
            new Thread(() =>
            {
                try
                {
                    new ConsultarStatusDoServico().Executar("FusionPdvSincronizador");
                    StatusServidor = FontAwesomeIcon.CheckCircle;
                    CorStatus = Brushes.LawnGreen;
                }
                catch (Exception)
                {
                    StatusServidor = FontAwesomeIcon.TimesCircle;
                    CorStatus = Brushes.DarkRed;
                }
            }).Start();
        }

        public void Entrar()
        {
            if (string.IsNullOrEmpty(_login)) throw new InvalidOperationException("Porfavor digitar um usuário.");
            if (string.IsNullOrEmpty(_senha)) throw new InvalidOperationException("Porfavor digitar uma senha.");

            try
            {
                using (var sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao())
                {
                    var usuarioRepositorio = new UsuarioRepositorio(sessao);
                    var usuario = usuarioRepositorio.FazerLogin(_login, Sha1Helper.Computar(_senha));
                    if (usuario == null)
                        throw new InvalidOperationException("Senha ou usuário inválido.");

                    SessaoSistema.UsuarioLogado = usuario;
                    SessaoSistema.ConfiguracoesBalanca = BuscaConfiguracoesBalanca(sessao);
                    SessaoSistema.ConfiguracoesFinanceiro = BuscaConfiguracoesFinanceiro(sessao);
                    SessaoSistema.Empresa = BuscaEmpresa(sessao);
                    SessaoSistema.LogoCaixa = BuscaLogo(sessao);

                    new FazerBackupArquivoAuxiliar("2").EfetuarBackup();
                }

                DeletaLogSincronizador();
                DeletaFusionPdvLog();
            }
            catch (RepositorioExeption ex)
            {
                throw new RepositorioExeption(ex.Message, ex);
            }
        }

        private static void DeletaLogSincronizador()
        {
            var arquivo = new ManipulaArquivo(ManipulaArquivo.LocalAplicacao() + @"\pdv-sincronizador.log");
            if (!arquivo.IsArquivoEmUso())
                arquivo.DeletaSeExistir();
        }

        private static void DeletaFusionPdvLog()
        {
            var arquivo2 = new ManipulaArquivo(ManipulaArquivo.LocalAplicacao() + @"\FusionPdv.log");
            if (!arquivo2.IsArquivoEmUso())
                arquivo2.DeletaSeExistir();
        }

        private ConfiguracaoFrenteCaixaPdv BuscaLogo(ISession sessao)
        {
            var repositorio = new RepositorioConfiguracaoFrenteCaixaPdv(sessao);
            return repositorio.BuscarUnico();
        }

        private EmpresaDt BuscaEmpresa(ISession sessao)
        {
            var repositorio = new EmpresaRepositorio(sessao);

            var empresa = repositorio.BuscaTodos()[0];

            return empresa;
        }

        private ConfiguracaoFinanceiroPdv BuscaConfiguracoesFinanceiro(ISession sessao)
        {
            var repositorio = new RepositorioConfiguracaoFinanceiroPdv(sessao);

            var configuracaoFinanceiro = repositorio.BuscarUnico();

            return configuracaoFinanceiro;
        }

        private BalancaPdv BuscaConfiguracoesBalanca(ISession session)
        {
            var repositorio = new RepositorioBalancaPdv(session);

            var balanca = repositorio.BuscaUnicaConfiguracao();

            return balanca;
        }
    }
}