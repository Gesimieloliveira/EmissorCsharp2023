using System;
using System.ServiceProcess;
using System.Threading;
using FusionCore.Helpers.Ambiente;

namespace FusionCore.Helpers.Sincronizador
{
    public class ConsultarStatusDoServico
    {
        public void Executar(string nomeServico)
        {
            var sc = new ServiceController(nomeServico);

            if(sc.Status != ServiceControllerStatus.Running) throw new Exception("O serviço não está rodando.");

            VerificaStatus();
        }

        private void VerificaStatus()
        {
            var valor = LerDadoArquivo();

            if (string.IsNullOrEmpty(valor))
            {
                valor = "1";
                new GravaStatusPdvSyncronizador(valor).Executar();
            }

            var resultado = int.Parse(valor);

            if(resultado == 0) throw new Exception("Servidor está fora do ar");
        }

        private static string LerDadoArquivo()
        {
            var continuar = true;
            string valor = null;

            while (continuar)
            {
                Thread.Sleep(1000);
                ManipulaArquivo arquivo = null;
                try
                {
                    arquivo =
                        new ManipulaArquivo(ManipulaArquivo.LocalAplicacao() + "\\StatusDoFusionSincronizador.agil4");
                    valor = arquivo.VerificaSeExisteECria()
                        .AbreArquivoParaLeitura()
                        .LerDadosComReplaceDeEspacosParaVazio();
                    continuar = false;
                }
                catch (Exception)
                {
                    LerDadoArquivo();
                }
                finally
                {
                    arquivo?.FecharArquivo();
                }
            }

            return valor;
        }

        
    }
}
