using System;
using System.Text.RegularExpressions;

namespace FusionLibrary.Helper.Diversos
{
    public static class StringHelper
    {
        public static string LimitarTamanho(this string input, int tamanhoMaximo)
        {
            if (input == null)
                return null;

            return input.Length > tamanhoMaximo ? input.Substring(0, tamanhoMaximo) : input;
        }

        public static bool PossuiApenasNumeros(this string input)
        {
            var regex = new Regex(@"^\d+$");
            return regex.IsMatch(input);
        }

        public static ulong ApenasNumeros(this string input)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    return 0;
                }

                var apenasNumeros = Regex.Replace(input, @"[^\d]", "");
                return apenasNumeros.Length > 0 ? Convert.ToUInt64(apenasNumeros) : 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static string ApenasNumerosString(this string input)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    return string.Empty;
                }

                var apenasNumeros = Regex.Replace(input, @"[^\d]", "");
                return apenasNumeros.Length > 0 ? apenasNumeros : string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string RemoveNaoNumericos(this string input)
        {
            return string.IsNullOrWhiteSpace(input)
                ? string.Empty
                : Regex.Replace(input, @"[^\d]", "");
        }
    }
}