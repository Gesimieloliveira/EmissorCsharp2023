using System.Collections.Generic;
using FusionCore.FusionPdv.Financeiro;

namespace FusionCore.Repositorio.Contratos.FusionPdvContratos
{
    public interface IRepositorioTipoDocumentoPdv : IRepositorio<TipoDocumentoPdv, short>
    {
        void Salvar(TipoDocumentoPdv tipoDocumento);

        IEnumerable<TipoDocumentoPdv> BuscaAtivos();
    }
}