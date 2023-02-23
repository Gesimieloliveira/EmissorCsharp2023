using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;
using FusionLibrary.Wpf.Tools.Base;

namespace FusionLibrary.Wpf.Tools
{
    public class RibbonMenuItemHelper : RibbonHelperBase
    {
        public static readonly DependencyProperty ImageImageSourceProperty
             = DependencyProperty.RegisterAttached(
                 "ImageImageSource",
                 typeof(string),
                 typeof(RibbonMenuItemHelper),
                 new FrameworkPropertyMetadata(default(string),
                     FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                     ImageFontChangedCallback));

        private static void ImageFontChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            var ribbonMenuItem = d as RibbonMenuItem;
            if (ribbonMenuItem == null)
                return;

            ribbonMenuItem.ImageSource = CreateGlyph((string)e.NewValue, new FontFamily(@"/FontAwesome.WPF;component/#FontAwesome"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal, (SolidColorBrush)Application.Current.Resources["MenuIconBrush"]);
        }

        public static void SetImageImageSource(DependencyObject element, string value)
        {
            element.SetValue(ImageImageSourceProperty, value);
        }

        public static string GetImageImageSource(DependencyObject element)
        {
            return (string)element.GetValue(ImageImageSourceProperty);
        }

        public static readonly DependencyProperty ImageFontColorProperty
             = DependencyProperty.RegisterAttached(
                 "ImageFontColor",
                 typeof(Color),
                 typeof(RibbonMenuItemHelper),
                 new FrameworkPropertyMetadata(default(Color),
                     FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                     ImageFontColorChangedCallback));

        private static void ImageFontColorChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var menuItem = d as RibbonMenuItem;
            if (menuItem == null)
                return;

            var drawingImage = menuItem.ImageSource as DrawingImage;
            var glyphRunDrawing = drawingImage?.Drawing as GlyphRunDrawing;

            if (glyphRunDrawing == null) return;

            glyphRunDrawing.ForegroundBrush = new SolidColorBrush((Color)e.NewValue);
            menuItem.ImageSource = new DrawingImage(glyphRunDrawing);
        }

        public static void SetImageFontColor(DependencyObject element, Color value)
        {
            element.SetValue(ImageImageSourceProperty, value);
        }

        public static Color GetImageFontColor(DependencyObject element) => (Color)element.GetValue(ImageImageSourceProperty);
    }
}
