using System;
using System.ServiceProcess;
using FusionCore.Helpers.Log;

namespace FusionSincronizador
{
    internal static class ServicoMain
    {
        private static void Main(string[] args)
        {
            FusionLog.ConfiguraLog();

            var host = new FusionPdvSincronizador();

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