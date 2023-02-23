using NHibernate;

namespace FusionCore.Configuracoes
{
    public class RepositorioMovimentaEstoqueFaturamento
    {
        private readonly ISession _sessao;

        public RepositorioMovimentaEstoqueFaturamento(ISession sessao)
        {
            _sessao = sessao;
        }

        public ConfiguracaoEstoqueFaturamento ObterConfiguracaoEstoqueFaturamento()
        {
            return _sessao.QueryOver<ConfiguracaoEstoqueFaturamento>()
                .Where(cef => cef.Id == ConfiguracaoEstoqueFaturamento.IdStatic)
                .SingleOrDefault<ConfiguracaoEstoqueFaturamento>();
        }

        public void SalvarOuAtualizar(ConfiguracaoEstoqueFaturamento configuracaoEstoqueFaturamento)
        {
            _sessao.SaveOrUpdate(configuracaoEstoqueFaturamento);
            _sessao.Flush();
        }
    }
}