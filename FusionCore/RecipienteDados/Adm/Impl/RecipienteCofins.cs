using System.Collections.Generic;
using System.Linq;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Tributacoes.Federal;

namespace FusionCore.RecipienteDados.Adm.Impl
{
    public class RecipienteCofins : Recipiente
    {
        private static IList<TributacaoCofins> _cache;
        public override bool ManterCache => true;

        public override void RecarregaCache()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioTributacao(sessao);
                _cache = repositorio.TodasTributacoesCofins();
            }
        }

        public IList<TributacaoCofins> GetTodos()
        {
            return _cache;
        }

        public IEnumerable<TributacaoCofins> GetPorOperacao(TipoOperacao tipo)
        {
            return _cache.Where(l => l.TipoOperacao == tipo).ToList();
        }

        public TributacaoCofins Get(string codigo)
        {
            return _cache.FirstOrDefault(p => p.Id == codigo);
        }
    }
}
