using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.DocumentoAPagar.Lancamentos.Flyouts
{
    public partial class FlyoutAdicionaJuro
    {
        public FlyoutAdicionaJuro()
        {
            InitializeComponent();
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            try
            {
                (DataContext as FlyoutAdicionaJuroModel)?.Salvar();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }
    }
}
