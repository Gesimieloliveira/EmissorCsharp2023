using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace FusionWPF.Helpers
{
    public static class DataGridColumnHelper
    {
        public static readonly DependencyProperty AlignProperty = DependencyProperty.RegisterAttached(
            "Align",
            typeof(HorizontalAlignment),
            typeof(DataGridColumnHelper),
            new FrameworkPropertyMetadata(default(HorizontalAlignment), AlignLeftPropertyChangedCallback));

        public static void SetAlign(DependencyObject obj, HorizontalAlignment value)
        {
            obj.SetValue(AlignProperty, value);
        }

        public static HorizontalAlignment GetAlign(DependencyObject obj)
        {
            return (HorizontalAlignment)obj.GetValue(AlignProperty);
        }

        private static void AlignLeftPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue && d is DataGridTextColumn col)
            {
                var style = new Style(typeof(TextBlock), col.ElementStyle);
                style.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, ConvertToTextAlign((HorizontalAlignment)e.NewValue)));
                style.Seal();

                col.ElementStyle = style;

                var baseOn = Application.Current.FindResource("FusionDataGridColumnHeader") as Style;
                var styleHead = new Style(typeof(DataGridColumnHeader), baseOn);

                styleHead.Setters.Add(new Setter(Control.HorizontalContentAlignmentProperty, e.NewValue));
                styleHead.Seal();

                col.HeaderStyle = styleHead;
            }
        }

        private static TextAlignment ConvertToTextAlign(HorizontalAlignment align)
        {
            switch (align)
            {
                case HorizontalAlignment.Center:
                    return TextAlignment.Center;
                case HorizontalAlignment.Right:
                    return TextAlignment.Right;
                case HorizontalAlignment.Left:
                    return TextAlignment.Left;
                case HorizontalAlignment.Stretch:
                    return TextAlignment.Justify;
                default:
                    return TextAlignment.Right;
            }
        }
    }
}