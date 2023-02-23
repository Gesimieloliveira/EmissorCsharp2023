using System.Windows;
using FusionLibrary.VisaoModel;
using MahApps.Metro.Controls;

namespace Fusion.Factories
{
    internal static class DialogFactory
    {
        public static MetroWindow Criar(UIElement content, ViewModel contexto)
        {
            var window = new MetroWindow
            {
                Style = (Style) Application.Current.FindResource("MetroWindowDialogStyle"),
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Content = content,
                DataContext = contexto
            };

            return window;
        }
    }
}