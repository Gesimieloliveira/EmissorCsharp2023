using FusionCore.FusionNfce.Financeiro;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioConfiguracaoFinanceiroNfce : Repositorio<ConfiguracaoFinanceiroNfce, byte>, IRepositorioConfiguracaoFinanceiroNfce
    {
        public RepositorioConfiguracaoFinanceiroNfce(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(ConfiguracaoFinanceiroNfce configuracaoFinanceiro)
        {
            Sessao.SaveOrUpdate(configuracaoFinanceiro);
        }

        public ConfiguracaoFinanceiroNfce BuscarUnico()
        {
            return GetPeloId(1);
        }
    }
}