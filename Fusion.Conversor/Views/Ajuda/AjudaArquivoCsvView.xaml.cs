using System.Windows;

namespace Fusion.Conversor.Views.Ajuda
{
    public partial class AjudaArquivoCsvView
    {
        private readonly AjudaArquivoCsvContexto _contexto;

        public AjudaArquivoCsvView(AjudaArquivoCsvContexto contexto)
        {
            _contexto = contexto;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = _contexto;

            _contexto.Inicializar();
        }

        private void EntendiClickHandler(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
