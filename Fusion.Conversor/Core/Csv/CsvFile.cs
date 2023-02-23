using System.IO;
using System.Text;
using FusionCore.TextEncoding;

namespace Fusion.Conversor.Core.Csv
{
    public class CsvFile
    {
        private readonly string _path;
        private readonly Encoding _encoding;

        public CsvFile(string path, TipoEncoding encoding)
        {
            _path = path;
            _encoding = encoding.ToSystemEncoding();
        }

        public StreamReader GetStream()
        {
            var fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(fileStream, _encoding);

            return sr;
        }
    }
}