using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace FusionWPF.Controles
{
    public class FusionWindowProgresso : Control
    {
        public static readonly DependencyProperty UsarProgressoProperty =
            DependencyProperty.Register("UsarProgresso",
                typeof(bool),
                typeof(FusionWindowProgresso),
                new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty ProgressoMaximoProperty =
            DependencyProperty.Register("ProgressoMaximo",
                typeof(double),
                typeof(FusionWindowProgresso),
                new FrameworkPropertyMetadata(100D));

        public static readonly DependencyProperty ProgressoAtualProperty =
            DependencyProperty.Register("ProgressoAtual",
                typeof(double),
                typeof(FusionWindowProgresso),
                new FrameworkPropertyMetadata(0D));

        [Bindable(true)]
        [DefaultValue(false)]
        public bool UsarProgresso
        {
            get => (bool) GetValue(UsarProgressoProperty);
            set => SetValue(UsarProgressoProperty, value);
        }


        [Bindable(true)]
        [DefaultValue(100D)]
        public double ProgressoMaximo
        {
            get => (double) GetValue(ProgressoMaximoProperty);
            set => SetValue(ProgressoMaximoProperty, value);
        }

        [Bindable(true)]
        [DefaultValue(0D)]
        public double ProgressoAtual
        {
            get => (double) GetValue(ProgressoAtualProperty);
            set => SetValue(ProgressoAtualProperty, value);
        }

        private ProgressRing ProgressRing => GetTemplateChild("PART_ProgressRing") as ProgressRing;
        private MetroProgressBar ProgressBar => GetTemplateChild("PART_ProgressBar") as MetroProgressBar;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
        }
    }
}