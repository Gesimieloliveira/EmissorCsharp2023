using System;
using System.Windows;
using System.Windows.Input;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Helpers;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.TabelaPrecos
{
    public partial class TabelaPrecosView
    {
        public TabelaPrecosView(TabelaPrecosViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;

            AtalhoBinder.Iniciar(this)
                .BindBotao(Key.F2, BtnConfirmar);
        }

        public readonly TabelaPrecosViewModel ViewModel;

        private void TabelaPrecosView_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
            CbTablaPrecos.Focus();
        }

        private void BtnConfirmar_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.ConfirmarEscolha();
                Close(true);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}