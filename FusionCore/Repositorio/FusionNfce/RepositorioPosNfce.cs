using System.Collections.Generic;
using FusionCore.FusionNfce.Tef;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;
using NHibernate.Criterion;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioPosNfce : Repositorio<PosNfce, short>, IRepositorioPosNfce
    {
        public RepositorioPosNfce(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(PosNfce pos)
        {
            Sessao.SaveOrUpdate(pos);
        }

        public IList<PosNfce> BuscarPosParaMFe()
        {
            PosNfce posNfce = null;

            var flagMfe = Restrictions.Eq(Projections.Property(() => posNfce.FlagMfe), true);
            var status = Restrictions.Eq(Projections.Property(() => posNfce.Status), true);

            return Sessao.QueryOver(() => posNfce).Where(flagMfe && status).List();
        }

        public IList<PosNfce> BuscarPosParaNFce()
        {
            PosNfce posNfce = null;

            var flagNfce = Restrictions.Eq(Projections.Property(() => posNfce.FlagNfce), true);
            var status = Restrictions.Eq(Projections.Property(() => posNfce.Status), true);

            return Sessao.QueryOver(() => posNfce).Where(flagNfce && status).List();
        }
    }
}