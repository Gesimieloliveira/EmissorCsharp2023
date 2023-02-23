using System.Windows;

namespace Fusion.Visao.Dashboard
{
    public partial class DashboardSimples
    {
        private readonly DashboardSimplesModel _contexto;

        public DashboardSimples(DashboardSimplesModel contexto)
        {
            InitializeComponent();
            _contexto = contexto;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (_contexto == null)
            {
                return;
            }

            _contexto?.RefreshAsync();
            _contexto?.IniciaAutoRefresh();

            DataContext = _contexto;
        }
    }
}