using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FontAwesome.WPF;

namespace FusionWPF.Controles
{
    public class DataGridButtonColumn : DataGridBoundColumn
    {
        private static Style _defaultElementStyle;
        private static Style _defaultEditingElementStyle;
        private static Style _basedStyle;

        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DataGridButtonColumn));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(FontAwesomeIcon), typeof(DataGridButtonColumn), new FrameworkPropertyMetadata(FontAwesomeIcon.Question));
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(DataGridBoundColumn), new FrameworkPropertyMetadata(default(Brush)));

        static DataGridButtonColumn()
        {
            ElementStyleProperty.OverrideMetadata(typeof(DataGridButtonColumn), new FrameworkPropertyMetadata(DefaultElementStyle));
            EditingElementStyleProperty.OverrideMetadata(typeof(DataGridButtonColumn), new FrameworkPropertyMetadata(DefaultEditElementStyle));
        }

        private static Style DefaultEditElementStyle
        {
            get
            {
                if (_defaultEditingElementStyle == null)
                {
                    var style = new Style(typeof(BotaoIcone), BasedStyle);

                    style.Seal();
                    _defaultEditingElementStyle = style;
                }

                return _defaultEditingElementStyle;
            }
        }

        private static Style BasedStyle
        {
            get
            {
                if (_basedStyle == null)
                {
                    var style = Application.Current.FindResource("BotaoIcone") as Style;

                    style?.Seal();
                    _basedStyle = style;
                }

                return _basedStyle;
            }
        }

        private static Style DefaultElementStyle
        {
            get
            {
                if (_defaultElementStyle == null)
                {
                    var style = new Style(typeof(BotaoIcone), BasedStyle);

                    style.Seal();
                    _defaultElementStyle = style;
                }

                return _defaultElementStyle;
            }
        }

        public FontAwesomeIcon Icon
        {
            get => (FontAwesomeIcon) GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public Brush Background
        {
            get => (Brush) GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        public event RoutedEventHandler Click;

        private FrameworkElement GenerateButtonElement(DataGridCell cell)
        {
            var botaoIcone = cell?.Content as BotaoIcone ?? new BotaoIcone();

            botaoIcone.Style = ElementStyle;
            botaoIcone.Background = Background;
            botaoIcone.Icon = Icon;
            botaoIcone.Click -= BotaoIconeClickHandler;
            botaoIcone.Click += BotaoIconeClickHandler;

            return botaoIcone;
        }

        private void BotaoIconeClickHandler(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, new RoutedEventArgs(ClickEvent, e.Source));
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            return GenerateButtonElement(cell);
        }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            return GenerateButtonElement(cell);
        }
    }
}