using System;
using System.Windows;
using Fusion.Sessao;
using Fusion.Visao.Usuario;
using Fusion.Visao.Backup;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Sobre;

namespace Fusion.Visao.Menu
{
    public partial class TopMenuUsuario
    {
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;

        private MenuViewModel _viewModel;

        public TopMenuUsuario()
        {
            InitializeComponent();
        }

        private void TopMenuUsuario_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as MenuViewModel;
        }

        private void MenuEditar_OnClick(object sender, RoutedEventArgs e)
        {
            var formModel = new UsuarioFormModel(_sessaoSistema.UsuarioLogado);
            var formUsuario = new UsuarioForm(formModel);
            formUsuario.ShowDialog();
        }

        private void MenuLogout_OnClick(object sender, RoutedEventArgs e)
        {
            if (DialogBox.MostraConfirmacao("Quer mesmo fazer o logout?") != MessageBoxResult.Yes)
                return;

            try
            {
                _viewModel.Deslogar();
            }
            catch (Exception ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void MenuBackup_OnClick(object sender, RoutedEventArgs e)
        {
            var tela = new BackupForm();
            tela.ShowDialog();
        }

        private void MenuSobre_OnClick(object sender, RoutedEventArgs e)
        {
            new SobreForm().ShowDialog();
        }
    }
}