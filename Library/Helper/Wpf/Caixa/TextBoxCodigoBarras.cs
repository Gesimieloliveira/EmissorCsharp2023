using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace FusionLibrary.Helper.Wpf.Caixa
{
    public static class TextBoxCodigoBarras
    {
        public static readonly DependencyProperty CodigoBarrasProperty =
            DependencyProperty.RegisterAttached("CodigoBarras", typeof(bool), typeof(TextBoxAcceptHelper),
                new FrameworkPropertyMetadata(false, CodigoBarrasChanged));

        private static void CodigoBarrasChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBoxBase;
            if (textBox == null)
                throw new InvalidOperationException("AcceptOnlyNumber deve ser utilizado apenas em TextBox");

            if ((bool)e.NewValue) textBox.PreviewTextInput += PreviewTextInputHanlder;
            else textBox.PreviewTextInput -= PreviewTextInputHanlder;
        }

        private static void PreviewTextInputHanlder(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"[^0-9,\-\%\+\*]");
            e.Handled = regex.IsMatch(e.Text);
        }

        public static void SetCodigoBarras(DependencyObject obj, bool value)
        {
            obj.SetValue(CodigoBarrasProperty, value);
        }

        public static bool GetCodigoBarras(DependencyObject obj)
        {
            return (bool)obj.GetValue(CodigoBarrasProperty);
        }
    }
}