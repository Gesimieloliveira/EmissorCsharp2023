using System;
using System.Windows;
using System.Windows.Input;
using FusionCore.Helpers.Binding;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.SolictaTotal
{
    public partial class SolicitaTotalView
    {
        protected SolicitaTotalView(SolicitaTotalViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            ViewModel = viewModel;
        }

        public readonly SolicitaTotalViewModel ViewModel;

        public static SolicitaTotalView Criar()
        {
            return new SolicitaTotalView(new SolicitaTotalViewModel());
        }

        private void AcaoConfirmar()
        {
            try
            {
                ViewModel.AplicarTotal();
                Close(true);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void ConfirmarClickHandler(object sender, RoutedEventArgs e)
        {
            AcaoConfirmar();
        }

        private void TotalKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            e.Handled = true;
            TbValorTotal.UpdateBindingText();
            AcaoConfirmar();
        }
    }
}