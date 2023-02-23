using FusionCore.FusionNfce.Fiscal;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioNfceItem : Repositorio<NfceItem, int>, IRepositorioNfceItem
    {
        public RepositorioNfceItem(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(NfceItem item)
        {
            Sessao.SaveOrUpdate(item);
        }
    }
}