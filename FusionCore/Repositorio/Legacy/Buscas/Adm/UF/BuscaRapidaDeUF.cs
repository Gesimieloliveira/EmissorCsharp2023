using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.UF
{
    public class BuscaRapidaDeUF : IBuscaListagem<EstadoDTO>
    {
        private readonly string _filtro;

        public BuscaRapidaDeUF(string filtro)
        {
            _filtro = filtro ?? string.Empty;
        }

        public IList<EstadoDTO> Busca(ISession sessao)
        {
            var query = sessao.Query<EstadoDTO>()
                .Where(uf => uf.CodigoIbge.ToString() == _filtro
                             || uf.Nome.Contains(_filtro)
                             || uf.Sigla == _filtro);

            return query.ToList();
        }
    }
}