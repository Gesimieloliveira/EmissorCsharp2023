using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace FusionLibrary.Helper.Wpf
{
    public class TextBoxTrim
    {
        public static readonly DependencyProperty TrimProperty =
            DependencyProperty.RegisterAttached("Trim", typeof(bool), typeof(TextBoxTrim),
                new FrameworkPropertyMetadata(false, TrimChanged));

        private static void TrimChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TextBoxBase textBox))
            {
                throw new InvalidOperationException("TextBoxTrim deve ser utilizado apenas em TextBox");
            }

            if ((bool)e.NewValue)
            {
                textBox.TextChanged += ChangedTextboxInputTrim;
                textBox.LostFocus += LostTextBoxTrim;
            }
            else
            {
                textBox.TextChanged -= ChangedTextboxInputTrim;
                textBox.LostFocus -= LostTextBoxTrim;
            }
        }

        private static void LostTextBoxTrim(object sender, RoutedEventArgs e)
        {
            if (!(sender is TextBox textBox))
            {
                return;
            }

            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = string.Empty;
            }

            textBox.Text = textBox.Text?.Trim();
        }

        private static void ChangedTextboxInputTrim(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox textBox))
            {
                return;
            }

            textBox.Text = textBox.Text.TrimStart();
        }

        public static bool GetTrim(DependencyObject obj)
        {
            return (bool)obj.GetValue(TrimProperty);
        }

        public static void SetTrim(DependencyObject obj, bool value)
        {
            obj.SetValue(TrimProperty, value);
        }
    }
}