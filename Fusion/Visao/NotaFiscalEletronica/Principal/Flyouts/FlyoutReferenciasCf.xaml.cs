using System.Windows;
using System.Windows.Input;
using Fusion.Visao.NotaFiscalEletronica.Principal.Flyouts.Models;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Flyouts
{
    public partial class FlyoutReferenciasCf
    {
        private FlyoutReferenciaCfModel GetModel => (FlyoutReferenciaCfModel) DataContext;

        public FlyoutReferenciasCf()
        {
            InitializeComponent();
        }

        private void IsOpenChangedHandler(object sender, RoutedEventArgs e)
        {
            var flyout = (FlyoutReferenciasCf) sender;
            if (flyout.IsOpen) TbNumeroEcf.Focus();
        }

        private void ClickVincularHandler(object sender, RoutedEventArgs e)
        {
            GetModel.VincularCupomFiscal();
        }

        private void DesvincularCupomHandler(object sender, MouseButtonEventArgs e)
        {
            GetModel.DesvincularCupomSelecionado();
        }
    }
}