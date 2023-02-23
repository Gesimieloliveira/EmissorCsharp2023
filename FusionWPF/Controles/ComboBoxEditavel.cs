using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using FusionLibrary.Wpf.Tools;

// ReSharper disable MemberCanBePrivate.Global

namespace FusionWPF.Controles
{
    public class ComboBoxEditavel : ComboBox
    {
        private const string EditableTextBox = "PART_EditableTextBox";
        private const string PopupPart = "PART_Popup";
        private const string DropDownPart = "PART_DropDownToggle";
        private DependencyObject EditableTextBoxElement => GetTemplateChild(EditableTextBox);
        private Popup PopupElement => (Popup)GetTemplateChild(PopupPart);
        private ToggleButton DropDownElement => (ToggleButton)GetTemplateChild(DropDownPart);

        public static readonly DependencyProperty AvancarComEnterProperty = DependencyProperty.Register("AvancarComEnter", typeof(bool), typeof(ComboBoxEditavel), new PropertyMetadata(true));

        public bool AvancarComEnter
        {
            get => (bool) GetValue(AvancarComEnterProperty);
            set => SetValue(AvancarComEnterProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (EditableTextBoxElement != null)
            {
                FocusAdvancement.SetAdvancesByEnterKey(EditableTextBoxElement, AvancarComEnter);
            }

            if (DropDownElement != null)
            {
                DropDownElement.SizeChanged += ResizePopup;
            }

            if (IsFocused)
            {
                MoveFocusToEditableTextBox();
            }
        }

        private void MoveFocusToEditableTextBox()
        {
            if (!IsEditable || !(EditableTextBoxElement is TextBox tb))
            {
                return;
            }

            tb.Focus();
            tb.SelectAll();
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            MoveFocusToEditableTextBox();
        }

        private void ResizePopup(object sender, SizeChangedEventArgs e)
        {
            if (PopupElement != null)
            {
                PopupElement.Width = e.NewSize.Width;
            }
        }
    }
}