using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;
using FusionLibrary.Wpf.Tools.Base;

namespace FusionLibrary.Wpf.Tools
{
    public class RibbonButtonHelper : RibbonHelperBase
    {
        public static readonly DependencyProperty LargeImageSourceProperty
             = DependencyProperty.RegisterAttached(
                 "LargeImageSource",
                 typeof(string),
                 typeof(RibbonButtonHelper),
                 new FrameworkPropertyMetadata(default(string),
                     FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                     LargeImageSourceChangedCallback));

        private static void LargeImageSourceChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            var ribbonMenuItem = d as RibbonButton;
            if (ribbonMenuItem == null)
                return;

            ribbonMenuItem.LargeImageSource = CreateGlyph((string)e.NewValue, new FontFamily(@"/FontAwesome.WPF;component/#FontAwesome"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal, (SolidColorBrush)Application.Current.Resources["MenuIconBrush"]);
        }

        public static void SetLargeImageSource(DependencyObject element, string value)
        {
            element.SetValue(LargeImageSourceProperty, value);
        }

        public static string GetLargeImageSource(DependencyObject element)
        {
            return (string)element.GetValue(LargeImageSourceProperty);
        }

        public static readonly DependencyProperty LargeImageColorProperty
             = DependencyProperty.RegisterAttached(
                 "LargeImageColor",
                 typeof(Color),
                 typeof(RibbonHelperBase),
                 new FrameworkPropertyMetadata(default(Color),
                     FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                     LargeImageColorChangedCallback));

        private static void LargeImageColorChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menuItem = d as RibbonButton;
            if (menuItem == null)
                return;

            var drawingImage = menuItem.LargeImageSource as DrawingImage;
            var glyphRunDrawing = drawingImage?.Drawing as GlyphRunDrawing;

            if (glyphRunDrawing == null) return;

            glyphRunDrawing.ForegroundBrush = new SolidColorBrush((Color)e.NewValue);
            menuItem.LargeImageSource = new DrawingImage(glyphRunDrawing);
        }

        public static void SetLargeImageColor(DependencyObject element, Color value)
        {
            element.SetValue(LargeImageColorProperty, value);
        }

        public static Color GetLargeImageColor(DependencyObject element) => (Color)element.GetValue(LargeImageColorProperty);
    }
}