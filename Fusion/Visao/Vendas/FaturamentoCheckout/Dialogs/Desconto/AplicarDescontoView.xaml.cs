using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Desconto
{
    public partial class AplicarDescontoView
    {
        public AplicarDescontoView(AplciarDescontoViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }

        private AplciarDescontoViewModel ViewModel { get; }

        private void AplicarDescontoView_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
        }

        private void AplicarDescontoClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.AplicarDesconto();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}