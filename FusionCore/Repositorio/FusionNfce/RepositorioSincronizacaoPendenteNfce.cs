using System.Collections.Generic;
using System.Text;
using FusionCore.NfceSincronizador.Flags;
using FusionCore.NfceSincronizador.SincronizacaoEntidade;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;
using NHibernate.Transform;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioSincronizacaoPendenteNfce : Repositorio<SincronizacaoPendenteNfce, SincronizacaoPendenteNfce>, IRepositorioSincronizacaoPendenteNfce
    {
        public RepositorioSincronizacaoPendenteNfce(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(SincronizacaoPendenteNfce sincronizacaoPendente)
        {
            var queryDelete = Sessao.CreateSQLQuery("delete sync_pendente where referencia = :referencia" +
                                                    " and entidade = :entidade");

            queryDelete.SetString("referencia", sincronizacaoPendente.Referencia);
            queryDelete.SetParameter("entidade", sincronizacaoPendente.Entidade);

            queryDelete.ExecuteUpdate();


            var query = Sessao.CreateSQLQuery("insert into sync_pendente (referencia, entidade) " +
                                              "values (:referencia, :entidade) ");

            query.SetString("referencia", sincronizacaoPendente.Referencia);
            query.SetParameter("entidade", sincronizacaoPendente.Entidade);

            query.ExecuteUpdate();
        }

        public void Deletar(SincronizacaoPendenteNfce sincronizacaoPendente)
        {
            Sessao.Delete(sincronizacaoPendente);
        }

        public IList<SincronizacaoPendenteNfce> BuscaTodosParaSincronizacao(EntidadeSincronizavel sincronizavel)
        {
            var hql = new StringBuilder("select ");
            hql.Append("sp.Referencia as Referencia, sp.Entidade as Entidade ");
            hql.Append("from SincronizacaoPendenteNfce sp where sp.Entidade = :entidade ");

            var query = Sessao.CreateQuery(hql.ToString())
                .SetParameter("entidade", sincronizavel);

            query.SetResultTransformer(Transformers.AliasToBean(typeof(SincronizacaoPendenteNfce)));

            var todos = query.List<SincronizacaoPendenteNfce>();

            return todos;
        }
    }
}