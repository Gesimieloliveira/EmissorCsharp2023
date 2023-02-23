using System;
using System.Linq;
using System.ServiceProcess;

namespace FusionNfce.Core
{
    public class ControlaServicoSincronizador
    {
        private const string NomeDoServico = "FusionNfceSincronizador";

        public static void Iniciar()
        {
            if (NaoExisteOServico()) return;

            try
            {
                using (var servico = CriaServicoController(out var tempoEsgotado))
                {
                    servico.Start();
                    servico.WaitForStatus(ServiceControllerStatus.Running, tempoEsgotado);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Falaha o iniciar serviço: {e.Message}");
            }
        }

        public static void Parar()
        {
            if (NaoExisteOServico()) return;

            try
            {
                using (var servico = CriaServicoController(out var tempoEsgotado))
                {
                    if (ServisoPausado(servico)) return;

                    servico.Stop();
                    servico.WaitForStatus(ServiceControllerStatus.Stopped, tempoEsgotado);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Falha ao parar serviço: {e.Message}");
            }
        }

        private static ServiceController CriaServicoController(out TimeSpan tempoEsgotado)
        {
            var servico = new ServiceController(NomeDoServico);
            tempoEsgotado = TimeSpan.FromSeconds(30);
            return servico;
        }

        private static bool NaoExisteOServico()
        {
            return ServiceController.GetServices().All(s => s.ServiceName != NomeDoServico);
        }

        private static bool ServisoPausado(ServiceController servico)
        {
            return servico.Status != ServiceControllerStatus.Running;
        }
    }
}