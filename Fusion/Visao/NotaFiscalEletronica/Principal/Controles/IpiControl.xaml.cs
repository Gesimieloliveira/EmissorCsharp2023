using System.Windows;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Controles
{
    public partial class IpiControl
    {
        public IpiControl()
        {
            InitializeComponent();
            Contexto = new IpiContexto();
        }

        public IpiContexto Contexto { get; }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            Contexto.Inicializar();

            DataContext = Contexto;
        }
    }
}