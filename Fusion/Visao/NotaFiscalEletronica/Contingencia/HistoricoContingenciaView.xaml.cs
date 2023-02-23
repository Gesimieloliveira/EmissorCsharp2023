using System.Windows;

namespace Fusion.Visao.NotaFiscalEletronica.Contingencia
{
    public partial class HistoricoContingenciaView
    {
        private HistoricoContingenciaViewModel GetView => (HistoricoContingenciaViewModel) DataContext;

        public HistoricoContingenciaView()
        {
            DataContext = new HistoricoContingenciaViewModel();
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            GetView.Inicializar();
        }

        private void FinalizarContigenciaHandler(object sender, RoutedEventArgs e)
        {
            GetView.FinalizarContigenciaSelecionada();
        }

        private void IniciarContingenciaHandler(object sender, RoutedEventArgs e)
        {
            GetView.AbrirFlyoutIniciarContingencia();
        }
    }
}