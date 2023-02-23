using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Usuario
{
    public partial class UsuarioForm
    {
        private readonly UsuarioFormModel _viewModel;

        public UsuarioForm(UsuarioFormModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.SalvarModel();
                DialogBox.MostraInformacao("Usuario salvo com sucesso");
                Close();
            }
            catch (Exception ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = _viewModel;
            TbLogin.Focus();

            _viewModel.Inicializar();
        }
    }
}