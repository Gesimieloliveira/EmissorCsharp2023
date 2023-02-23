using System;
using System.IO;
using FusionCore.Debug;

namespace FusionCore.Helpers.Ambiente
{
    public static class FusionProjetoHelper
    {
        public static string DiretorioProjeto
        {
            get
            {
                var dir = Environment.GetEnvironmentVariable("PROJETO_FUSION") ?? string.Empty;

                if (File.Exists(Path.Combine(dir, "Fusion.sln")))
                {
                    return dir;
                }

                throw new InvalidOperationException("Modo Debug: Preciso que defina a variável de ambiente PROJETO_FUSION!");
            }
        }

        public static string NomeBranch => new GitRepositoryInformation(DiretorioProjeto).BranchName;

    }
}