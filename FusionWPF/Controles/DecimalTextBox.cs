using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FusionLibrary.Wpf.Tools;

// ReSharper disable RedundantTypeSpecificationInDefaultExpression

namespace FusionWPF.Controles
{
    public class DecimalTextBox : TextBox
    {
        private bool TemSeparador => Text.Contains(",");

        public static readonly DependencyProperty LimiteCasasDecimalProperty = DependencyProperty.Register(nameof(LimiteDecimal),
            typeof(int),
            typeof(DecimalTextBox),
            new PropertyMetadata(default(int)));

        public static readonly DependencyProperty MascaraAoDigitarProperty = DependencyProperty.Register(nameof(MascaraAoDigitar),
            typeof(bool),
            typeof(DecimalTextBox),
            new PropertyMetadata(false));

        public int LimiteDecimal
        {
            get => (int)GetValue(LimiteCasasDecimalProperty);
            set => SetValue(LimiteCasasDecimalProperty, value);
        }

        public bool MascaraAoDigitar
        {
            get => (bool)GetValue(MascaraAoDigitarProperty);
            set => SetValue(MascaraAoDigitarProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            FocusAdvancement.SetAdvancesByEnterKey(this, true);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }

            base.OnPreviewKeyDown(e);
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (e.Text == "," && TemSeparador)
            {
                e.Handled = true;
                return;
            }

            if (!MascaraAoDigitar && AtingiuLimiteDecimals())
            {
                e.Handled = true;
                return;
            }

            e.Handled = MascaraAoDigitar
                ? !Regex.IsMatch(e.Text, @"[0-9,\.]", RegexOptions.IgnoreCase)
                : !Regex.IsMatch(e.Text, @"[0-9,]", RegexOptions.IgnoreCase);

            base.OnPreviewTextInput(e);
        }

        private bool AtingiuLimiteDecimals()
        {
            if (LimiteDecimal == default(int) || TemSeparador == false)
            {
                return false;
            }

            var textSplit = Text.Split(',');
            var posicaoSeparador = Text.IndexOf(',');

            return CaretIndex > posicaoSeparador && textSplit[1].Length >= LimiteDecimal;
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (!MascaraAoDigitar)
            {
                base.OnTextChanged(e);
                return;
            }

            var maskedText = AddMask(Text);

            if (maskedText != Text)
            {
                Text = maskedText;
                CaretIndex = maskedText.Length;
            }

            base.OnTextChanged(e);
        }

        private string AddMask(string input)
        {
            var hasSignal = input.StartsWith("-");
            var value = $"0{input ?? ""}".Replace(".", "").Replace(",", ".");

            if (!long.TryParse(Regex.Replace(value, @"\D", ""), out var longValue))
            {
                return AddMask("0");
            }

            var newValue = longValue.ToString();
            if (newValue.Length < LimiteDecimal + 1)
            {
                newValue = newValue.PadLeft(LimiteDecimal + 1, '0');
            }

            newValue = Regex.Replace(newValue, @"(\d)(\d{" + LimiteDecimal + "})$", "$1,$2");
            newValue = Regex.Replace(newValue, @"[0-9](?=(?:[0-9]{3})+(?![0-9]))", "$&.");

            return hasSignal ? $"-{newValue}" : newValue;
        }
    }
}