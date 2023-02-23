using System;
using System.Windows;
using System.Windows.Controls;

namespace FusionWPF.Helpers
{
    public class DatePickerHelper
    {
        public static readonly DependencyProperty FocusOnCalendarCloseProperty = DependencyProperty.RegisterAttached("FocusOnCalendarClose", typeof(bool), typeof(DatePickerHelper), new FrameworkPropertyMetadata(false, FocusOnCalendarCloseChanged));

        public static bool GetFocusOnCalendarClose(DependencyObject obj)
        {
            return (bool) obj.GetValue(FocusOnCalendarCloseProperty);
        }

        public static void SetFocusOnCalendarClose(DependencyObject obj, bool value)
        {
            obj.SetValue(FocusOnCalendarCloseProperty, value);
        }

        private static void FocusOnCalendarCloseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DatePicker dp))
            {
                throw new Exception("Não foi possível acessar o evento CalendarClose do objeto DatePicker");
            }

            dp.CalendarClosed -= DatePickerCalendarClosedHandler;

            if ((bool) e.NewValue)
            {
                dp.CalendarClosed += DatePickerCalendarClosedHandler;
            }
        }

        private static void DatePickerCalendarClosedHandler(object sender, RoutedEventArgs e)
        {
            if (sender is DatePicker dp)
            {
                dp.Focus();
            }
        }
    }
}