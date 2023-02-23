using System.Linq;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioEstado : Repositorio<EstadoDTO, int>
    {
        public RepositorioEstado(ISession sessao) : base(sessao)
        {
        }

        public EstadoDTO GetPelaSigla(string sigla)
        {
            if (sigla == null)
                return null;

            var query = Sessao.Query<EstadoDTO>().Where(e => e.Sigla.ToUpper() == sigla.ToUpper());
            return (EstadoDTO) query.FirstOrNull();
        }

        public EstadoDTO PeloIbge(byte codigoIbge)
        {
            var query = Sessao.Query<EstadoDTO>()
                .Where(i => i.CodigoIbge == codigoIbge);

            return query.SingleOrDefault();
        }
    }
}