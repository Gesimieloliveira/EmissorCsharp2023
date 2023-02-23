using System.Windows;
using System.Windows.Controls;

namespace FusionWPF.Controles
{
    public class FatorConversao : DecimalTextBox
    {
        public static readonly DependencyProperty SiglaProperty = DependencyProperty.Register("Sigla",
            typeof(string),
            typeof(FatorConversao),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ConversaoProperty = DependencyProperty.Register("Conversao",
            typeof(decimal),
            typeof(FatorConversao),
            new PropertyMetadata(default(decimal)));

        private TextBlock PartConversao => GetTemplateChild("PART_Conversao") as TextBlock;
        private TextBlock PartSigla => GetTemplateChild("PART_Sigla") as TextBlock;

        public string Sigla
        {
            get => (string) GetValue(SiglaProperty);
            set => SetValue(SiglaProperty, value);
        }

        public decimal Conversao
        {
            get => (decimal) GetValue(ConversaoProperty);
            set => SetValue(ConversaoProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var conversaoBinding = GetBindingExpression(ConversaoProperty);
            var siglaBinding = GetBindingExpression(SiglaProperty);

            if (conversaoBinding != null)
            {
                PartConversao?.SetBinding(TextBlock.TextProperty, conversaoBinding.ParentBindingBase);
            }

            if (siglaBinding != null)
            {
                PartSigla?.SetBinding(TextBlock.TextProperty, siglaBinding.ParentBindingBase);
            }
        }
    }
}