using System;
using FusionCore.FusionPdv.Sessao;
using FusionCore.Helpers.Sincronizador;
using FusionCore.PdvSincronizador.Sync;

namespace FusionSincronizador.Core
{
    public class ServicoSincronizadorMain
    {
        public void ExecutarSincronizacao()
        {
            try
            {
                Log.Registrar("Serviço iniciou uma sincronização");
                Sincronizador.Instancia.SincronizarTudo();

                new GravaStatusPdvSyncronizador("1").Executar();
            }
            catch (Exception ex)
            {
                new GravaStatusPdvSyncronizador("0").Executar();

                CriaLogDaException(ex);

                try
                {
                    GerenciaSessao.GerenciaSessaoInicializar();
                }
                catch (Exception ext)
                {
                    CriaLogDaException(ext);
                }
            }
        }

        private void CriaLogDaException(Exception exception)
        {
            try
            {
                LogExeption(exception);
                LogInnerException(exception.InnerException);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static void LogInnerException(Exception e)
        {
            while (true)
            {
                if (e == null) return;

                Log.Registrar("--- Inner Exception ---");
                Log.Registrar("Mensagem de erro:" + e.Message);
                Log.Registrar("StackTrace: " + e.StackTrace);
                e = e.InnerException;
            }
        }

        private static void LogExeption(Exception ex)
        {
            Log.Registrar("Serviço falhou na sincronizacao");
            Log.Registrar("Mensagem de erro: " + ex.Message);
            Log.Registrar("StackTrace: " + ex.StackTrace);
        }
    }
}