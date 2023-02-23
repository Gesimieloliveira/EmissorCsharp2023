using System.Text.RegularExpressions;
using FusionCore.Helpers.Hidratacao;

namespace FusionCore.FusionAdm.Fiscal.Helpers
{
    public static class StringHelper
    {
        public static string TrimSefazOrNull(this string valor, int tamanhoMaximo = 0)
        {
            valor = TrimSefaz(valor, tamanhoMaximo);

            return valor.IsNullOrEmpty() ? null : valor;
        }

        public static string TrimSefaz(this string input, int tamanhoMaximo = 0)
        {
            var inputLimpo = LimpaInput(input);

            if (string.IsNullOrWhiteSpace(inputLimpo) || tamanhoMaximo == 0)
                return inputLimpo;

            return inputLimpo.Length <= tamanhoMaximo
                ? inputLimpo
                : LimpaInput(inputLimpo.Substring(0, tamanhoMaximo));
        }

        private static string LimpaInput(string input)
        {
            if (input == null) return null;

            var limpo = input.Trim();

            var regex = new Regex("[\r\n]", RegexOptions.IgnoreCase);
            return regex.Replace(limpo, string.Empty);
        }
    }
}