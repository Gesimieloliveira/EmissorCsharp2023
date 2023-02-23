using System;
using System.Threading;
using FusionCore.Helpers.Ambiente;

namespace FusionCore.Helpers.Sincronizador
{
    public class GravaStatusPdvSyncronizador
    {
        private readonly string _status;

        public GravaStatusPdvSyncronizador(string status)
        {
            _status = status;
        }

        public void Executar()
        {
            TentaGravarNoArquivo(_status);
        }

        private static void TentaGravarNoArquivo(string valor)
        {
            var continuar = true;

            while (continuar)
            {
                Thread.Sleep(1000);
                ManipulaArquivo arquivo = null;
                try
                {
                    arquivo =
                        new ManipulaArquivo(ManipulaArquivo.LocalAplicacao() + "\\StatusDoFusionSincronizador.agil4",
                            valor);
                    arquivo.VerificaSeExisteECria().AbreArquivo().Escreve().FecharArquivo();
                    continuar = false;
                }
                catch (Exception)
                {
                    TentaGravarNoArquivo(valor);
                }
                finally
                {
                    arquivo?.FecharArquivo();
                }
            }
        }
    }
}
