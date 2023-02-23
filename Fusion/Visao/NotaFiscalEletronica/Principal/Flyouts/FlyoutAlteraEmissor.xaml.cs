using System;
using System.Windows;
using Fusion.Visao.NotaFiscalEletronica.Principal.Flyouts.Models;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Flyouts
{
    public partial class FlyoutAlteraEmissor
    {
        private FlyoutAlteraEmissorModel GetModel => DataContext as FlyoutAlteraEmissorModel;

        public FlyoutAlteraEmissor()
        {
            InitializeComponent();
        }

        private void ClickSalvarHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                GetModel.SalvarAlteracao();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}