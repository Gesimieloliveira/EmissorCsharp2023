using System;
using System.ServiceProcess;
using Fusion.Storage;
using FusionCore.Helpers.Log;
using FusionNfce.Storage;

namespace FusionNfceSincronizador
{
    static class Program
    {
        static void Main(string[] args)
        {
            FusionLog.ConfiguraLog();

            DependenciaStorageFusion.Referenciar();
            DependenciaStorageNfce.Referenciar();

            var host = new FusionNfceSincronizador();

            if (Environment.UserInteractive)
            {
                host.CallOnStart(args);
                Console.WriteLine(@"Press any key to stop program");
                Console.Read();
                host.CallOnStop();

                return;
            }

            ServiceBase.Run(host);
        }
    }
}
