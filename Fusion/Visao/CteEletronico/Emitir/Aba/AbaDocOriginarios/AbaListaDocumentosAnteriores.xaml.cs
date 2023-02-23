using System.Windows;
using Fusion.Visao.CteEletronico.Emitir.Aba.Models;

namespace Fusion.Visao.CteEletronico.Emitir.Aba.AbaDocOriginarios
{
    public partial class AbaListaDocumentosAnteriores 
    {
        private AbaDocumentosOriginariosModel _viewModel;

        public AbaListaDocumentosAnteriores()
        {
            InitializeComponent();
        }

        private void ListaDocumentoAnterior_OnLoaded(object sender, RoutedEventArgs e)
        {
            var model = DataContext as AbaDocumentosOriginariosModel;

            if (model == null) return;

            _viewModel = (AbaDocumentosOriginariosModel)DataContext;
        }

        private void OnClickDeletaItem(object sender, RoutedEventArgs e)
        {
            _viewModel.DeletaDocumentoAnteriorSelecionado();
        }
    }
}
