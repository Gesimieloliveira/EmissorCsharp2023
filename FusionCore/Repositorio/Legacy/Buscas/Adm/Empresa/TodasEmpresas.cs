using System.Collections.Generic;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Empresa
{
    public class TodasEmpresas : IBuscaListagem<EmpresaDTO>
    {
        public IList<EmpresaDTO> Busca(ISession sessao)
        {
            var queryOver = sessao.QueryOver<EmpresaDTO>();
            var lista = queryOver.List<EmpresaDTO>();
            return lista;
        }
    }
}