using NHibernate;

namespace FusionCore.FusionServico
{
    public static class FusionServicoFacade
    {
        public static ConfiguracaoExportacao CarregaConfiguraaco(ISession session)
        {
            var repositorio = new RepositorioFusionServico(session);
            return repositorio.GetConfiguracaoExportacao();
        }

        public static void UpdateConfiguracao(ISession session, ConfiguracaoExportacao configuracao)
        {
            var repositorio = new RepositorioFusionServico(session);
            repositorio.Update(configuracao);
        }
    }
}