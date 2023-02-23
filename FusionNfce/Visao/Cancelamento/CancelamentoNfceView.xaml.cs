using System.Windows;
using System.Windows.Controls;
using FusionWPF.Base.Utils;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionNfce.Visao.Cancelamento
{
    public partial class CancelamentoNfceView
    {
        private readonly CancelamentoNfceModel _visaoModel;

        public CancelamentoNfceView(CancelamentoNfceModel viewModel)
        {
            _visaoModel = viewModel;
            DataContext = _visaoModel;
            InitializeComponent();

            _visaoModel.Sucesso += SucessoHandler;
            _visaoModel.Falha += FalhaHandler;
        }

        private void FalhaHandler(object sender, string e)
        {
            ProgressBarAgil4.CloseProgressBar();
            DialogBox.MostraAviso(e);
        }

        private void SucessoHandler(object sender, string e)
        {
            ProgressBarAgil4.CloseProgressBar();
            DialogBox.MostraInformacao(e);
            Close();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _visaoModel.Inicializar();
        }

        private void OnClickCancelar(object sender, RoutedEventArgs e)
        {
            ProgressBarAgil4.ShowProgressBar();
            _visaoModel.CancelarAsync();
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            _visaoModel.Validation_Error(sender, e);
        }
    }
}