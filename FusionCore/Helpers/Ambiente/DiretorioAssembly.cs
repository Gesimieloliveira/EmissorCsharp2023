using System;
using System.IO;
using System.Reflection;
using FusionCore.Debug;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedVariable

namespace FusionCore.Helpers.Ambiente
{
    public static class DiretorioAssembly
    {
        private static readonly Assembly CurrentAssemlby;

        static DiretorioAssembly()
        {
            CurrentAssemlby = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        }

        public static string GetPastaAssembly()
        {
            return Path.GetDirectoryName(CurrentAssemlby.Location);
        }

        public static string GetPastaConfig()
        {
            return PreparaDiretorio(Path.Combine(GetDiretorioBase(), "Config"));
        }

        private static string GetDiretorioBase()
        {
            if (BuildMode.IsProducao)
            {
                return Path.GetDirectoryName(CurrentAssemlby.Location);
            }

            var assemblyName = CurrentAssemlby.GetName().Name;

            assemblyName = assemblyName.Replace("FusionNfceSincronizador", "FusionNFCe");
            assemblyName = assemblyName.Replace("FusionSincronizador", "FusionPdv");
            assemblyName = assemblyName.Replace("Sped", "Fusion");
            assemblyName = assemblyName.Replace("Fusion.Servico", "Fusion");
            assemblyName = assemblyName.Replace("Dev.AppRelatorios", "Fusion");
            assemblyName = assemblyName.Replace("Fusion.Background.App", "Fusion");

            var specialFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var fusionDebug = Path.Combine(specialFolder, "SistemaFusion", "Debug");

            return Path.Combine(fusionDebug, "Branch", FusionProjetoHelper.NomeBranch, assemblyName);
        }

        private static string PreparaDiretorio(string diretorio)
        {
            if (Directory.Exists(diretorio))
            {
                return diretorio;
            }

            Directory.CreateDirectory(diretorio);
            return diretorio;
        }

        public static string GetPastaTemp()
        {
            return PreparaDiretorio(Path.Combine(GetDiretorioBase(), "Temp"));
        }
    }
}