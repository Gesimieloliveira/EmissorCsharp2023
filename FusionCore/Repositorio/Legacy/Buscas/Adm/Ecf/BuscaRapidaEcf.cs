using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Ecf
{
    public class BuscaRapidaEcf : IBuscaListagem<PdvEcfDTO>
    {
        private readonly string _texto;

        public BuscaRapidaEcf(string texto)
        {
            _texto = texto ?? "";
        }

        public IList<PdvEcfDTO> Busca(ISession sessao)
        {
            var query = sessao.Query<PdvEcfDTO>()
                .Where(ecf => ecf.Id.ToString() == _texto
                              || ecf.Modelo == _texto
                              || ecf.NumeroEcf == _texto
                              || ecf.Serie == _texto);

            return query.ToList();
        }
    }
}