using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FusionCore.FusionAdm.Componentes;
using FusionCore.Helpers.Hidratacao;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionWPF.SendMail
{
    public partial class EnvioEmailView
    {
        private EnvioEmailBehavior ViewModel => DataContext as EnvioEmailBehavior;

        public EnvioEmailView(EnvioEmailBehavior model)
        {
            DataContext = model;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.SucessoEnvio += SucessoEnvioHandler;
            ViewModel.FalhaEnvio += FalhaEnvioHandler;
            TbAssunto.Focus();
        }

        private void FalhaEnvioHandler(object sender, Exception e)
        {
            DialogBox.MostraAviso(e.Message);
        }

        private void SucessoEnvioHandler(object sender, EventArgs e)
        {
            DialogBox.MostraInformacao("Email foi enviado com sucesso");
            Dispatcher.Invoke(Close);
        }

        private void TbEmailKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            ViewModel.AdicionarEmailDigitado();
            ViewModel.EmailDigitado = string.Empty;
            TbEmail.Focus();
        }

        private void RemoveEmailHandler(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button == null)
            {
                return;
            }

            ViewModel.RemoverEmail((Email) button.Tag);
        }

        private void ClickEnviarHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.EnviarEmailAsync();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void TbEmail_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (ViewModel.EmailDigitado.IsNullOrEmpty())
            {
                return;
            }

            ViewModel.AdicionarEmailDigitado();
            ViewModel.EmailDigitado = string.Empty;
            TbEmail.Focus();
        }
    }
}