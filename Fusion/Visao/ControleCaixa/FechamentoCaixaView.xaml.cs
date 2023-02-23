using System;
using System.Windows;
using System.Windows.Controls;
using Fusion.Sessao;
using FusionCore.ControleCaixa;
using FusionCore.Sessao;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.ControleCaixa
{
    public partial class GerenciamentoCaixaView
    {
        public GerenciamentoCaixaView()
        {
            InitializeComponent();
            Contexto = new FechamentoCaixaContexto();
        }

        public FechamentoCaixaContexto Contexto { get; }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            Contexto.CarregarCaixas();
            DataContext = Contexto;
        }

        private void CalcularCaixaClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                var caixa = ((Button) sender).Tag as CaixaIndividual;

                var dialog = new CalculoCaixaChild(caixa);

                this.ShowChildWindowAsync(dialog);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}