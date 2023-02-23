using System.Windows;
using Fusion.FastReport.Relatorios.Sistema.Caixa;

namespace FusionWPF.SharedViews.ControleCaixa
{
    public partial class ResultadoDoCaixaView
    {
        public ResultadoDoCaixaView(ResultadoDoCaixaContexto contexto)
        {
            InitializeComponent();
            Contexto = contexto;
        }

        public ResultadoDoCaixaContexto Contexto { get; }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = Contexto;
            Contexto.CarregarResultado();
        }

        private void FiltrarResultadoClickHandler(object sender, RoutedEventArgs e)
        {
            Contexto.CarregarResultado();
        }

        private void ImprimirResultadoClickHandler(object sender, RoutedEventArgs e)
        {
            using (var r = new RListagemDeCaixasFechados(Contexto.SessaoManager))
            {
                r.InformarPeriodo(Contexto.DataInicio, Contexto.DataFinal);
                r.Visualizar();
            }
        }
    }
}