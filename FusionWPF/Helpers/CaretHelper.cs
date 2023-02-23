using System.Windows.Controls;

namespace FusionWPF.Helpers
{
    public static class CaretHelper
    {
        public static void MoveCaretToEnd(this TextBox textBox)
        {
            var textLeng = textBox.Text?.Length ?? 0;

            if (textLeng == 0)
            {
                return;
            }

            textBox.CaretIndex = textLeng;
        }
    }
}