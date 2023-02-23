using FusionCore.FusionPdv.Financeiro;
using FusionCore.Repositorio.Contratos.FusionPdvContratos;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionPdv
{
    public class RepositorioConfiguracaoFinanceiroPdv : Repositorio<ConfiguracaoFinanceiroPdv, byte>, IRepositorioConfiguracaoFinanceiroPdv
    {
        public RepositorioConfiguracaoFinanceiroPdv(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(ConfiguracaoFinanceiroPdv configuracaoFinanceiro)
        {
            Sessao.SaveOrUpdate(configuracaoFinanceiro);
        }

        public ConfiguracaoFinanceiroPdv BuscarUnico()
        {
            return GetPeloId(1);
        }
    }
}