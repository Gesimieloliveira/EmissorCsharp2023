using System;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;

// ReSharper disable RedundantTypeSpecificationInDefaultExpression

namespace FusionWPF.Dialogos
{
    public static class InputBox
    {
        public static bool ShowInput<T>(string titulo, out T resultado)
        {
            resultado = default(T);

            var dialog = new MetroWindow
            {
                SizeToContent = SizeToContent.Height,
                Width = 400,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Padding = new Thickness(10)
            };

            var control = new StackPanel();
            var textBlock = new TextBlock {Text = titulo};
            var textBox = new TextBox();

            var button = new Button
            {
                Margin = new Thickness(0, 10, 0, 0),
                Content = "Continuar",
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = 135
            };

            button.Click += (sender, args) =>
            {
                dialog.DialogResult = true;
            };

            control.Children.Add(textBlock);
            control.Children.Add(textBox);
            control.Children.Add(button);

            dialog.Content = control;

            if (dialog.ShowDialog() == false)
            {
                return false;
            }

            try
            {
                var text = textBox.Text as object;
                var output = Convert.ChangeType(text, typeof(T));

                resultado = (T) output;

                return true;
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"{titulo}: Valor inválido para tipo esperado");
            }
        }
    }
}