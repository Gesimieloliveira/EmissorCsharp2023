using System.Text.RegularExpressions;

namespace FusionCore.Core.Formatadores
{
    public static class FormatadorDocumento
    {
        public static string ToFormadoCnpj(this string cnpj)
        {
            return Regex.Replace(cnpj, "([0-9]{2})([0-9]{3})([0-9]{3})([0-9]{4})([0-9]{2})", "$1.$2.$3/$4-$5");
        }

        public static string ToFormatoCpf(this string cpf)
        {
            return Regex.Replace(cpf, "([0-9]{3})([0-9]{3})([0-9]{3})([0-9]{2})", "$1.$2.$3-$4");
        }
    }
}