using System.Collections.Generic;
using FusionCore.Tributacoes.Estadual;
using FusionCore.Tributacoes.Federal;
using NHibernate;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioTributacao
    {
        private readonly ISession _session;

        public RepositorioTributacao(ISession session)
        {
            _session = session;
        }

        public IList<TributacaoIcms> TodasTributacoesIcmsNFe()
        {
            return QueryTributacaoIcms().Where(x => x.IsNFe == true).List();
        }

        public IList<TributacaoIcms> TodasTributacoesIcmsCTe()
        {
            return QueryTributacaoIcms().Where(x => x.IsCTe == true).List();
        }

        public IList<TributacaoIpi> TodasTributacoesIpi()
        {
            return _session.QueryOver<TributacaoIpi>().List();
        }

        public IList<TributacaoCsosn> TodasTributacoesCsosn()
        {
            return _session.QueryOver<TributacaoCsosn>().List();
        }

        public IList<TributacaoPis> TodasTributacoesPis()
        {
            return _session.QueryOver<TributacaoPis>().List();
        }

        public IList<TributacaoCofins> TodasTributacoesCofins()
        {
            return _session.QueryOver<TributacaoCofins>().List();
        }

        private IQueryOver<TributacaoIcms, TributacaoIcms> QueryTributacaoIcms()
        {
            return _session.QueryOver<TributacaoIcms>();
        }

        public IList<TributacaoIcms> TodasTributacoesIcmsCTeOs()
        {
            return QueryTributacaoIcms().Where(x => x.IsCTeOs == true).List();
        }

        public IList<EquadramentoIpi> TodosEnquadramentoIpi()
        {
            return _session.QueryOver<EquadramentoIpi>().List();
        }

        public IList<TributacaoIcms> Todos()
        {
            return QueryTributacaoIcms().List();
        }
    }
}