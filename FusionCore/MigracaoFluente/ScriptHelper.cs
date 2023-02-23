namespace FusionCore.MigracaoFluente
{
    public static class ScriptHelper
    {
        public static string CriarPath(string nomeScript)
        {
            return "FusionCore.MigracaoFluente.Scripts." + nomeScript;
        }
    }
}