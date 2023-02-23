using System;
using System.IO;

namespace FusionSincronizador.Core
{
    public static class Log
    {
        public static string Path { private get; set; }

        static Log()
        {
            Path = AppDomain.CurrentDomain.BaseDirectory;
        }

        public static void Registrar(string evento)
        {
            try
            {
                var stringPath = string.Concat(Path, "pdv-sincronizador.log");
                var vWriter = new StreamWriter(@stringPath, true);
                vWriter.WriteLine(DateTime.Now + ":" + evento);
                vWriter.Flush();
                vWriter.Close();
            }
            catch (Exception)
            {
                //ingore
            }
        }
    }
}