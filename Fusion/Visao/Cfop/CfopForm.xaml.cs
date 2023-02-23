using System.Windows;

namespace Fusion.Visao.Cfop
{
    public partial class CfopForm
    {
        private CfopFormModel ViewModel => DataContext as CfopFormModel;

        public CfopForm(CfopFormModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.Inicializar();
        }
    }
}