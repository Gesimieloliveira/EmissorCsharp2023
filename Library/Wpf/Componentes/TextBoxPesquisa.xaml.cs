using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace FusionLibrary.Wpf.Componentes
{
    public partial class TextBoxPesquisa
    {
        private static readonly RoutedEvent OnSearchEvent =
            EventManager.RegisterRoutedEvent("OnSearch", RoutingStrategy.Bubble,
                typeof (RoutedEventHandler), typeof (TextBoxPesquisa));

        private static readonly RoutedEvent OnKeyDownEvent =
            EventManager.RegisterRoutedEvent("OnKeyDown", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(TextBoxPesquisa));

        [Bindable(true)]
        [Category("Common")]
        [DefaultValue(null)]
        public string Texto
        {
            get { return (string)GetValue(TextoProperty); }
            set { SetValue(TextoProperty, value); }
        }


        public static readonly DependencyProperty TextoProperty =
             DependencyProperty.Register("Texto", typeof(string), typeof(TextBoxPesquisa), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool StartFocus { get; set; }

        public TextBoxPesquisa()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler OnSearch
        {
            add { AddHandler(OnSearchEvent, value); }
            remove { RemoveHandler(OnSearchEvent, value); }
        }

        public event RoutedEventHandler OnKeyDown
        {
            add { AddHandler(OnKeyDownEvent, value);}
            remove { RemoveHandler(OnKeyDownEvent, value);}
        }

        private void OnClickBotaoSearch(object sender, RoutedEventArgs e)
        {
            DispararEventoPesquisa();
        }

        private void OnKeyDownPesquisa(object sender, KeyEventArgs e)
        {

            switch (e.Key)
            {
                case Key.Enter:
                    DispararEventoPesquisa();
                    break;

                case Key.Down:
                    DispararEventoKeyDown();
                    break;
            }
        }

        private void DispararEventoKeyDown()
        {
            RaiseEvent(new RoutedEventArgs(OnKeyDownEvent, this));
        }

        private void DispararEventoPesquisa()
        {
            Texto = Texto?.Trim();

            RaiseEvent(new RoutedEventArgs(OnSearchEvent, this));
        }

        private void TextBoxPesquisa_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (StartFocus)
            {
                FocusManager.SetFocusedElement(this, TextBoxComponentePesquisa);
            }
        }
    }
}