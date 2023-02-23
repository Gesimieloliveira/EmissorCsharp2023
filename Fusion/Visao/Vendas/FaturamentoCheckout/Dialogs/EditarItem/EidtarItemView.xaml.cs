using System;
using System.Windows;
using System.Windows.Input;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Helpers;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.EditarItem
{
    public partial class EidtarItemView
    {
        public EidtarItemView(EditarItemViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;

            AtalhoBinder.Iniciar(this)
                .BindBotao(Key.F2, BtnConfirmar);
        }

        public readonly EditarItemViewModel ViewModel;

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.Inicializar();
            DataContext = ViewModel;
            TbValorUnitario.Focus();
        }

        private void OnAplicarEdicao(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.AplicarAlteracoes();
                Close(true);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}