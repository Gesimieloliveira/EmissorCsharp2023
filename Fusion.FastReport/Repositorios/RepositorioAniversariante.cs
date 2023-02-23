using System.Collections.Generic;
using System.Text;
using Fusion.FastReport.DataSources;
using FusionCore.Repositorio.Filtros;
using FusionCore.Repositorio.Legacy.Flags.Pessoa;
using NHibernate;
using NHibernate.Transform;

namespace Fusion.FastReport.Repositorios
{
    public class RepositorioAniversariante : Repositorio
    {
        public RepositorioAniversariante(IStatelessSession sessao) : base(sessao)
        {
        }

        public IList<DsAniversariante> BuscaAniversariantes(FiltroPeriodoNascimento periodo)
        {
            var sql = new StringBuilder();

            sql.AppendLine("select pe.id as Id, pe.nome as Nome, pe.nascidoEm as DataNascimento, tel.numero as PrimeiroTelefone");
            sql.AppendLine("from pessoa pe");
            sql.AppendLine("outer apply (select top 1 otel.numero from pessoa_telefone otel where otel.pessoa_id = pe.id) as tel");
            sql.AppendLine("where pe.nascidoEm is not null and pe.tipo = :tipo and month(pe.nascidoEm) between :mInicio and :mFim");
            sql.AppendLine("order by pe.nascidoEm");

            var query = Sessao.CreateSQLQuery(sql.ToString())
                .SetString("tipo", PessoaTipo.Fisica)
                .SetInt32("mInicio", periodo.MesInicio)
                .SetInt32("mFim", periodo.MesFinal);

            query.SetResultTransformer(Transformers.AliasToBean<DsAniversariante>());

            return query.List<DsAniversariante>();
        }
    }
}