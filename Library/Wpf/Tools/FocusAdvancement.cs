using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;

namespace FusionLibrary.Wpf.Tools
{
    public class FocusAdvancement
    {
        public static readonly DependencyProperty NextElementProperty =
            DependencyProperty.RegisterAttached("NextElement", typeof(UIElement), typeof(FocusAdvancement));

        public static readonly DependencyProperty DirectionProperty = DependencyProperty.RegisterAttached("Direction",
            typeof(FocusNavigationDirection),
            typeof(FocusAdvancement),
            new FrameworkPropertyMetadata(FocusNavigationDirection.Next));

        public static readonly DependencyProperty AdvancesByEnterKeyProperty =
            DependencyProperty.RegisterAttached("AdvancesByEnterKey",
                typeof(bool),
                typeof(FocusAdvancement),
                new FrameworkPropertyMetadata(false, AdvancesByEnterKeCnagedHandler));

        public static bool GetAdvancesByEnterKey(DependencyObject obj)
        {
            return (bool) obj.GetValue(AdvancesByEnterKeyProperty);
        }

        public static void SetAdvancesByEnterKey(DependencyObject obj, bool value)
        {
            obj.SetValue(AdvancesByEnterKeyProperty, value);
        }

        public static UIElement GetNextElement(DependencyObject obj)
        {
            return (UIElement) obj.GetValue(NextElementProperty);
        }

        public static void SetNextElement(DependencyObject obj, UIElement value)
        {
            obj.SetValue(NextElementProperty, value);
        }

        public static FocusNavigationDirection GetDirection(DependencyObject obj)
        {
            return (FocusNavigationDirection) obj.GetValue(DirectionProperty);
        }

        public static void SetDirection(DependencyObject obj, FocusNavigationDirection direction)
        {
            obj.SetValue(DirectionProperty, direction);
        }

        private static void AdvancesByEnterKeCnagedHandler(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        ) {

            if (!(d is UIElement element))
            {
                return;
            }

            element.KeyDown -= MoveFocusHandler;
            element.PreviewKeyDown -= MoveFocusHandler;

            if ((bool) e.NewValue != true)
            {
                return;
            }

            var isDatePicker = d is DatePicker || d is DateTimePicker;

            if (isDatePicker)
            {
                element.PreviewKeyDown += MoveFocusHandler;
                return;
            }

            element.KeyDown += MoveFocusHandler;
        }

        private static void MoveFocusHandler(object sender, KeyEventArgs e)
        {
            if (!e.Key.Equals(Key.Enter) && !e.Key.Equals(Key.Tab))
            {
                return;
            }

            if (e.KeyboardDevice.Modifiers != ModifierKeys.None)
            {
                return;
            }

            var element = sender as UIElement;
            var focusedElement = Keyboard.FocusedElement as UIElement;
            var nextElement = GetNextElement(element);

            if (!GetAdvancesByEnterKey(element))
            {
                return;
            }

            MoveFocus(focusedElement, nextElement);
            e.Handled = true;
        }

        private static void MoveFocus(UIElement element, IInputElement nextElement = null)
        {
            if (nextElement != null)
            {
                nextElement.Focus();
                return;
            }

            element.MoveFocus(new TraversalRequest(GetDirection(element)));
        }
    }
}