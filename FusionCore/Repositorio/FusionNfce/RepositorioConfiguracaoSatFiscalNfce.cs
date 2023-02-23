using FusionCore.FusionNfce.ConfiguracaoSat;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioConfiguracaoSatFiscalNfce : Repositorio<ConfiguracaoSatFiscal, int>, IRepositorioConfiguracaoSatFiscalNfce
    {
        public RepositorioConfiguracaoSatFiscalNfce(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(ConfiguracaoSatFiscal configuracaoSatFiscal)
        {
            configuracaoSatFiscal.Id = 1;
            Sessao.SaveOrUpdate(configuracaoSatFiscal);
            Sessao.Flush();
        }

        public ConfiguracaoSatFiscal BuscarConfiguracao()
        {
            return Sessao.Get<ConfiguracaoSatFiscal>(1);
        }
    }
}