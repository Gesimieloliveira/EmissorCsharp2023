using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Ecf
{
    public class EcfPeloNumeroSerie : IBuscaUnico<PdvEcfDTO>
    {
        private readonly string _numeroSerie;

        public EcfPeloNumeroSerie(string numeroSerie)
        {
            _numeroSerie = numeroSerie;
        }

        public PdvEcfDTO Busca(ISession sessao)
        {
            var query = sessao.Query<PdvEcfDTO>()
                .Where(ecf => ecf.Serie == _numeroSerie);

            return (PdvEcfDTO) query.FirstOrNull();
        }
    }
}