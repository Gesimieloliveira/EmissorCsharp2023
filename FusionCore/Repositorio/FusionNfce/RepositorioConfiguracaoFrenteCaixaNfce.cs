using FusionCore.FusionNfce.Configuracoes;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioConfiguracaoFrenteCaixaNfce : Repositorio<ConfiguracaoFrenteCaixaNfce, byte>, IRepositorioConfiguracaoFrenteCaixaNfce
    {
        public RepositorioConfiguracaoFrenteCaixaNfce(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(ConfiguracaoFrenteCaixaNfce configuracaoFrenteCaixa)
        {
            Sessao.SaveOrUpdate(configuracaoFrenteCaixa);
        }

        public ConfiguracaoFrenteCaixaNfce BuscarUnico()
        {
            return GetPeloId(1);
        }
    }
}