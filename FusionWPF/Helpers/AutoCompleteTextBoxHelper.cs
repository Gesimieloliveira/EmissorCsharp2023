using System.Windows;
using static System.Windows.FrameworkPropertyMetadataOptions;

namespace FusionWPF.Helpers
{
    public class AutoCompleteTextBoxHelper
    {
        public static readonly DependencyProperty FooterMessageProperty = DependencyProperty.RegisterAttached("FooterMessage", typeof(string), typeof(AutoCompleteTextBoxHelper), new FrameworkPropertyMetadata(null, BindsTwoWayByDefault));
        public static readonly DependencyProperty HasFooterProperty = DependencyProperty.RegisterAttached("HasFooter", typeof(bool), typeof(AutoCompleteTextBoxHelper), new FrameworkPropertyMetadata(false, BindsTwoWayByDefault));

        public static void SetFooterMessage(DependencyObject obj, string value)
        {
            obj.SetValue(FooterMessageProperty, value);
        }

        public static string GetFooterMessage(DependencyObject obj)
        {
            return (string)obj.GetValue(FooterMessageProperty);
        }

        public static void SetHasFooter(DependencyObject obj, bool value)
        {
            obj.SetValue(HasFooterProperty, value);
        }

        public static bool GetHasFooter(DependencyObject obj)
        {
            return (bool)obj.GetValue(HasFooterProperty);
        }
    }
}