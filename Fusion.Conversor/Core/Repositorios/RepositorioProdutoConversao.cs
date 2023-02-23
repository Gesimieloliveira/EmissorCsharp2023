using System.Collections.Generic;
using FusionCore.Core.Flags;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Tributacoes.Federal;
using FusionCore.Tributacoes.Regras;
using NHibernate;

namespace Fusion.Conversor.Core.Repositorios
{
    public class RepositorioProdutoConversao
    {
        private readonly IStatelessSession _session;

        public RepositorioProdutoConversao(IStatelessSession session)
        {
            _session = session;
        }

        public IList<ProdutoUnidadeDTO> Unidades()
        {
            var query = _session.QueryOver<ProdutoUnidadeDTO>();
            return query.List();
        }

        public IList<RegraTributacaoSaida> RegrasDeSaida()
        {
            var query = _session.QueryOver<RegraTributacaoSaida>();
            return query.List();
        }

        public IList<TributacaoIpi> TributacoesSaidaIpi()
        {
            var query = _session.QueryOver<TributacaoIpi>()
                .Where(i => i.TipoOperacao == TipoOperacao.Saida);

            return query.List();
        }

        public IList<TributacaoPis> TributacoesSaidaPis()
        {
            var query = _session.QueryOver<TributacaoPis>()
                .Where(i => i.TipoOperacao == TipoOperacao.Saida);

            return query.List();
        }

        public IList<TributacaoCofins> TributacoesSaidaCofins()
        {
            var query = _session.QueryOver<TributacaoCofins>()
                .Where(i => i.TipoOperacao == TipoOperacao.Saida);
            
            return query.List();
        }

        public IList<ProdutoGrupoDTO> Grupos()
        {
            var query = _session.QueryOver<ProdutoGrupoDTO>();

            return query.List();
        }

        public IList<NcmDTO> Ncms()
        {
            var query = _session.QueryOver<NcmDTO>();

            return query.List();
        }

        public IList<EquadramentoIpi> EnqudramentosIpi()
        {
            var query = _session.QueryOver<EquadramentoIpi>();

            return query.List();
        }
    }
}