using System;
using System.Windows;
using System.Windows.Input;

namespace FusionLibrary.Helper.Wpf
{
    public class DataGridMover
    {
        public static readonly DependencyProperty MoverEnterProperty =
            DependencyProperty.RegisterAttached("MoverEnter", typeof(bool), typeof(DataGridMover),
                new FrameworkPropertyMetadata(false, MoverEnterChanged));

        public static bool GetMoverEnter(DependencyObject obj)
        {
            return (bool)obj.GetValue(MoverEnterProperty);
        }

        public static void SetMoverEnter(DependencyObject obj, bool value)
        {
            obj.SetValue(MoverEnterProperty, value);
        }


        private static void MoverEnterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var UIElement = d as UIElement;

            if (UIElement == null)
                throw new InvalidOperationException();

            if ((bool)e.NewValue)
            {
                UIElement.PreviewKeyDown += OnPreviewKeyDown;
            }
            else
            {
                UIElement.PreviewKeyDown -= OnPreviewKeyDown;
            }
        }

        private static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var uiElement = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter && uiElement != null)
            {
                e.Handled = true;
                uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }
    }
}