using System.Windows;
using Fusion.Visao.CteEletronicoOs.Emitir.Aba.Model;

namespace Fusion.Visao.CteEletronicoOs.Emitir.Aba
{
    public partial class AbaServicoSeguroRodoviario
    {
        private AbaServicoSeguroRodoOsModel ViewModel => DataContext as AbaServicoSeguroRodoOsModel;

        public AbaServicoSeguroRodoviario()
        {
            InitializeComponent();
        }

        private void ClickRemovePercurso(object sender, RoutedEventArgs e)
        {
            ViewModel.ExcluirPercurso();
        }

        private void DeletaComponenteSelecionado(object sender, RoutedEventArgs e)
        {
            ViewModel.ExcluirComponente();
        }

        private void DeletaDocumentoReferenciadoSelecionado(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel.ExcluirDocumentoReferenciado();
        }
    }
}
