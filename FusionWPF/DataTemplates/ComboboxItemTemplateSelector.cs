using System.Windows;
using System.Windows.Controls;
using FusionWPF.Extensions;

namespace FusionWPF.DataTemplates
{
    public class ComboBoxItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SelectedTemplate { get; set; }
        public DataTemplate ItemTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var comboBoxItem = container.GetVisualParent<ComboBoxItem>();

            return comboBoxItem == null 
                ? SelectedTemplate 
                : ItemTemplate;
        }
    }
}