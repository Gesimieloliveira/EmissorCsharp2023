using System.Collections.Generic;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.RecipienteDados.Adm.Impl
{
    public class RecipienteUnidade : Recipiente
    {
        private static IList<ProdutoUnidadeDTO> _cache;
        public override bool ManterCache => false;

        public override void RecarregaCache()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioProdutoUnidade(sessao);
                _cache = repositorio.BuscaTodos();
            }
        }

        public IList<ProdutoUnidadeDTO> GetTodos()
        {
            return _cache;
        }
    }
}