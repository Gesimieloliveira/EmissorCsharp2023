using System.Collections.Generic;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.PerfilCfop
{
    public class TodosPerfilCfop : IBuscaListagem<PerfilCfopDTO>
    {
        public IList<PerfilCfopDTO> Busca(ISession sessao)
        {
            var queryOver = sessao.QueryOver<PerfilCfopDTO>();

            return queryOver.List();
        }
    }
}
