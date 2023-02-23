using System.Text.RegularExpressions;

namespace Fusion.Conversor.Core.Helpers
{
    public class StringPreparer
    {
        private static readonly Regex ApenasNumeroRegex = new Regex("[^0-9]", RegexOptions.IgnoreCase);

        public string Prepare(string input, int maxLength)
        {
            input = Prepare(input);

            return input.Length > maxLength 
                ? input.Substring(0, maxLength) 
                : input;
        }

        public string Prepare(string input)
        {
            if (string.IsNullOrEmpty(input) || input.ToLower() == "null")
            {
                return string.Empty; ;
            }

            return input;
        }

        public bool IsValid(string input)
        {
            return !string.IsNullOrEmpty(input) && input.ToLower() != "null";
        }

        public string RemoveNaoNumeros(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return ApenasNumeroRegex.Replace(input, "");
        }
    }
}