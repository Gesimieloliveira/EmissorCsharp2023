using FusionCore.FusionNfce.ConfiguracaoBalanca;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioBalancaNfce : Repositorio<BalancaNfce, byte>
    {
        public RepositorioBalancaNfce(ISession sessao) : base(sessao)
        {
        }

        public void SalvarOuAtualizar(BalancaNfce balanca)
        {
            Sessao.SaveOrUpdate(balanca);
        }

        public BalancaNfce BuscarUnicaBalanca()
        {
            const byte id = 1;

            return Sessao.Get<BalancaNfce>(id);
        }
    }
}