using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.PerfilNfe
{
    public partial class PerfilNfeForm
    {
        private readonly PerfilNfeFormModel _viewModel;

        public PerfilNfeForm(PerfilNfeFormModel formModel)
        {
            _viewModel = formModel;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _viewModel.Inicializar();
            DataContext = _viewModel;
            ConfigurarTela();
        }
        private void ConfigurarTela()
        {
            if (_viewModel.Id == 0)
            {
                BotaoSalvar.Content = "Salvar Inclusão";
                BotaoDeletar.Visibility = Visibility.Collapsed;
            };
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.SalvaAlteracoes();
                DialogBox.MostraInformacao("Perfil NF-e salvo com sucesso");
                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void OnClickDeletar(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.DeletaRegistro();
                DialogBox.MostraInformacao("Perfil NF-e foi deletado");
                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}