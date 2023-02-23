using System.Reflection;

namespace FusionCore.Helpers.AssemblyUtils.Leitura
{
    public class Versao3Digito : IRegraLeitura
    {
        public string Ler(Assembly assembly)
        {
            var version = assembly.GetName().Version;
            return $"v{version.Major}.{version.Minor}.{version.Build}";
        }
    }
}