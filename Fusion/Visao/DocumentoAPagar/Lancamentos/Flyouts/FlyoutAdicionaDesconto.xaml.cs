using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.DocumentoAPagar.Lancamentos.Flyouts
{
    public partial class FlyoutAdicionaDesconto
    {
        public FlyoutAdicionaDesconto()
        {
            InitializeComponent();
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            try
            {
                (DataContext as FlyoutAdicionaDescontoModel)?.Salvar();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }
    }
}
