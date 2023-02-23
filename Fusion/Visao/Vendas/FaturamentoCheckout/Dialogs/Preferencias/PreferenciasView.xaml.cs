using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Preferencias
{
    public partial class PreferenciasView
    {
        public PreferenciasView(PreferenciasViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
        }

        public readonly PreferenciasViewModel ViewModel;

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.Inicializar();
            DataContext = ViewModel;
        }

        private void ClickSalvarHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.SalvaPreferencias();
                DialogBox.MostraInformacao("Preferencias para essa máquina foram salvas");
                Close(true);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}