using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using JetBrains.Annotations;

namespace FusionLibrary.Wpf.Componentes
{
    public sealed partial class ToolbarFormulario : INotifyPropertyChanged
    {
        private static readonly RoutedEvent OnSaveEvent =
            EventManager.RegisterRoutedEvent("OnSave", RoutingStrategy.Bubble,
                typeof (RoutedEventHandler), typeof (ToolbarFormulario));

        private static readonly RoutedEvent OnDeleteEvent =
            EventManager.RegisterRoutedEvent("OnDelete", RoutingStrategy.Bubble,
                typeof (RoutedEventHandler), typeof (ToolbarFormulario));

        private static readonly RoutedEvent OnCloseEvent =
            EventManager.RegisterRoutedEvent("OnClose", RoutingStrategy.Bubble,
                typeof (RoutedEventHandler), typeof (ToolbarFormulario));

        private bool _showClose = true;
        private bool _showDelete = true;
        private bool _showSave = true;

        public ToolbarFormulario()
        {
            InitializeComponent();
        }

        public bool ShowSave
        {
            get { return _showSave; }
            set
            {
                _showSave = value;
                NotifyPropertyChanged();
            }
        }

        public bool ShowDelete
        {
            get { return _showDelete; }
            set
            {
                _showDelete = value;
                NotifyPropertyChanged();
            }
        }

        public bool ShowClose
        {
            get { return _showClose; }
            set
            {
                _showClose = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event RoutedEventHandler OnSave
        {
            add { AddHandler(OnSaveEvent, value); }
            remove { RemoveHandler(OnSaveEvent, value); }
        }

        public event RoutedEventHandler OnDelete
        {
            add { AddHandler(OnDeleteEvent, value); }
            remove { AddHandler(OnDeleteEvent, value); }
        }

        public event RoutedEventHandler OnClose
        {
            add { AddHandler(OnCloseEvent, value); }
            remove { AddHandler(OnCloseEvent, value); }
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OnSaveEvent, this));
        }

        private void OnClickExcluir(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OnDeleteEvent, this));
        }

        private void OnClickFechar(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OnCloseEvent, this));
        }


        public bool SalvarAtivado
        {
            get { return (bool)GetValue(SalvarAtivadoProperty); }
            set
            {
                SetValue(SalvarAtivadoProperty, value);
            }
        }


        public static readonly DependencyProperty SalvarAtivadoProperty =
             DependencyProperty.Register("SalvarAtivado", typeof(bool), typeof(ToolbarFormulario), new PropertyMetadata(true, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ToolbarFormulario;
            if (control != null) control.BtnSalvar.IsEnabled = (bool) e.NewValue;
        }

        [NotifyPropertyChangedInvocator]
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}