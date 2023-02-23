using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using FontAwesome.WPF;

namespace FusionWPF.Controles
{
    public class BotaoIcone : Button
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(FontAwesomeIcon), typeof(BotaoIcone), new FrameworkPropertyMetadata(FontAwesomeIcon.Question));

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(FontAwesomeIcon.Question)]
        public FontAwesomeIcon Icon
        {
            get => (FontAwesomeIcon) GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
    }
}