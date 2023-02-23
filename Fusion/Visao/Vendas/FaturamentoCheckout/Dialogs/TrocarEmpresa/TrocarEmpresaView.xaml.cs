using System;
using System.Windows;
using System.Windows.Input;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Helpers;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.TrocarEmpresa
{
    public partial class TrocarEmpresaView
    {
        public TrocarEmpresaView(TrocarEmpresaViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;

            AtalhoBinder.Iniciar(this)
                .BindBotao(Key.F2, BtnConfirmar);
        }

        public readonly TrocarEmpresaViewModel ViewModel;

        private void OnLoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
            Keyboard.Focus(CbEmpresa);
        }

        private void TrocarEmpresaClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.SelecionarEmpresa();
                Close(true);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}