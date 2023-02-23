using System.Windows;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Controles
{
    public partial class OutrasOpcoesChildWindow
    {
        private readonly IOutrasOpcoesContexto _contexto;

        public OutrasOpcoesChildWindow(IOutrasOpcoesContexto contexto)
        {
            _contexto = contexto;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = _contexto;
        }
    }
}