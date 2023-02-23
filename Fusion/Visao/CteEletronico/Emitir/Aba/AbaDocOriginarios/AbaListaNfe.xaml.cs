using System.Windows;
using System.Windows.Input;
using Fusion.Visao.CteEletronico.Emitir.Aba.Models;

namespace Fusion.Visao.CteEletronico.Emitir.Aba.AbaDocOriginarios
{
    public partial class AbaListaNfe
    {
        private AbaDocumentosOriginariosModel _viewModel;

        public AbaListaNfe()
        {
            InitializeComponent();
        }

        private void OnDoubleClickItem(object sender, MouseButtonEventArgs e)
        {
        }

        private void OnClickDeletaItem(object sender, RoutedEventArgs e)
        {
            _viewModel.DeletaDocumentoNfeSelecionada();
        }

        private void AbaListaNfe_OnLoaded(object sender, RoutedEventArgs e)
        {
            var model = DataContext as AbaDocumentosOriginariosModel;
            if (model == null) return;
            _viewModel = (AbaDocumentosOriginariosModel) DataContext;
        }
    }
}