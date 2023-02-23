using System;
using System.Windows;
using System.Windows.Threading;
using Fusion.Storage;
using FusionCore.Helpers.Culture;
using FusionCore.Helpers.Log;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Conversor
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DependenciaStorageFusion.Referenciar();
            CultureInfoHelper.DefineDefaultCultureInfo();
            FusionLog.ConfiguraLog();

            DispatcherUnhandledException += ExceptionHandler;
        }

        private void ExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                e.Handled = true;

                if (e.Exception.GetType() == typeof(InvalidOperationException))
                {
                    DialogBox.MostraAviso(e.Exception.Message);
                    return;
                }

                DialogBox.MostraErro(e.Exception.Message, e.Exception);
            });
        }
    }
}
