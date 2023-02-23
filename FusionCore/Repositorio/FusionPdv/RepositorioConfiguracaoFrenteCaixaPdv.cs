using FusionCore.FusionPdv.Configuracoes;
using FusionCore.Repositorio.Contratos.FusionPdvContratos;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionPdv
{
    public class RepositorioConfiguracaoFrenteCaixaPdv : Repositorio<ConfiguracaoFrenteCaixaPdv, byte>, IRepositorioConfiguracaoFrenteCaixaPdv
    {
        public RepositorioConfiguracaoFrenteCaixaPdv(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(ConfiguracaoFrenteCaixaPdv configuracaoFrenteCaixa)
        {
            Sessao.SaveOrUpdate(configuracaoFrenteCaixa);
        }

        public ConfiguracaoFrenteCaixaPdv BuscarUnico()
        {
            return GetPeloId(1);
        }
    }
}