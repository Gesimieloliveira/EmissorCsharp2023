using System.Windows;
using System.Windows.Input;
using Fusion.Sessao;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;

namespace Fusion.Visao.Usuario
{
    public partial class GerenciarUsuariosControl
    {
        public GerenciarUsuariosControl()
        {
            InitializeComponent();
            Contexto = new GerenciarUsuariosContexto(new SessaoManagerAdm());
        }

        public GerenciarUsuariosContexto Contexto { get; }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = Contexto;
            Contexto.CarregarDados();
        }

        private void NovoUsuarioClickHandler(object sender, RoutedEventArgs e)
        {
            var formModel = new UsuarioFormModel(new UsuarioDTO());
            var view =  new UsuarioForm(formModel);

            view.ShowDialog();
        }

        private void GerenciarPapeisClickHandler(object sender, RoutedEventArgs e)
        {
            var view = new GerenciarPermissao(SessaoSistema.Instancia);

            view.ShowDialog();

            Contexto.CarregarDados();
        }

        private void LinhaUsuarioDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            var usuario = Contexto.CarregarUsuarioSelecionado();

            var contexto = new UsuarioFormModel(usuario);
            var view = new UsuarioForm(contexto);

            view.ShowDialog();

            Contexto.CarregarDados();
        }
    }
}