using FusionCore.Repositorio.FusionNfce;
using NHibernate;

namespace FusionCore.FusionNfce.ConfiguracaoTerminal
{
    public static class ConfiguracaoTerminalFacade
    {
        public static byte ObtemNumeroTerminal(ISession session)
        {
            var repositorio = new RepositorioConfiguracaoTerminalNfce(session);

            return repositorio.ObtemIdentificadorTerminal();
        }
    }
}