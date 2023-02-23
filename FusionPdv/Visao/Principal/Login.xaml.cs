using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using FusionPdv.Ecf;
using FusionPdv.Visao.ConexaoBancoDados;
using FusionPdv.Visao.Tef;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionPdv.Visao.Principal
{
    public partial class Login
    {
        private readonly LoginModel _loginModel;
        private bool FezLogin { get; set; }

        public Login()
        {
            _loginModel = new LoginModel(this);
            InitializeComponent();
            DataContext = _loginModel;
            TbUsuario.Focus();
        }

        private void BtEntrar_OnClick(object sender, RoutedEventArgs e)
        {
            EfetuarLogin();
        }

        private void EfetuarLogin()
        {
            try
            {
                _loginModel.Entrar();
                FezLogin = true;
                new Caixa().Show();
                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void Login_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
                case Key.F11:
                    new TelaConexaoPdv().ShowDialog();
                    break;
                case Key.F12:
                    Conexao_Tef_OnClick(sender, e);
                    break;
            }
        }

        private void Conexao_OnClick(object sender, RoutedEventArgs e)
        {
            new TelaConexaoPdv().ShowDialog();
        }

        private void Conexao_Tef_OnClick(object sender, RoutedEventArgs e)
        {
            new ConfiguraTefForm().ShowDialog();
        }

        private void Login_OnClosing(object sender, CancelEventArgs e)
        {
            if (FezLogin)
            {
                return;
            }

            try
            {
                SessaoEcf.EcfFiscal.Close();
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }
    }
}