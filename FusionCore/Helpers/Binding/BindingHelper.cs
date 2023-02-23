using System.Windows.Controls;
using System.Windows.Input;

namespace FusionCore.Helpers.Binding
{
    public static class BindingHelper
    {
        public static void UpdateBindingText(this TextBox textBox)
        {
            textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
        }

        public static void ForceFocusedElementUpdateSource()
        {
            var focusedElement = Keyboard.FocusedElement as TextBox;
            focusedElement?.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
        }
    }
}