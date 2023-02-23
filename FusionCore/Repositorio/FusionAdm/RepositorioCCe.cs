using System.Collections.Generic;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Fiscal.NF.CCe;
using FusionCore.Repositorio.Contratos;
using NHibernate;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioCCe : Repositorio<CartaCorrecaoNfe, int>, IRepositorioCCe
    {
        public RepositorioCCe(ISession sessao) : base(sessao)
        {
        }

        public void Persistir(CartaCorrecaoNfe cce)
        {
            Sessao.Persist(cce);
            Sessao.Flush();
        }

        public void Alterar(CartaCorrecaoNfe cce)
        {
            Sessao.Update(cce);
            Sessao.Flush();
        }

        public IList<CartaCorrecaoNfe> BuscaPelaNfe(Nfeletronica nfe)
        {
            var query = Sessao.QueryOver<CartaCorrecaoNfe>()
                .Where(c => c.Nfe == nfe);

            return query.OrderBy(x => x.SequenciaEvento).Desc.List<CartaCorrecaoNfe>();
        }
    }
}