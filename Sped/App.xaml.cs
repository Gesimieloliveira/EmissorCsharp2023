using System.Windows;
using Fusion.Storage;
using FusionCore.Helpers.Log;

namespace Sped
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            FusionLog.ConfiguraLog();
            DependenciaStorageFusion.Referenciar();
        }
    }
}
