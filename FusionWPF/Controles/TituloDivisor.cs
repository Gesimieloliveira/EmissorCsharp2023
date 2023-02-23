using System.Windows;
using System.Windows.Controls;

namespace FusionWPF.Controles
{
    public class TituloDivisor : Control
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TituloDivisor), new FrameworkPropertyMetadata(default(string)));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
    }
}