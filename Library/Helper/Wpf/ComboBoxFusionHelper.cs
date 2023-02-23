using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FusionLibrary.Helper.Wpf
{
    public static class ComboBoxFusionHelper
    {
        public static readonly DependencyProperty AbreDropDownProperty =
            DependencyProperty.RegisterAttached("AbreDropDown", typeof(bool), typeof(ComboBoxFusionHelper),
                new FrameworkPropertyMetadata(false, AbreDropDownChanged));

        private static void AbreDropDownChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var comboBox = d as ComboBox;
            if (comboBox == null)
                throw new InvalidOperationException("GThorComboBoxHelper deve ser utilizado apenas em ComboBox");

            if ((bool)e.NewValue) comboBox.PreviewTextInput += PreviewTextboxInputDropDownOpen;
            else comboBox.PreviewTextInput -= PreviewTextboxInputDropDownOpen;
        }

        private static void PreviewTextboxInputDropDownOpen(object sender, TextCompositionEventArgs e)
        {
            var comboBox = sender as ComboBox;

            if (comboBox == null) return;
            if (typeof(ComboBox) != comboBox.GetType()) return;

            comboBox.IsDropDownOpen = true;
        }

        public static bool GetAbreDropDown(DependencyObject obj)
        {
            return (bool)obj.GetValue(AbreDropDownProperty);
        }

        public static void SetAbreDropDown(DependencyObject obj, bool value)
        {
            obj.SetValue(AbreDropDownProperty, value);
        }
    }
}