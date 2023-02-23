using System;
using System.IO;
using Fusion.Factories;

namespace Fusion.Visao.Relatorios.Comum
{
    public static class AcaoEscolherArquivoFrx
    {
        public static void Escolher(Action<string> sucesso)
        {
            var fusionPath = Environment.GetEnvironmentVariable("PROJETO_FUSION");
            var dialog = FileDialogFactory.CriaDialogFRX();

            if (fusionPath != null)
            {
                dialog.InitialDirectory = Path.Combine(fusionPath, "Fusion.FastReport", "Arquivos");
                dialog.CheckFileExists = true;
            }

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            sucesso(dialog.FileName);
        }
    }
}