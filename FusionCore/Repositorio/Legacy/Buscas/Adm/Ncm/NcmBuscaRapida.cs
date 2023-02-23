using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Ncm
{
    public class NcmBuscaRapida : IBuscaListagem<NcmDTO>
    {
        private readonly string _filtro;

        public NcmBuscaRapida(string filtro)
        {
            _filtro = filtro ?? "";
        }

        public IList<NcmDTO> Busca(ISession sessao)
        {
            var query = sessao.Query<NcmDTO>()
                .Where(n => n.Descricao.Contains(_filtro) || n.Id == _filtro || n.Cest == _filtro);

            return query.ToList();
        }
    }
}