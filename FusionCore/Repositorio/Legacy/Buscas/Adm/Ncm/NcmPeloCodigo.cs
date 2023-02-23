using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Ncm
{
    public class NcmPeloCodigo : IBuscaUnico<NcmDTO>
    {
        private readonly string _codigoNcm;

        public NcmPeloCodigo(string codigoNcm)
        {
            _codigoNcm = codigoNcm;
        }

        public NcmDTO Busca(ISession sessao)
        {
            var query = sessao.Query<NcmDTO>().Where(n => n.Id == _codigoNcm);
            return (NcmDTO) query.FirstOrNull();
        }
    }
}