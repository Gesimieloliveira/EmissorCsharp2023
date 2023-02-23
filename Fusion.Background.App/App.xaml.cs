using System.Diagnostics;
using System.Windows;
using Fusion.WhiteLabel;
using FusionCore.Helpers.Ambiente;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Background.App
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (UnicoAberto.IsAppAlreadyRunning(Process.GetCurrentProcess()))
            {
                DialogBox.MostraAviso("Fusion Serviços", $"{MarcaWhiteLabel.NomeSoftware} já está em excecução!");
                Process.GetCurrentProcess().Kill();
                return;
            }
        }
    }
}
