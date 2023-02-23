using System.Collections.Generic;
using Fusion.FastReport.DataSources;
using FusionCore.Repositorio.Filtros;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace Fusion.FastReport.Repositorios
{
    public class RepositorioInutilizacao : Repositorio
    {
        private readonly NfeInutilizacaoNumeracaoDTO _tbInutilizacao = null;
        private readonly EmpresaDTO _tbEmpresa = null;
        private readonly DsInutilizacao _dsInutilizcao;

        public RepositorioInutilizacao(IStatelessSession sessao) : base(sessao)
        {
        }

        public IList<DsInutilizacao> ListarInutilizacoes(FiltroPeriodo periodo)
        {
            var subQueryEmpresa = QueryOver.Of(() => _tbEmpresa)
                .Select(e => e.RazaoSocial)
                .Where(e => e.Cnpj == _tbInutilizacao.CnpjEmitente)
                .Take(1);

            var query = Sessao.QueryOver(() => _tbInutilizacao)
                .SelectList(list => list
                    .Select(() => _tbInutilizacao.ModeloDocumento).WithAlias(() => _dsInutilizcao.Modelo)
                    .Select(() => _tbInutilizacao.Serie).WithAlias(() => _dsInutilizcao.Serie)
                    .Select(() => _tbInutilizacao.NumeroInicial).WithAlias(() => _dsInutilizcao.NumeroInicial)
                    .Select(() => _tbInutilizacao.NumeroFinal).WithAlias(() => _dsInutilizcao.NumeroFinal)
                    .Select(() => _tbInutilizacao.Protocolo).WithAlias(() => _dsInutilizcao.Protocolo)
                    .Select(() => _tbInutilizacao.InutilizacaoEm).WithAlias(() => _dsInutilizcao.InutilizadoEm)
                    .Select(() => _tbInutilizacao.CnpjEmitente).WithAlias(() => _dsInutilizcao.EmpesaCnpj)
                    .SelectSubQuery(subQueryEmpresa).WithAlias(() => _dsInutilizcao.EmpresaRazao)
                );

            query.Where(periodo.Restriction(Projections.Property(() => _tbInutilizacao.InutilizacaoEm)));
            query.TransformUsing(Transformers.AliasToBean(typeof(DsInutilizacao)));

            return query.List<DsInutilizacao>();
        }
    }
}