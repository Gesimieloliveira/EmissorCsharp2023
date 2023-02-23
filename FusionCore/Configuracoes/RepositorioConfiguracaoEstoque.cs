using System;
using NHibernate;

namespace FusionCore.Configuracoes
{
    public class RepositorioConfiguracaoEstoque
    {
        private readonly ISession _sessao;

        public RepositorioConfiguracaoEstoque(ISession sessao)
        {
            _sessao = sessao;
        }

        public ConfiguracaoEstoque GetConfiguracaoUnica()
        {
            var query = _sessao.QueryOver<ConfiguracaoEstoque>()
                .Where(e => e.Id == 1)
                .Take(1);

            return query.SingleOrDefault();
        }

        public void SaveOrUpdate(ConfiguracaoEstoque configuracao)
        {
            configuracao.AlteradoEm = DateTime.Now;

            _sessao.SaveOrUpdate(configuracao);
            _sessao.Flush();
        }

        public void Update(ConfiguracaoEstoque configuracao)
        {
            configuracao.AlteradoEm = DateTime.Now;

            _sessao.Update(configuracao);
            _sessao.Flush();
        }
    }
}