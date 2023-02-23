using System;
using System.IO;
using FusionCore.Helpers.Ambiente;

namespace FusionCore.Helpers.Log
{
    public class RegistarLog : IRegistrarLog
    {
        private static IRegistrarLog _instancia;

        public string Path { get; set; }

        private RegistarLog()
        {
            Path = ManipulaPasta.LocalSistema();
        }

        public void Registrar(string evento)
        {
            var stringPath = string.Concat(Path, "\\nfce-sincronizador.log");
            var vWriter = new StreamWriter(@stringPath, true);
            vWriter.WriteLine(DateTime.Now + " : " + evento);
            vWriter.Flush();
            vWriter.Close();
        }

        public void RegistrarException(Exception ex)
        {
            CriaLogDaException(ex);
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

        private void LogInnerException(Exception e)
        {
            while (true)
            {
                if (e == null) return;

                Registrar("--- Inner Exception ---");
                Registrar("Mensagem de erro:" + e.Message);
                Registrar("StackTrace: " + e.StackTrace);
                e = e.InnerException;
            }
        }

        private void LogExeption(Exception ex)
        {
            Registrar("Serviço falhou na sincronizacao");
            Registrar("Mensagem de erro: " + ex.Message);
            Registrar("StackTrace: " + ex.StackTrace);
        }

        public static IRegistrarLog Istancia => Cria();

        private static IRegistrarLog Cria()
        {
            return _instancia ?? (_instancia = new RegistarLog());
        }
    }
}