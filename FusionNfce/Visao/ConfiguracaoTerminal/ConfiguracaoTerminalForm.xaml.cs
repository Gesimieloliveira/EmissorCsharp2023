using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using FusionCore.FusionNfce.Sessao;
using FusionNfce.Visao.BarraDeProgresso;
using FusionNfce.Visao.Conexao;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionNfce.Visao.ConfiguracaoTerminal
{
    public partial class ConfiguracaoTerminalForm
    {
        private readonly ConfiguracaoTerminalFormModel _formModel;

        public ConfiguracaoTerminalForm()
        {
            _formModel = new ConfiguracaoTerminalFormModel();
            DataContext = _formModel;
            InitializeComponent();
        }

        private async void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            ProgressBarAgil4.ShowProgressBar();
            await Task.Factory.StartNew(Salvar);
            ProgressBarAgil4.CloseProgressBar();
        }

        private void Salvar()
        {
            try
            {
                _formModel.Salvar();
                DialogBox.MostraInformacao("Configurações salvas com sucesso");
                Dispatcher.Invoke(Close);
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void OnClick_Fechar(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ConfiguracaoTerminalForm_OnContentRendered(object sender, EventArgs e)
        {
            try
            {
                GerenciaSessaoNfce.GerenciaSessaoInicializaTodasConexoes();
            }
            catch (Exception ex)
            {
                DialogBox.MostraAviso(ex.Message);
                new ConexaoForm().ShowDialog();
                ConfiguracaoTerminalForm_OnContentRendered(sender, e);
                return;
            }

            Dispatcher.Invoke(() =>
            {
                _formModel.DeletaEmissorNfceEEmissorSat();
                _formModel.AtualizarDadosTela();
                _formModel.CarregaConfiguracao();
            });
        }

        private void ConfiguracaoTerminalForm_OnClosing(object sender, CancelEventArgs e)
        {
            try
            {
                var configuracao = _formModel.BuscaConfiguracao();

                if (configuracao == null) throw new InvalidOperationException();
            }
            catch (InvalidOperationException)
            {
                var confirmacao =
                    DialogBox.MostraConfirmacao(
                        "Se fechar esta tela, sem ter uma configuração iremos fechar a aplicação, \nDeseja realmente fechar?\nobrigado",
                        MessageBoxImage.Information);

                if (confirmacao)
                {
                    Dispatcher.CurrentDispatcher.Invoke(
                        () => { Application.Current.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Send); });
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}