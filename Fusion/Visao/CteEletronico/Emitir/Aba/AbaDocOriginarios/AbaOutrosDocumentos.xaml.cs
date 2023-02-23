using System.Windows;
using Fusion.Visao.CteEletronico.Emitir.Aba.Models;

namespace Fusion.Visao.CteEletronico.Emitir.Aba.AbaDocOriginarios
{
    public partial class AbaOutrosDocumentos
    {
        private AbaDocumentosOriginariosModel _viewModel;

        public AbaOutrosDocumentos()
        {
            InitializeComponent();
        }

        private void OnClickDeletaItem(object sender, RoutedEventArgs e)
        {
            _viewModel.DeletaDocumentoOutroDocumentoSelecionado();
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            var model = DataContext as AbaDocumentosOriginariosModel;
            if (model == null) return;
            _viewModel = (AbaDocumentosOriginariosModel) DataContext;
        }
    }
}