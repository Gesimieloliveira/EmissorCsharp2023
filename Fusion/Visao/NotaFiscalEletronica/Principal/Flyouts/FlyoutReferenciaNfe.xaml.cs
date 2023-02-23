using System.Windows;
using System.Windows.Input;
using Fusion.Visao.NotaFiscalEletronica.Principal.Flyouts.Models;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Flyouts
{
    public partial class FlyoutReferenciaNfe
    {
        private FlyoutReferenciaNfeModel _viewModel;

        public FlyoutReferenciaNfe()
        {
            InitializeComponent();
        }

        private void OnIsOpenChangedFlyout(object sender, RoutedEventArgs e)
        {
            _viewModel = DataContext as FlyoutReferenciaNfeModel;
            TextBoxChaveReferenciada.Focus();
        }

        private void OnKeyDownChaveReferenciar(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            _viewModel.RegistraChaveReferenciada();
            TextBoxChaveReferenciada.Focus();
        }

        private void OnClickRemoveReferencia(object sender, RoutedEventArgs e)
        {
            _viewModel.RemoveChaveReferenciada();
            TextBoxChaveReferenciada.Focus();
        }

        private void ClickReferenciarChaveHandler(object sender, RoutedEventArgs e)
        {
            _viewModel.RegistraChaveReferenciada();
            TextBoxChaveReferenciada.Focus();
        }
    }
}