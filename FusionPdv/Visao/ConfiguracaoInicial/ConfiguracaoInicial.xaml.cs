using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using FusionPdv.Servicos.Ecf;
using FusionPdv.Servicos.ValidacaoInicial;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionPdv.Visao.ConfiguracaoInicial
{

    public partial class ConfiguracaoInicial
    {
        private readonly ConfiguracaoInicialModel _configuracaoInicialModel;

        public ConfiguracaoInicial()
        {
            InitializeComponent();

            try
            {
                _configuracaoInicialModel = new ConfiguracaoInicialModel();
                DataContext = _configuracaoInicialModel;


                if (_configuracaoInicialModel.ExisteCodigoNaEcf()) return;

                BtnSalvar.Visibility = Visibility.Collapsed;
                BtnSalvarDados.Visibility = Visibility.Visible;
                BtnDesativar.Visibility = Visibility.Visible;
                CbModelo.IsEnabled = false;
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
                Close();
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (BtnSalvar.Content.Equals("Salvar (F2)"))
            {
                if (e.Key == Key.F2)
                {
                    BtnSalvar_Click(sender, e);
                }
            }
            else
            {
                if (e.Key == Key.F2)
                {
                    BtnDesativar_Click(sender, e);
                }
            }


            if (e.Key == Key.Escape)
                Close();
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            var messageBoxResult = MessageBox.Show("Deseja realmente ATIVAR a impressora atual?", "ATIVAR IMPRESSORA", MessageBoxButton.YesNo);

            if (messageBoxResult != MessageBoxResult.Yes) return;
            SalvarImpressora("Ecf trocada com sucesso.");
        }

        private void BtnDesativar_Click(object sender, RoutedEventArgs e)
        {
            var messageBoxResult = DialogBox.MostraConfirmacao("Deseja realmente DESATIVAR a impressora atual?");

            if (messageBoxResult != MessageBoxResult.Yes) return;
            
            try
            {
                _configuracaoInicialModel.DesativarEcf();
                new EcfDesativar().Desativar();
                MessageBox.Show("Ecf desativa com sucesso.");
                BtnDesativar.Visibility = Visibility.Collapsed;
                BtnSalvarDados.Visibility = Visibility.Collapsed;
                BtnSalvar.Visibility = Visibility.Visible;
                CbModelo.IsEnabled = true;
            }
            catch (ExceptionMd5 ex)
            {
                DialogBox.MostraErro(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro(ex.Message);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ConfiguracaoInicial_OnClosing(object sender, CancelEventArgs e)
        {
        }

        private void BtnSalvarDados_OnClick(object sender, RoutedEventArgs e)
        {
            var messageBoxResult = MessageBox.Show("Salvar novos dados da impressora?", "IMPRESSORA", MessageBoxButton.YesNo);

            if (messageBoxResult != MessageBoxResult.Yes) return;

            SalvarImpressora("Dados Salvo com sucesso.");
        }

        private void SalvarImpressora(string msg)
        {
            try
            {
                _configuracaoInicialModel.Salvar();
                MessageBox.Show(msg);
                BtnSalvar.Visibility = Visibility.Collapsed;
                BtnDesativar.Visibility = Visibility.Visible;
                BtnSalvarDados.Visibility = Visibility.Visible;
                CbModelo.IsEnabled = false;
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro(ex.Message, ex);
            }
        }
    }
}
