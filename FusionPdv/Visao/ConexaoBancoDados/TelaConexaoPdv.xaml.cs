using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionPdv.Visao.ConexaoBancoDados
{
    public partial class TelaConexaoPdv
    {
        private readonly ConexaoPdvModel _viewModel;

        public TelaConexaoPdv()
        {
            _viewModel = new ConexaoPdvModel();

            DataContext = _viewModel;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _viewModel.CarregarDados();
        }

        private void SalvarConfiguracoesHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.SalvarConfiguracoes();
                Close();
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro("Falha salvar configurações", ex);
            }
        }

        private void FecharHandler(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}