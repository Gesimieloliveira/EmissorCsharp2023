using System.Collections;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using WpfControls.Editors;

namespace Fusion.Controles.Providers
{
    public class ProdutoSuggestionProvider : ISuggestionProvider
    {
        public IEnumerable GetSuggestions(string filter)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioProduto(sessao);
                return repositorio.BuscaRapida(filter, 50);
            }
        }
    }
}