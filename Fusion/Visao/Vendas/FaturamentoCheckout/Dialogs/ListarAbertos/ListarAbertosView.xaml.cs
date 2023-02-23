using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FusionCore.Vendas.Faturamentos;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Utils;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.ListarAbertos
{
    public partial class ListarAbertosView
    {
        public ListarAbertosView(ListarAbertosViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        public readonly ListarAbertosViewModel ViewModel;

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
            ViewModel.RefreshData();
            TbFaturamentos.Focus();
        }

        private void PKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Down
                || !ViewModel.PossuiFaturamentos
                || e.OriginalSource?.GetType() == typeof(ListBoxItem))
            {
                return;
            }

            e.Handled = true;
            TbFaturamentos.FocusFirstItem();
        }

        private void PKeyDownItemHandler(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            e.Handled = true;
            AcaoSelecionar();
        }

        private void AcaoSelecionar()
        {
            try
            {
                ViewModel.OnSelecionarItem();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void DoubleClickItemHandler(object sender, MouseButtonEventArgs e)
        {
            AcaoSelecionar();
        }

        private void ClickImprimirHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.ItemSelecionado = (FaturamentoSlim)((Button)sender).Tag;
            ViewModel.OnImprimirSelecionado();
        }
    }
}