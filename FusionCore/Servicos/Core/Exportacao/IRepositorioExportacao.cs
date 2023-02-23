using System;
using System.Collections.Generic;

namespace FusionCore.Servicos.Core.Exportacao
{
    public interface IRepositorioExportacao : IDisposable
    {
        IList<DocumentoXml> ListarDocumentosNaoExportados();
        void SalvarExportadas(IEnumerable<DocumentoXml> exportados);
    }
}