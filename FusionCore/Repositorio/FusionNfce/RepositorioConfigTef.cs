using FusionCore.FusionNfce.Tef;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioConfigTef : Repositorio<ConfigTef, int>
    {
        public RepositorioConfigTef(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(ConfigTef configTef)
        {
            Sessao.SaveOrUpdate(configTef);
        }

        public ConfigTef BuscarConfiguracaoTef()
        {
            return Sessao.QueryOver<ConfigTef>().Where(config => config.Id == 1).SingleOrDefault<ConfigTef>();
        }
    }
}