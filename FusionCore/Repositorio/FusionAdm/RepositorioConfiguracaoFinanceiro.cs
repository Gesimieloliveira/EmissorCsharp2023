using System;
using FusionCore.FusionAdm.Financeiro;
using NHibernate;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioConfiguracaoFinanceiro : Repositorio<ConfiguracaoFinanceiro, byte>
    {
        public RepositorioConfiguracaoFinanceiro(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(ConfiguracaoFinanceiro configuracaoFinanceiro)
        {
            Sessao.SaveOrUpdate(configuracaoFinanceiro);
            Sessao.Flush();
        }

        public ConfiguracaoFinanceiro BuscarUnico()
        {
            return GetPeloId(1);
        }

        public ConfiguracaoFinanceiro BuscarParaSincronizar(DateTime ultimaSincronizacao)
        {
            var queryOver = Sessao.QueryOver<ConfiguracaoFinanceiro>();

            var configuracaoFinanceiro = queryOver.Where(cf => cf.AlteradoEm >= ultimaSincronizacao)
                .SingleOrDefault<ConfiguracaoFinanceiro>();

            return configuracaoFinanceiro;
        }
    }
}