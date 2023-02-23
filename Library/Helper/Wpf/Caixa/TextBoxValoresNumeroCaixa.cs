using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace FusionLibrary.Helper.Wpf.Caixa
{
    public static class TextBoxValoresNumeroCaixa
    {
        public static readonly DependencyProperty ValidarProperty =
             DependencyProperty.RegisterAttached("Validar", typeof(bool), typeof(TextBoxAcceptHelper),
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
            var textBox = sender as TextBox;

            var regex = new Regex(@"^[0-9]+,?([0-9]+)?$");

            var textoPrevisto = textBox?.Text + e.Text;

            e.Handled = !regex.IsMatch(textoPrevisto);
        }

        public static void SetValidar(DependencyObject obj, bool value)
        {
            obj.SetValue(ValidarProperty, value);
        }

        public static bool GetValidar(DependencyObject obj)
        {
            return (bool)obj.GetValue(ValidarProperty);
        }
    }
}