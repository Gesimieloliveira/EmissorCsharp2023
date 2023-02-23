using System.Collections.Generic;
using FusionCore.FusionAdm.Financeiro.Repositorios;
using FusionCore.Repositorio.Filtros;
using FusionCore.Sessao;

namespace FusionCore.FusionAdm.Financeiro.Servicos
{
    public class ServicoListagemDocumentos
    {
        private readonly ISessaoManager _sessaoManager;

        public ServicoListagemDocumentos(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public IEnumerable<ResumoDocumentoReceberDTO> ListarDocumentos(IFiltro filtro)
        {
            using (var sessaao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioDocumentoReceber(sessaao);

                return repositorio.BuscarDocumentos(filtro);
            }
        }
    }
}