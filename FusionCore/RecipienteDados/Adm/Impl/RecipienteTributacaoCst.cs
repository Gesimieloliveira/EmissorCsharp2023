using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Tributacoes.Estadual;
using FusionCore.Tributacoes.Repositorio;

namespace FusionCore.RecipienteDados.Adm.Impl
{
    public class RecipienteTributacaoCst : Recipiente
    {
        private static IList<TributacaoCst> _cache;

        public override bool ManterCache => true;

        public override void RecarregaCache()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioTributacaoCst(sessao);
                _cache = repositorio.BuscaTodos();
            }
        }

        public IList<TributacaoCst> GetTodos()
        {
            return _cache;
        }

        public TributacaoCst Get(string cst)
        {
            return _cache.FirstOrDefault(i => i.Id == cst);
        }
    }
}