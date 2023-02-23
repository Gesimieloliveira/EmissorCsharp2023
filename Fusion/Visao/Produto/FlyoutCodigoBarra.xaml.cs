using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Produto
{
    public partial class FlyoutCodigoBarra
    {
        public FlyoutCodigoBarra()
        {
            InitializeComponent();
        }

        private FlyoutCodigoBarraModel Contexto => DataContext as FlyoutCodigoBarraModel;

        private void ClickSalvarCodigoBarraHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                Contexto?.SalvarAlteracao();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void IsOpenChangedHandler(object sender, RoutedEventArgs e)
        {
            var flyout = sender as FlyoutCodigoBarraModel;

            if (flyout?.IsOpen == true)
            {
                TextBoxAlias.Focus();
            }
        }
    }
}
