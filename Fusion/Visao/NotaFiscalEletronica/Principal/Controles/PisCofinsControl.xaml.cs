using System.Windows;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Controles
{
    public partial class PisCofinsControl
    {
        public PisCofinsContexto Contexto { get; }

        public PisCofinsControl()
        {
            Contexto = new PisCofinsContexto();
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            Contexto.Inicializar();

            DataContext = Contexto;
        }
    }
}