using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace FusionLibrary.Helper.Wpf
{
    public static class TextBoxAcceptHelper
    {
        public static readonly DependencyProperty AcceptOnlyNumbersProperty =
            DependencyProperty.RegisterAttached("AcceptOnlyNumbers", typeof (bool), typeof (TextBoxAcceptHelper),
                new FrameworkPropertyMetadata(false, AcceptOnlyNumbersChanged));

        public static readonly DependencyProperty AcceptDecimalFormatProperty =
            DependencyProperty.RegisterAttached("AcceptDecimalFormat", typeof (bool), typeof (TextBoxAcceptHelper),
                new FrameworkPropertyMetadata(false, AcceptDecimalFormatChanged));

        private static void AcceptOnlyNumbersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBoxBase;
            if (textBox == null)
                throw new InvalidOperationException("AcceptOnlyNumber deve ser utilizado apenas em TextBox");

            if ((bool) e.NewValue) textBox.PreviewTextInput += PreviewTextInputHanlder;
            else textBox.PreviewTextInput -= PreviewTextInputHanlder;
        }

        private static void PreviewTextInputHanlder(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public static void SetAcceptOnlyNumbers(DependencyObject obj, bool value)
        {
            obj.SetValue(AcceptOnlyNumbersProperty, value);
        }

        public static bool GetAcceptOnlyNumbers(DependencyObject obj)
        {
            return (bool) obj.GetValue(AcceptOnlyNumbersProperty);
        }

        private static void AcceptDecimalFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBoxBase;

            if (textBox == null)
                throw new InvalidOperationException("AcceptOnlyDecimal deve ser utilizado apenas em TextBox");

            if ((bool) e.NewValue) textBox.PreviewTextInput += PreviewInputDecimalHandler;
            else textBox.PreviewTextInput -= PreviewInputDecimalHandler;
        }

        private static void PreviewInputDecimalHandler(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9,]+");
            e.Handled = regex.IsMatch(e.Text) ;
        }

        public static void SetAcceptDecimalFormat(DependencyObject obj, bool value)
        {
            obj.SetValue(AcceptOnlyNumbersProperty, value);
        }

        public static bool GetAcceptDecimalFormat(DependencyObject obj)
        {
            return (bool) obj.GetValue(AcceptOnlyNumbersProperty);
        }
    }
}