using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;

namespace FusionCore.RecipienteDados.Adm.Impl
{
    public class RecipienteTipoDocumento : Recipiente
    {
        private static IList<TipoDocumento> _cache;
        public override bool ManterCache => false;

        public override void RecarregaCache()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioTipoDocumento(sessao);
                _cache = repositorio.BuscaTodos();
            }
        }

        public IList<TipoDocumento> GetAtivos()
        {
            return _cache.Where(a => a.EstaAtivo).ToList();
        }

        public IList<TipoDocumento> GetTodos()
        {
            return _cache;
        }
    }
}
