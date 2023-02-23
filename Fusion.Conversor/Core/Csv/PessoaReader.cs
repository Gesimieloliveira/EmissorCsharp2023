using System;
using System.Collections.Generic;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Fusion.Conversor.Core.Map;

namespace Fusion.Conversor.Core.Csv
{
    public class PessoaReader
    {
        private readonly CsvFile _csvFile;

        public PessoaReader(CsvFile csvFile)
        {
            _csvFile = csvFile;
        }

        public IEnumerable<PessoaCsv> ReadAll()
        {
            var records = new List<PessoaCsv>();

            var cfg = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ";" };

            cfg.RegisterClassMap(typeof(PessoaCsvMap));
            cfg.PrepareHeaderForMatch = (header, index) => header.ToLower();
            cfg.TrimOptions = TrimOptions.Trim;
            cfg.MissingFieldFound = null;

            using (var reader = _csvFile.GetStream())
            using (var csv = new CsvReader(reader, cfg))
            {
                try
                {
                    csv.Read();
                    csv.ReadHeader();

                    while (csv.Read())
                    {
                        records.Add(csv.GetRecord<PessoaCsv>());
                    }
                }
                catch (Exception e)
                {
                    throw new Exception($"Erro no registro: {csv.Context.Field}", e);
                }
            }

            return records;
        }
    }
}