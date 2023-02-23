namespace FusionLibrary.Helper.Diversos
{
    public static class FormatadorHelper
    {
        public static string FormataParaCpf(this string cadeia)
        {
            if (string.IsNullOrWhiteSpace(cadeia))
                return string.Empty;

            var inteiro = cadeia.ApenasNumeros();
            return inteiro.ToString(@"000\.000\.000-00");
        }

        public static string FormataParaCnpj(this string cadeia)
        {
            if (string.IsNullOrWhiteSpace(cadeia))
                return string.Empty;

            var inteiro = cadeia.ApenasNumeros();
            return inteiro.ToString(@"00\.000\.000\/0000\-00");
        }
    }
}