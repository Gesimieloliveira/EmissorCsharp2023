using NHibernate;

namespace FusionCore.Configuracoes
{
    public class RepositorioConfiguracaoCaixa
    {
        private readonly ISession _session;

        public RepositorioConfiguracaoCaixa(ISession session)
        {
            _session = session;
        }

        public ConfiguracaoControleDeCaixa ObterUnica()
        {
            var query = _session.QueryOver<ConfiguracaoControleDeCaixa>();

            return query.SingleOrDefault();
        }

        public void Alterar(ConfiguracaoControleDeCaixa configuracao)
        {
            _session.Update(configuracao);
            _session.Flush();
        }
    }
}