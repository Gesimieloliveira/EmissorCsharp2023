using System.Text;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Transform;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.UF
{
    public class BuscaEstadoPorSigla : IBuscaUnico<EstadoDTO>
    {
        private readonly string _siglaUf;

        public BuscaEstadoPorSigla(string siglaUf)
        {
            _siglaUf = siglaUf;
        }

        public EstadoDTO Busca(ISession sessao)
        {
            var hql = new StringBuilder("SELECT uf.Id as Id, uf.Sigla as Sigla, ");
            hql.Append("uf.Nome as Nome, uf.CodigoIbge as CodigoIbge ");
            hql.Append("FROM EstadoDTO uf ");
            hql.Append("WHERE uf.Sigla = :sigla");

            var ufDTO = sessao.CreateQuery(hql.ToString())
                .SetParameter("sigla", _siglaUf)
                .SetResultTransformer(Transformers.AliasToBean(typeof (EstadoDTO)))
                .UniqueResult<EstadoDTO>();

            return ufDTO;
        }
    }
}
