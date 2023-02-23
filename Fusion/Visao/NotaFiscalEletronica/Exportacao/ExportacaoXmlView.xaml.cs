using System.Windows;

namespace Fusion.Visao.NotaFiscalEletronica.Exportacao
{
    public partial class ExportacaoXmlView
    {
        private ExportacaoXmlModel ViewModel => DataContext as ExportacaoXmlModel;

        public ExportacaoXmlView()
        {
            DataContext = new ExportacaoXmlModel();
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.Inicializar();
        }
    }
}