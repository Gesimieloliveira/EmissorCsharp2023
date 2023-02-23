using System.Threading;
using System.Windows;
using MahApps.Metro.SimpleChildWindow;

namespace Dev.AppDemo
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FusionChildWindowClick(object sender, RoutedEventArgs e)
        {
            var fcw = new DemoFusionChildWindow();
            this.ShowChildWindowAsync(fcw);
        }

        private async void OnAtivarProgressClick(object sender, RoutedEventArgs e)
        {
            await RunTaskWithProgress(() => Thread.Sleep(3000));
        }

        private async void OnAtivarProgressBarClick(object sender, RoutedEventArgs e)
        {
            UseProgeress = true;
            ProgressMaximum = 100;
            Progress = 0;

            void IncrementProgress()
            {
                Dispatcher.Invoke(() => Progress += 25);
            }

            await RunTaskWithProgress(() =>
            {
                for (var i = 0; i < 5; i++)
                {
                    Thread.Sleep(500);
                    IncrementProgress();
                }
            });

            UseProgeress = false;
        }
    }
}