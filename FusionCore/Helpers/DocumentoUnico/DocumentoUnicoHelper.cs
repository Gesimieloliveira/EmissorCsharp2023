using System.Text.RegularExpressions;

namespace FusionCore.Helpers.DocumentoUnico
{
    public class DocumentoUnicoHelper
    {
        private static readonly Regex Regex = new Regex("[^0-9]");

        public string PreparaString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            var output = Regex.Replace(input, "");

            return output;
        }

        public bool TamanhoCpf(string input)
        {
            return PreparaString(input).Length == 11;
        }

        public bool TamanhoCnpj(string input)
        {
            return PreparaString(input).Length == 14;
        }
    }
}