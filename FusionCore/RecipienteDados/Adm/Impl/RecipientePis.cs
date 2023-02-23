using System.Collections.Generic;
using System.Linq;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Tributacoes.Federal;

namespace FusionCore.RecipienteDados.Adm.Impl
{
    public class RecipientePis : Recipiente
    {
        private static IList<TributacaoPis> _cache;
        public override bool ManterCache => true;

        public override void RecarregaCache()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioTributacao(sessao);
                _cache = repositorio.TodasTributacoesPis();
            }
        }

        public IEnumerable<TributacaoPis> GetTodos()
        {
            return _cache;
        }

        public IEnumerable<TributacaoPis> GetPorOperacao(TipoOperacao tipo)
        {
            return _cache.Where(l => l.TipoOperacao == tipo).ToList();
        }

        public TributacaoPis Get(string cst)
        {
            return _cache.FirstOrDefault(p => p.Id == cst);
        }
    }
}
