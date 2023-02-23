using System.Collections.Generic;
using FusionCore.FusionAdm.Financeiro;

namespace FusionCore.Repositorio.Contratos
{
    public interface IRepositorioDocumentoPagar : IRepositorio<DocumentoPagar, int>
    {
        void Salvar(DocumentoPagar documentoPagar);
        IList<DocumentoPagar> BuscaRapida(string texto);
        void SalvarLancamento(DocumentoPagarLancamento documentoPagarLancamento);
    }
}