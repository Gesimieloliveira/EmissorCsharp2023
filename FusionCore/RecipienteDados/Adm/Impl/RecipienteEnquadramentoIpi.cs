using System.Collections.Generic;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Tributacoes.Federal;

namespace FusionCore.RecipienteDados.Adm.Impl
{
    public class RecipienteEnquadramentoIpi : Recipiente
    {
        private IList<EquadramentoIpi> _cache;
        public override bool ManterCache => true;

        public override void RecarregaCache()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioTributacao(sessao);
                _cache = repositorio.TodosEnquadramentoIpi();
            }
        }

        public IEnumerable<EquadramentoIpi> GetTodos()
        {
            return _cache;
        }
    }
}