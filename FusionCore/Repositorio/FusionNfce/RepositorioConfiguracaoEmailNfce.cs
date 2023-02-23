using FusionCore.FusionNfce.ConfiguracaoEmail;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioConfiguracaoEmailNfce : Repositorio<ConfiguracaoEmailNfce, int>, IRepositorioConfiguracaoEmailNfce
    {
        public RepositorioConfiguracaoEmailNfce(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(ConfiguracaoEmailNfce configuracaoEmail)
        {
            Sessao.SaveOrUpdate(configuracaoEmail);
        }

        public ConfiguracaoEmailNfce BuscarUnicaConfiguracao()
        {
            var query = Sessao.QueryOver<ConfiguracaoEmailNfce>().Where(ce => ce.Id == 1);

            var configuracaoEmail = query.SingleOrDefault<ConfiguracaoEmailNfce>();

            return configuracaoEmail;
        }
    }
}