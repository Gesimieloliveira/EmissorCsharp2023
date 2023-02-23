using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;

namespace FusionWPF.Controles
{
    public class SearchTextBox : TextBox
    {
        public static readonly RoutedEvent SearchEventEvent = EventManager.RegisterRoutedEvent(
            "SearchEvent",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SearchTextBox));

        public static readonly RoutedEvent ClearEventEvent = EventManager.RegisterRoutedEvent(
            "ClearEvent",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SearchTextBox));

        public static readonly DependencyProperty SearchCommandProperty = DependencyProperty.Register(
            "SearchCommand",
            typeof(ICommand),
            typeof(SearchTextBox),
            new FrameworkPropertyMetadata(null, ButtonCommandChanged));

        public static readonly DependencyProperty ClearCommandProperty = DependencyProperty.Register(
            "ClearCommand",
            typeof(ICommand),
            typeof(SearchTextBox),
            new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty ShowButtonProperty = DependencyProperty.Register(
            "ShowButton",
            typeof(bool),
            typeof(SearchTextBox),
            new FrameworkPropertyMetadata(true, ShowButtonChanged));


        public static readonly DependencyProperty SelectAllOnFocusPproperty = DependencyProperty.Register(
            "SelectAllOnFocus",
            typeof(bool),
            typeof(SearchTextBox),
            new FrameworkPropertyMetadata(false, SelectAllOnFocusChanged));

        public static readonly DependencyProperty ButtonEnabledWhenReadOnlyProperty = DependencyProperty.Register(
            "ButtonEnabledWhenReadOnly",
            typeof(bool),
            typeof(SearchTextBox),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty ButtonClearEnabledWhenHasTextProperty = DependencyProperty.Register(
            "ButtonClearEnabledWhenHasText",
            typeof(bool),
            typeof(SearchTextBox),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private Button PickerButton => (Button) GetTemplateChild("PART_ClearText");

        public event RoutedEventHandler SearchEvent
        {
            add => AddHandler(SearchEventEvent, value);
            remove => RemoveHandler(SearchEventEvent, value);
        }

        public event RoutedEventHandler ClearEvent
        {
            add => AddHandler(ClearEventEvent, value);
            remove => RemoveHandler(ClearEventEvent, value);
        }

        public bool ShowButton
        {
            get => (bool) GetValue(ShowButtonProperty);
            set => SetValue(ShowButtonProperty, value);
        }

        public ICommand SearchCommand
        {
            get => (ICommand) GetValue(SearchCommandProperty);
            set => SetValue(SearchCommandProperty, value);
        }

        public ICommand ClearCommand
        {
            get => (ICommand) GetValue(ClearCommandProperty);
            set => SetValue(ClearCommandProperty, value);
        }

        public bool SelectAllOnFocus
        {
            get => (bool) GetValue(SelectAllOnFocusPproperty);
            set => SetValue(SelectAllOnFocusPproperty, value);
        }

        public bool ButtonEnabledWhenReadOnly
        {
            get => (bool) GetValue(ButtonEnabledWhenReadOnlyProperty);
            set => SetValue(ButtonEnabledWhenReadOnlyProperty, value);
        }

        public bool ButtonClearEnabledWhenHasText
        {
            get => (bool) GetValue(ButtonClearEnabledWhenHasTextProperty);
            set => SetValue(ButtonClearEnabledWhenHasTextProperty, value);
        }

        private static void ButtonCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is SearchTextBox pickerBox))
            {
                return;
            }

            // TextBoxHelper.SetButtonCommand(pickerBox, (ICommand) e.NewValue);
        }

        private static void ShowButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pickerBox = d as SearchTextBox;

            if (pickerBox?.PickerButton == null)
            {
                return;
            }

            pickerBox.PickerButton.Visibility = (bool) e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }


        private static void SelectAllOnFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxHelper.SetSelectAllOnFocus(d, (bool) e.NewValue);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (PickerButton != null &&
                (e.Property.ToString() == nameof(IsReadOnly) ||
                 e.Property.ToString() == nameof(ButtonEnabledWhenReadOnly)))
            {
                EnableButtonIfReadOnly();
            }
        }

        private void EnableButtonIfReadOnly()
        {
            if (IsReadOnly)
            {
                PickerButton.IsEnabled = ButtonEnabledWhenReadOnly;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (PickerButton != null)
            {
                var showButton = (bool) GetValue(ShowButtonProperty);

                PickerButton.Click -= ClickPickerButtonHandler;
                PickerButton.Click += ClickPickerButtonHandler;

                PickerButton.Visibility = showButton ? Visibility.Visible : Visibility.Collapsed;

                EnableButtonIfReadOnly();
            }

            TextBoxHelper.SetSelectAllOnFocus(this, SelectAllOnFocus);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.B)
            {
                e.Handled = true;
                InvokeSearchByShortcut();
            }
        }

        private void InvokeSearchByShortcut()
        {
            if (ButtonEnabledWhenReadOnly == false) return;
            if (IsEnabled == false) return;

            ExecuteSearchAction(this, new RoutedEventArgs());
        }

        private void ClickPickerButtonHandler(object sender, RoutedEventArgs e)
        {
            if (Focusable)
            {
                Keyboard.Focus(this);
            }

            if (ButtonClearEnabledWhenHasText && Text?.Length > 0)
            {
                ExecuteClearAction(sender, e);
                return;
            }

            ExecuteSearchAction(sender, e);
        }

        private void ExecuteSearchAction(object sender, RoutedEventArgs e)
        {
            if (SearchCommand != null && SearchCommand.CanExecute(sender))
            {
                SearchCommand.Execute(sender);
            }

            e.RoutedEvent = SearchEventEvent;
            RaiseEvent(e);
        }

        private void ExecuteClearAction(object sender, RoutedEventArgs e)
        {
            if (ClearCommand != null && ClearCommand.CanExecute(sender))
            {
                ClearCommand.Execute(sender);
            }

            e.RoutedEvent = ClearEventEvent;
            RaiseEvent(e);
        }
    }
}