using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;

namespace FusionWPF.Helpers
{
    public class FlyoutHelper
    {
        public static readonly DependencyProperty CloseOnEscapeProperty = DependencyProperty.RegisterAttached("CloseOnEscape", typeof(bool), typeof(FlyoutHelper), new FrameworkPropertyMetadata(false, CloseOnEscapeChanged));

        [AttachedPropertyBrowsableForType(typeof(Flyout))]
        public static void SetCloseOnEscape(DependencyObject obj, bool value)
        {
            obj.SetValue(CloseOnEscapeProperty, value);
        }

        [AttachedPropertyBrowsableForType(typeof(Flyout))]
        public static bool GetCloseOnEscape(DependencyObject obj)
        {
            return (bool) obj.GetValue(CloseOnEscapeProperty);
        }

        private static void CloseOnEscapeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Flyout element))
            {
                return;
            }

            void CloseByEscOnKeyDown(object sender, KeyEventArgs args)
            {
                if (args.Key == Key.Escape)
                {
                    element.IsOpen = false;   
                }
            }

            element.KeyDown -= CloseByEscOnKeyDown;

            if (true.Equals(e.NewValue))
            {
                element.KeyDown += CloseByEscOnKeyDown;
            }
        }
    }
}