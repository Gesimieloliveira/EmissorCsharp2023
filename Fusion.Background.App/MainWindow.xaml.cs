using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using FusionCore.Debug;
using FusionCore.Helpers.Log;
using FusionWPF.Base.Utils.Dialogs;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace Fusion.Background.App
{
    public partial class MainWindow
    {
        private readonly NotifyIcon _nIcon = new NotifyIcon();
        private Worker _worker;

        public MainWindow()
        {
            InitializeComponent();

            var icone = "icon.ico";

            if (BuildMode.IsProducao)
            {
                icone = "C:\\SistemaFusion\\Fusion\\icon.ico";
            }

            _nIcon.Icon = new Icon(icone);
            _nIcon.Visible = true;
            _nIcon.ShowBalloonTip(5000, "Serviços App", "Serviços", ToolTipIcon.Info);
            _nIcon.ContextMenuStrip = new ContextMenuStrip();
            _nIcon.ContextMenuStrip.Items.Add("Fechar Aplicação Serviços",
                null,
                (sender, args) =>
                {
                    if (!DialogBox.MostraConfirmacao("Deseja realmente fechar e encerrar os serviços?",
                        MessageBoxImage.Question))
                        return;

                    Close();
                });
            _nIcon.MouseClick += delegate(object sender, MouseEventArgs args)
            {
                if (args.Button == MouseButtons.Left)
                {
                    Visibility = Visibility.Visible;
                }
            };
        }

        private void MainWindow_OnContentRendered(object sender, EventArgs e)
        {
            Visibility = Visibility.Collapsed;
            FusionLog.ConfiguraLog();
            _worker = new Worker();

            _worker.RunWorkerAsync();
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Escape) return;

            Visibility = Visibility.Collapsed;
        }

        private void MainWindow_OnStateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                Visibility = Visibility.Collapsed;
        }

        private void ReinicarServicos_OnClick(object sender, RoutedEventArgs e)
        {
            if (!DialogBox.MostraConfirmacao("Deseja realmente reiniciar os serviços?", MessageBoxImage.Question))
                return;

            _worker?.Dispose();
            _worker = new Worker();
            _worker.RunWorkerAsync();

            DialogBox.MostraAviso("Serviços Reiniciados com Sucesso");
        }

        private void EncerrarServicos_OnClick(object sender, RoutedEventArgs e)
        {
            if (!DialogBox.MostraConfirmacao("Deseja realmente encerrar os serviços?", MessageBoxImage.Question))
                return;

            _worker?.Dispose();
            _worker = null;

            DialogBox.MostraAviso("Serviços Encerrados com Sucesso");
        }


        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _worker?.Dispose();
            _worker = null;
        }
    }
}
