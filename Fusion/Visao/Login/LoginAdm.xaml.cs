using System;
using System.ComponentModel;
using System.Windows;
using Fusion.Exceptions;
using Fusion.Sessao;
using Fusion.Visao.Menu;
using Fusion.Visao.Splash;
using Fusion.Visao.Vendas.FaturamentoCheckout;
using FusionCore.FusionAdm.Setup.Conexao;
using FusionCore.Repositorio.Legacy.Base.Helper;
using FusionCore.Sessao;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Configuracao;

namespace Fusion.Visao.Login
{
    public partial class LoginAdm
    {
        private bool _mantemAplicacaoAberta;
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;
        private readonly ISessaoManager _sessaoManager = new SessaoManagerAdm();

        public LoginAdm()
        {
            DataContext = new LoginAdmModel();
            InitializeComponent();
        }

        private LoginAdmModel ViewModel => DataContext as LoginAdmModel;

        private void OnLoadedHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.ConfigurarUsuario();
        }

        private void OnClickEntrar(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.Logar();

                if (ApenasModoFaturamento())
                {
                    FaturamentoCheckout.Factory.CurrentView.ShowView();
                    Close();
                    return;
                }

                var menu = MenuPrincipal.Instancia.Inicializar(new MenuViewModel());

                menu.Show();

                Close();
            }
            catch (SessaoHelperException ex)
            {
                DialogBox.MostraErro("Que pena, não consegui conectar com o Banco de Dados", ex);
            }
            catch (PrecisaAtualizarDatabaseException)
            {
                DialogBox.MostraAviso("Banco de dados está desatualizado. Irei checar as configurações novamente.");

                _mantemAplicacaoAberta = true;

                new SplashFusion().Show();
                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private bool ApenasModoFaturamento()
        {
            return _sessaoSistema.AcessoConcedido.PossuiFusionStarter &&
                   _sessaoSistema.UsuarioLogado.ApenasFaturamento;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (_sessaoSistema.UsuarioLogado != null || _mantemAplicacaoAberta)
            {
                return;
            }

            Application.Current.Shutdown();
        }

        private void OnClickConfigurarConexao(object sender, RoutedEventArgs e)
        {
            var configurador = new ConfiguradorConexao();
            var contexto = new ConfigurarConexaoViewModel(configurador);

            new ConfigurarConexaoView(contexto).ShowDialog();
        }

        private void LoginAdm_OnContentRendered(object sender, EventArgs e)
        {
#if DEBUG
            BtnEntrar.Focus();
#endif
        }
    }
}