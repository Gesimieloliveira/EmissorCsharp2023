using System;
using FusionCore.FusionAdm.Configuracoes;
using FusionCore.Repositorio.Contratos;
using NHibernate;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioConfiguracaoFrenteCaixa : Repositorio<ConfiguracaoFrenteCaixa, byte>, IRepositorioConfiguracaoFrenteCaixa
    {
        public RepositorioConfiguracaoFrenteCaixa(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(ConfiguracaoFrenteCaixa configuracaoFrenteCaixa)
        {
            configuracaoFrenteCaixa.AlteradoEm = DateTime.Now;
            Sessao.SaveOrUpdate(configuracaoFrenteCaixa);

            Flush();
        }

        public ConfiguracaoFrenteCaixa BuscarUnica()
        {
            return GetPeloId(1);
        }

        public ConfiguracaoFrenteCaixa BuscarUnicaParaSincronizacao(DateTime ultimaSincronizacao)
        {
            var queryOver = Sessao.QueryOver<ConfiguracaoFrenteCaixa>().Where(cfc => cfc.AlteradoEm > ultimaSincronizacao);
            var configuracao = queryOver.SingleOrDefault<ConfiguracaoFrenteCaixa>();

            return configuracao;
        }
    }
}