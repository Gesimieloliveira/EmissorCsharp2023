using System.Reflection;
using FusionCore.Helpers.AssemblyUtils.Leitura;

namespace FusionCore.Helpers.AssemblyUtils
{
    public static class AssemblyHelper
    {
        public static string LerVersao3Digitos(Assembly assembly)
        {
            return new Versao3Digito().Ler(assembly);
        }

        public static string LerDoAssemblyPrincipal(IRegraLeitura regraLeitura)
        {
            return regraLeitura.Ler(Assembly.GetEntryAssembly());
        }

        public static string LerDoAssemblyChamou(IRegraLeitura regraLeitura)
        {
            return regraLeitura.Ler(Assembly.GetCallingAssembly());
        }
    }
}