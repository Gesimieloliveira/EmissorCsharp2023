using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.Tributacoes.Interestadual
{
    public class RepositorioPartilhaIcms
    {
        private readonly ISession _sessao;

        public RepositorioPartilhaIcms(ISession sessao)
        {
            _sessao = sessao;
        }

        public decimal ObtemAliquota(EstadoDTO origem, EstadoDTO destino)
        {
            var query = _sessao.QueryOver<AliquotaInterestadual>()
                .Where(i => i.Origem == origem && i.Destino == destino)
                .Select(i => i.Aliquota);

            return query.SingleOrDefault<decimal>();
        }
    }
}