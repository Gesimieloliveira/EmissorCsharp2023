using FusionCore.FusionPdv.Configuracoes;
using FusionCore.Repositorio.Contratos.FusionPdvContratos;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionPdv
{
    public class RepositorioBalancaPdv : Repositorio<BalancaPdv, byte>, IRepositorioBalancaPdv
    {
        public RepositorioBalancaPdv(ISession sessao) : base(sessao)
        {
        }


        public void Salvar(BalancaPdv balancaPdv)
        {
            Sessao.SaveOrUpdate(balancaPdv);
        }

        public BalancaPdv BuscaUnicaConfiguracao()
        {
            return GetPeloId(1);
        }
    }
}