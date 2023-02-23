using System.Collections.Generic;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;

namespace FusionCore.Facades
{
    public class CentroLucroFacade
    {
        private readonly ISessaoManager _sessaoManager;

        public CentroLucroFacade(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public IList<CentroLucro> Listar()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioCentroLucro(sessao);
                return repositorio.BuscaTodos();
            }
        }
    }
}