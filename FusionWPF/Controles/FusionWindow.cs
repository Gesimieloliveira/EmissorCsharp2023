using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using MahApps.Metro.Controls;

namespace FusionWPF.Controles
{
    public class FusionWindow : MetroWindow
    {
        public static readonly DependencyProperty UseProgeressProperty =
            DependencyProperty.Register("UseProgeress",
                typeof(bool),
                typeof(FusionWindow),
                new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty ProgressMaximumProperty =
            DependencyProperty.Register("ProgressMaximum",
                typeof(double),
                typeof(FusionWindow),
                new FrameworkPropertyMetadata(100d));

        public static readonly DependencyProperty ProgressProperty =
            DependencyProperty.Register("Progress",
                typeof(double),
                typeof(FusionWindow),
                new FrameworkPropertyMetadata(0D));

        public static readonly RoutedEvent ProgressHideEvent = EventManager.RegisterRoutedEvent(
            "ProgressHide",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(FusionWindow));

        private readonly IDictionary<Key, Action> _atalhos;
        private readonly FusionWindowProgresso _progressBar;
        private IInputElement _elementFocusedOnProgressStart;

        public FusionWindow()
        {
            _atalhos = new Dictionary<Key, Action>();
            _progressBar = new FusionWindowProgresso();
            Style = (Style) FindResource("MetroWindowStyle");
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        [Bindable(true)]
        [DefaultValue(false)]
        public bool UseProgeress
        {
            get => (bool) GetValue(UseProgeressProperty);
            set => SetValue(UseProgeressProperty, value);
        }

        [Bindable(true)]
        [DefaultValue(100D)]
        public double ProgressMaximum
        {
            get => (double) GetValue(ProgressMaximumProperty);
            set => SetValue(ProgressMaximumProperty, value);
        }

        [Bindable(true)]
        [DefaultValue(0D)]
        public double Progress
        {
            get => (double) GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }

        public event RoutedEventHandler ProgressHide
        {
            add => AddHandler(ProgressHideEvent, value);
            remove => RemoveHandler(ProgressHideEvent, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            AttachProgerssBar();
            KeyDown += FusionWindowKeyDownHandler;
        }

        private void AttachProgerssBar()
        {
            if (!(GetVisualChild(0) is Grid firstChild))
            {
                throw new Exception("Falha ao iniciar elementos de design da janela");
            }

            _progressBar.Visibility = Visibility.Collapsed;
            _progressBar.KeyDown += (sender, args) => args.Handled = _progressBar.IsVisible;
            _progressBar.LostFocus += (sender, args) => args.Handled = _progressBar.IsVisible;

            BindingUseProgress();
            BindingProgressMaximum();
            BindingProgress();

            firstChild.Children.Add(_progressBar);
        }

        private void BindingUseProgress()
        {
            var binding = new Binding(nameof(UseProgeress));
            binding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(FusionWindow), 1);
            BindingOperations.SetBinding(_progressBar, FusionWindowProgresso.UsarProgressoProperty, binding);
        }

        private void BindingProgress()
        {
            var binding = new Binding(nameof(Progress));
            binding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(FusionWindow), 1);
            BindingOperations.SetBinding(_progressBar, FusionWindowProgresso.ProgressoAtualProperty, binding);
        }

        private void BindingProgressMaximum()
        {
            var binding = new Binding(nameof(ProgressMaximum));
            binding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(FusionWindow), 1);
            BindingOperations.SetBinding(_progressBar, FusionWindowProgresso.ProgressoMaximoProperty, binding);
        }

        protected async Task RunTaskWithProgress(Action action)
        {
            await Task.Run(() =>
            {
                ShowProgressRing();

                try
                {
                    action?.Invoke();
                    HideProgressRing();
                }
                catch (Exception e)
                {
                    HideProgressRing();
                    Dispatcher.Invoke(() => throw e);
                }
            });
        }

        protected void ShowProgressRing()
        {
            Dispatcher.Invoke(() =>
            {
                _elementFocusedOnProgressStart = Keyboard.FocusedElement;
                _progressBar.Visibility = Visibility.Visible;
                _progressBar.Focus();
            });
        }

        protected void HideProgressRing()
        {
            Dispatcher.Invoke(() =>
            {
                _progressBar.Visibility = Visibility.Collapsed;
                Keyboard.Focus(_elementFocusedOnProgressStart);
                RaiseEvent(new RoutedEventArgs(ProgressHideEvent, _progressBar));
            });
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            try
            {
                if (ComponentDispatcher.IsThreadModal)
                {
                    ResizeMode = ResizeMode.NoResize;
                    Style = (Style) FindResource("MetroWindowDialogStyle");
                }
            }
            catch
            {
                //ignore
            }
        }

        protected void RegistrarAtalhoBotao(Key key, Button button)
        {
            var action = new Action(() =>
            {
                if (!button.IsEnabled || !button.IsVisible)
                {
                    return;
                }

                var peer = new ButtonAutomationPeer(button);
                var inokeProvider = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;

                inokeProvider?.Invoke();
            });

            RegistrarAtalho(key, action);
        }

        protected void RegistrarAtalho(Key key, Action acao)
        {
            if (_atalhos.ContainsKey(key))
            {
                _atalhos[key] = acao;
                return;
            }

            _atalhos.Add(key, acao);
        }

        private void FusionWindowKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (!_atalhos.ContainsKey(e.Key))
            {
                return;
            }

            _atalhos[e.Key]?.Invoke();
            e.Handled = true;
        }
    }
}