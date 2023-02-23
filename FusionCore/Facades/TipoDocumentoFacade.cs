using System.Collections.Generic;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;

namespace FusionCore.Facades
{
    public class TipoDocumentoFacade
    {
        private readonly ISessaoManager _sessaoManager;

        public TipoDocumentoFacade(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public IList<TipoDocumento> Listar()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioTipoDocumento(sessao);
                return repositorio.BuscaTodos();
            }
        }
    }
}