using NHibernate;

namespace FusionCore.FusionServico
{
    public class RepositorioFusionServico
    {
        private readonly ISession _session;

        public RepositorioFusionServico(ISession session)
        {
            _session = session;
        }

        public ConfiguracaoExportacao GetConfiguracaoExportacao()
        {
            return _session.Get<ConfiguracaoExportacao>((byte)1);
        }

        public void Update(ConfiguracaoExportacao configuracao)
        {
            _session.Update(configuracao);
            _session.Flush();
        }
    }
}