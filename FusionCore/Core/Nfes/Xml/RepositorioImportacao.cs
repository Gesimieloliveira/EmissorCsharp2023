using System.Collections.Generic;
using FusionCore.RecipienteDados;
using FusionCore.RecipienteDados.Adm.Impl;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Tributacoes.Estadual;
using FusionCore.Tributacoes.Federal;
using NHibernate;

namespace FusionCore.Core.Nfes.Xml
{
    public class RepositorioImportacao : IRepositorioImportacao
    {
        private readonly ISession _session;

        public RepositorioImportacao(ISession session)
        {
            _session = session;
        }

        public IEnumerable<ProdutoUnidadeDTO> GetUnidades(string sigla)
        {
            return new RepositorioProdutoUnidade(_session).GetUnidadesPelaSigla(sigla);
        }

        public CidadeDTO GetCidadePeloIbge(int codigo)
        {
            return new RepositorioCidade(_session).BuscaPeloIbge(codigo);
        }

        public ProdutoGrupoDTO GetGrupo()
        {
            return _session.QueryOver<ProdutoGrupoDTO>().Where(g => g.Id == 1).SingleOrDefault();
        }

        public TributacaoIcms GetIcms(string cst)
        {
            return RecipienteFactory.Get<RecipienteIcmsNFe>().Get(cst);
        }

        public TributacaoPis GetPis(string cst)
        {
            return RecipienteFactory.Get<RecipientePis>().Get(cst);
        }

        public TributacaoCofins GetCofins(string cst)
        {
            return RecipienteFactory.Get<RecipienteCofins>().Get(cst);
        }

        public TributacaoIpi GetIpi(string cst)
        {
            return RecipienteFactory.Get<RecipienteIpi>().Get(cst);
        }
    }
}