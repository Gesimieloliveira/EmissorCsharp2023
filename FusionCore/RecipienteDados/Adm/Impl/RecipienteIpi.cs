using System.Collections.Generic;
using System.Linq;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Tributacoes.Federal;

namespace FusionCore.RecipienteDados.Adm.Impl
{
    public class RecipienteIpi : Recipiente
    {
        private static IList<TributacaoIpi> _cache;
        public override bool ManterCache => true;

        public override void RecarregaCache()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioTributacao(sessao);
                _cache = repositorio.TodasTributacoesIpi();
            }
        }

        public IEnumerable<TributacaoIpi> GetTodos()
        {
            return _cache;
        }

        public IEnumerable<TributacaoIpi> GetPorOperacao(TipoOperacao tipo)
        {
            return _cache.Where(p => p.TipoOperacao == tipo).ToList();
        }

        public TributacaoIpi Get(string cst)
        {
            return _cache.FirstOrDefault(p => p.Codigo == cst);
        }
    }
}