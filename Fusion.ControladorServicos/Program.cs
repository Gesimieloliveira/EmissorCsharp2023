using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Threading;

namespace Fusion.ControladorServicos
{
    internal class Program
    {
        internal static void Main()
        {
            if (SetupIsOpen())
            {
                return;
            }

            try
            {
                ChecharServicos();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(-1);
            }
        }

        private static bool SetupIsOpen()
        {
            return Mutex.TryOpenExisting(@"Global\1755C08A-D68F-47D0-AF46-D8B45C1932C5", out _);
        }

        private static void ChecharServicos()
        {
            var servicos = new[]
            {
                "MSSQL$FUSION",
                "FusionApi"
            };

            TryStartServicos(servicos);
        }

        private static void TryStartServicos(IEnumerable<string> servicos)
        {
            foreach (var sNome in servicos)
            {
                if (SetupIsOpen())
                {
                    break;
                }

                try
                {
                    using (var sc = new ServiceController(sNome))
                    {
                        if (sc.Status != ServiceControllerStatus.Running)
                        {
                            sc.Start();
                            Console.WriteLine($"Comando START enviado para {sNome}");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Falha ao controlar serviço {sNome} -> {e.Message}");
                }
            }
        }
    }
}