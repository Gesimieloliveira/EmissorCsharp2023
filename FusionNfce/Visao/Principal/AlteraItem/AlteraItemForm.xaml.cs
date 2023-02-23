using System;
using System.Windows;
using System.Windows.Input;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionNfce.Visao.Principal.AlteraItem
{
    public partial class AlteraItemForm
    {
        private AlteraItemFormModel ViewModel => DataContext as AlteraItemFormModel;

        public AlteraItemForm(AlteraItemFormModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.Inicializar();
            TbValorUnitario.Focus();
        }

        private void Fechar_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Aplicar_OnClick(object sender, RoutedEventArgs e)
        {
            AplicarAlteracoes();
        }

        private void AplicarAlteracoes()
        {
            BtnAplicar.Focus();

            try
            {
                ViewModel.AplicarAlteracoes();
                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void AlteraItemForm_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    AplicarAlteracoes();
                    break;
                case Key.Escape:
                    Close();
                    break;
            }
        }
    }
}
