using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.TabelasPrecos
{
    public partial class CalculadoraAjusteItem
    {
        public CalculadoraAjusteItem()
        {
            InitializeComponent();
        }

        private void EnviarPorcentagem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                (DataContext as CalculadoraAjusteItemContexto)?.OnHandlerPorcentagemCalculada();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}
