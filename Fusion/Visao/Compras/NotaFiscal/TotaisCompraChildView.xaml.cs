using System.Windows;

namespace Fusion.Visao.Compras.NotaFiscal
{
    public partial class TotaisCompraChildView
    {
        private TotaisCompraChildViewModel GetModel => DataContext as TotaisCompraChildViewModel;

        public TotaisCompraChildView(TotaisCompraChildViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            TbFrete.Focus();
            GetModel.Inicializar();
        }

        private void ClickConfimrar(object sender, RoutedEventArgs e)
        {
            GetModel.ConfirmaAlteracoes();
        }
    }
}