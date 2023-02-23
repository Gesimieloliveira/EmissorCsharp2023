using System.Text.RegularExpressions;

namespace FusionCore.Core.Formatadores
{
    public static class FormatadorTelefone
    {
        private static readonly Regex _regex10 = new Regex("([0-9]{2})([0-9]{4})([0-9]{4})");
        private static readonly Regex _regex11 = new Regex("([0-9]{2})([0-9]{5})([0-9]{4})");

        public static string FormataTelefone(this string telefone)
        {
            if (telefone?.Length == 10)
            {
                return _regex10.Replace(telefone, "($1) $2-$3");
            }

            if (telefone?.Length == 11)
            {
                return _regex11.Replace(telefone, "($1) $2-$3");
            }

            return telefone;
        }
    }
}