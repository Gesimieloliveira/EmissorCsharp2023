using System.Windows;

namespace Fusion.Visao.NotaFiscalEletronica.Contingencia
{
    public partial class IniciarContingenciaFlyout
    {
        public IniciarContigenciaViewModel GetView => DataContext as IniciarContigenciaViewModel;

        public IniciarContingenciaFlyout()
        {
            InitializeComponent();
        }

        private void IniciarContingenciaHandler(object sender, RoutedEventArgs e)
        {
            GetView.IniciarContingencia();
        }
    }
}