using System.Collections.Generic;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;

namespace FusionCore.Core.Tributario
{
    public class CfopFacade
    {
        private readonly ISessaoManager _sessaoManager;

        public CfopFacade(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public CfopDTO BuscarPeloCodigo(string codigo)
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioCfop(sessao);

                return repositorio.GetPeloId(codigo);
            }
        }

        public IEnumerable<CfopDTO> BuscarTodosDeEntrada()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioCfop(sessao);
                var todos = repositorio.BuscarApenasOsDeEntrada();

                return todos;
            }
        }
    }
}