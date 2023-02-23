using System.Collections.Generic;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using NHibernate;
using EmissorClasse = FusionCore.FusionAdm.Emissores.EmissorFiscal;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Emissores
{
    public class TodosEmissorFiscal : IBuscaListagem<EmissorClasse>
    {
        public IList<EmissorClasse> Busca(ISession sessao)
        {
            var query = sessao.QueryOver<EmissorClasse>();

            return query.List();
        }
    }
}