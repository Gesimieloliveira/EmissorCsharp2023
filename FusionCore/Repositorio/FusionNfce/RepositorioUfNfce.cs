using FusionCore.FusionNfce.Uf;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioUfNfce : Repositorio<UfNfce, byte>, IRepositorioUfNfce
    {
        public RepositorioUfNfce(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(UfNfce uf)
        {
            Sessao.SaveOrUpdate(uf);
        }
    }
}