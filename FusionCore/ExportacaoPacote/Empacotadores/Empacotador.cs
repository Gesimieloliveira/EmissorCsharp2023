using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace FusionCore.ExportacaoPacote.Empacotadores
{
    public class Empacotador : IEmpacotador
    {
        private IList<IEnvelope> _envelopes;

        public void ComEnvelopes(IList<IEnvelope> envelopes)
        {
            if (envelopes == null || !envelopes.Any())
                throw new ArgumentException(@"Lista de xml está vazia", nameof(envelopes));

            _envelopes = envelopes;
        }

        public FileInfo GeraPacote()
        {
            var arquivoZip = Path.GetTempFileName().Replace(".tmp", ".zip");

            using (var stream = new FileStream(arquivoZip, FileMode.OpenOrCreate))
            {
                using (var pacote = new ZipArchive(stream, ZipArchiveMode.Update))
                {
                    foreach (var envelope in _envelopes)
                    {
                        var entryName = Path.Combine(envelope.Grupo, envelope.Nome);
                        var entry = pacote.CreateEntry(entryName, CompressionLevel.Optimal);

                        using (var writer = new StreamWriter(entry.Open()))
                        {
                            writer.Write(envelope.Conteudo);
                        }
                    }
                }
            }

            return new FileInfo(arquivoZip);
        }
    }
}