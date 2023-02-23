using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Tributacoes.Estadual;

namespace FusionCore.RecipienteDados.Adm.Impl
{
    public class RecipienteIcmsCTeOs : Recipiente
    {
        private static IList<TributacaoIcms> _cache;
        public override bool ManterCache => true;

        public override void RecarregaCache()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioTributacao(sessao);
                _cache = repositorio.TodasTributacoesIcmsCTeOs();
            }
        }

        public IList<TributacaoIcms> GetTodos()
        {
            return _cache;
        }

        public TributacaoIcms Get(string cst)
        {
            return _cache.FirstOrDefault(i => i.Codigo == cst);
        }
    }
}