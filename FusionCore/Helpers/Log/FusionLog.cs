using System;
using System.IO;
using System.Reflection;
using FusionCore.Helpers.Ambiente;
using log4net;
using log4net.Config;

namespace FusionCore.Helpers.Log
{
    public static class FusionLog
    {
        private static bool _configurado;

        public static ILog GetLogger(MethodBase method)
        {
            if (_configurado == false)
            {
                ConfiguraLog();
            }

            return LogManager.GetLogger(method.DeclaringType);
        }

        public static void ConfiguraLog()
        {
            try
            {
                var log4ConfigFile = Path.Combine(DiretorioAssembly.GetPastaConfig(), "log4net.config");

                if (File.Exists(log4ConfigFile))
                {
                    File.Delete(log4ConfigFile);
                }

                var assembly = Assembly.GetExecutingAssembly();
                var embededConfig = "FusionCore.Helpers.Log.log4net.config";
                string content;

#if DEBUG
                embededConfig = "FusionCore.Helpers.Log.log4netDebug.config";
#endif

                using (var stream = assembly.GetManifestResourceStream(embededConfig))
                using (var reader = new StreamReader(stream))
                {
                    content = reader.ReadToEnd();
                }

                using (var writer = File.CreateText(log4ConfigFile))
                {
                    writer.Write(content);
                }

                XmlConfigurator.Configure(new FileInfo(log4ConfigFile));
                _configurado = true;
            }
            catch (Exception e)
            {
                throw new Exception("Falha configurar Log4: " + e.Message, e);
            }
        }
    }
}