using System.Collections.Generic;
using System.Text;
using FusionCore.FusionAdm.Emissores;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using NHibernate;
using NHibernate.Transform;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Emissores
{
    public class TodosEmissorFiscalComboBox : IBuscaListagem<EmissorFiscal>
    {
        public IList<EmissorFiscal> Busca(ISession sessao)
        {
            var hql = new StringBuilder("SELECT e.Id AS Id, e.Descricao AS Descricao ");
            hql.Append("FROM " + nameof(EmissorFiscal) + " e WHERE (e.FlagNfce = true OR e.FlagSat = true) AND e.IsFaturamento = false");

            var listaObjetos = sessao.CreateQuery(hql.ToString())
                .SetResultTransformer(Transformers.AliasToBean(typeof(EmissorFiscal)))
                .List<EmissorFiscal>();

            return listaObjetos;
        }
    }
}
