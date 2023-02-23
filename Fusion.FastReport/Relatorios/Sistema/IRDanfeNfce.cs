using System;
using System.IO;

namespace Fusion.FastReport.Relatorios.Sistema
{
    public interface IRDanfeNfce : IDisposable
    {
        void SegundaViaContingencia(bool segundaViaContingencia);
        void ForcarSegundaVia(bool forcarSegundaVia);
        void ComXml(string xmlstring, bool cancelado, byte[] logoMarca, string nomeFantasiaCustomizado);
        void Imprimir(string printer, int? quantidadeCopia = null);
        void Visualizar();
        void ExportarPdf(Stream stream);
    }
}