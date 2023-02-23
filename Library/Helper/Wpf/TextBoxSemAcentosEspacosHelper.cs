using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace FusionLibrary.Helper.Wpf
{
    public static class TextBoxSemAcentosEspacosHelper
    {
        public static readonly DependencyProperty AceitarSomenteSemAcentos =
            DependencyProperty.RegisterAttached("TextBoxSemAcentosEspacosHelper", typeof(bool), typeof(TextBoxSemAcentosEspacosHelper),
                new FrameworkPropertyMetadata(false, AceitarSemAcentosEspacosChanged));

        private static void AceitarSemAcentosEspacosChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBoxBase;
            if (textBox == null)
                throw new InvalidOperationException("TextBoxSemAcentosEspacosHelper deve ser utilizado apenas em TextBox");

            if ((bool)e.NewValue) textBox.PreviewTextInput += PreviewTextInputHanlder;
            else textBox.PreviewTextInput -= PreviewTextInputHanlder;
        }

        private static void PreviewTextInputHanlder(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"[^a-zA-Z]", RegexOptions.IgnoreCase);
            e.Handled = regex.IsMatch(e.Text);
        }

        public static void SetAceitarSomenteSemAcentos(DependencyObject obj, bool value)
        {
            obj.SetValue(AceitarSomenteSemAcentos, value);
        }

        public static bool GetAceitarSomenteSemAcentos(DependencyObject obj)
        {
            return (bool)obj.GetValue(AceitarSomenteSemAcentos);
        }
    }
}