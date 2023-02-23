using System.Windows;
using System.Windows.Controls;
using FusionWPF.Base.Contratos;
using MahApps.Metro.SimpleChildWindow;

namespace FusionWPF.Factories
{
    public static class ChildWindowFactory
    {
        public static ChildWindow Cria(UserControl control, IChildContext context = null, string titulo = null)
        {
            var view = new ChildWindow
            {
                Style = FindResource<Style>("ChildWindowStyle"),
                Content = control
            };

            if (context != null)
            {
                view.DataContext = context;
                view.Title = context.TituloChild;
                context.SolicitaFechamento += (s, e) => view.Close();
            }

            view.Title = titulo ?? view.Title;

            return view;
        }

        private static T FindResource<T>(string name)
        {
            return (T) Application.Current.FindResource(name);
        }
    }
}