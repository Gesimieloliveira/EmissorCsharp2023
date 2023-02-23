using System.Collections.Generic;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Tributacoes.Estadual;

namespace FusionCore.RecipienteDados.Adm.Impl
{
    public class RecipienteCsosn : Recipiente
    {
        private static IList<TributacaoCsosn> _cache;
        public override bool ManterCache => true;

        public override void RecarregaCache()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioTributacao(sessao);
                _cache = repositorio.TodasTributacoesCsosn();
            }
        }

        public IList<TributacaoCsosn> GetTodos()
        {
            return _cache;
        }
    }
}
