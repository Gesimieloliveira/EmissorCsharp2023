using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Fusion.Sessao;
using FusionCore.Sessao;
using MahApps.Metro.Controls;

namespace Fusion.Controladores.Menu
{
    public abstract class Controlador
    {
        private readonly MetroTabControl _tabControl;
        protected readonly SessaoSistema SessaoSistema;
        protected readonly ISessaoManager SessaoManager;

        protected Controlador(MetroTabControl tabControl)
        {
            _tabControl = tabControl;

            SessaoSistema = SessaoSistema.Instancia;
            SessaoManager = SessaoSistema.SessaoManager;
        }

        protected void AbrirJanelaEmAba(string titulo, UserControl content, object contexto = null)
        {
            var tabAberta = GetTab(titulo);

            if (tabAberta != null)
            {
                tabAberta.Focus();
                return;
            }

            if (contexto != null)
            {
                content.DataContext = contexto;
            }

            var novaTab = new MetroTabItem {Header = titulo, Content = content, CloseButtonEnabled = true};
            _tabControl.Items.Add(novaTab);
            novaTab.Focus();
        }

        protected MetroTabItem GetTab(string titulo)
        {
            return _tabControl.Items
                .Cast<MetroTabItem>()
                .FirstOrDefault(tabItem => (string) tabItem.Header == titulo);
        }

        protected void ShowDialog(Window view)
        {
            view.ShowDialog();
        }
    }
}