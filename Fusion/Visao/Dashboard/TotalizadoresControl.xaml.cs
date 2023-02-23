using System.Windows;
using System.Windows.Controls;

namespace Fusion.Visao.Dashboard
{
    public partial class TotalizadoresControl : UserControl
    {
        private TotalizadoresContexto Contexto => DataContext as TotalizadoresContexto;

        public TotalizadoresControl()
        {
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            Contexto?.Refresh();
        }
    }
}
