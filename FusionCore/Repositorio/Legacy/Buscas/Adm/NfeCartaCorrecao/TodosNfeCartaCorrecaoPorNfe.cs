namespace FusionCore.Repositorio.Legacy.Buscas.Adm.NfeCartaCorrecao
{
//    public class TodosNfeCartaCorrecaoPorNfe : IBuscaListagem<object>
//    {
//        private readonly int _nfeId;
//
//        public TodosNfeCartaCorrecaoPorNfe(int nfeId)
//        {
//            _nfeId = nfeId;
//        }
//
//        public IList<NfeCartaCorrecaoDTO> Busca(ISession sessao)
//        {
//            var hql = new StringBuilder();
//            hql.Append("SELECT c.Id AS cId, c.Correcao AS cCorrecao, c.OcorreuEm AS cData, c.SequenciaEvento as SeqEvento");
//            hql.Append(" FROM NfeCartaCorrecaoDTO AS c");
//            hql.Append(" INNER JOIN c.NfeId nfe");
//            hql.Append(" WHERE nfe.Id = :nfeId");
//
//            var objetos = sessao.CreateQuery(hql.ToString())
//                .SetInt32("nfeId", _nfeId)
//                .SetResultTransformer(Transformers.AliasToEntityMap)
//                .List();
//
//            var lista = (from Hashtable map in objetos
//                select new NfeCartaCorrecaoDTO
//                {
//                    Id = Convert.ToInt32(map["cId"]),
//                    Correcao = map["cCorrecao"].ToString(),
//                    OcorreuEm = Convert.ToDateTime(map["cData"]),
//                    SequenciaEvento = Convert.ToByte(map["SeqEvento"])
//                }
//                ).ToList();
//
//            return lista;
//        }
//    }
}