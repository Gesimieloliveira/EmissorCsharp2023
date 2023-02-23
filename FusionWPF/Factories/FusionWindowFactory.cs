using System.Windows;
using System.Windows.Controls;
using FusionWPF.Controles;

namespace FusionWPF.Factories
{
    public static class FusionWindowFactory
    {
        public class WSize
        {
            public WSize(double width, double height)
            {
                Width = width;
                Height = height;
            }

            public double Width { get; }
            public double Height { get; }
        }

        public static FusionWindow Criar(string titulo, UserControl control, WSize size = null)
        {
            var view = new FusionWindow
            {
                Title = titulo,
                Content = control,
                SizeToContent = SizeToContent.WidthAndHeight
            };

            if (size != null)
            {
                view.Width = size.Width;
                view.Height = size.Height;
                view.SizeToContent = SizeToContent.Manual;
            }

            return view;
        }
    }
}