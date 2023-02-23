using System.Windows;
using System.Windows.Input;
using Fusion.Controladores.Menu;

namespace Fusion.Visao.Tributacoes.Regras
{
    public partial class GerenciarRegrasListagem
    {
        private readonly RegraTributacaoSaidaController _controller;
        private readonly GerenciarRegrasListagemContexto _contexto;

        public GerenciarRegrasListagem(RegraTributacaoSaidaController controller, GerenciarRegrasListagemContexto contexto)
        {
            _controller = controller;
            _contexto = contexto;

            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _contexto.Inicializar();
            DataContext = _contexto;
        }

        private void NovaRegraClickHandler(object sender, RoutedEventArgs e)
        {
            _controller.NovaRegra();
        }

        private void DataGridRowDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            _controller.EditaRegra(_contexto.RegraSelecionada);
        }
    }
}
