using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Usuario
{
    public class TodosUsuarios : IBuscaListagem<UsuarioDTO>
    {
        public IList<UsuarioDTO> Busca(ISession sessao)
        {
            var query = sessao.Query<UsuarioDTO>();
            return query.ToList();
        }
    }
}