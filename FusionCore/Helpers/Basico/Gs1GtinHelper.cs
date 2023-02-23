using System.Linq;

namespace FusionCore.Helpers.Basico
{
    public static class Gs1GtinHelper
    {
        private static readonly int[] Multiplos = {3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3};
        private static readonly int[] TamanhosPermitidos = { 8, 12, 13, 14, 17, 18};

        public static bool EhUmGtinValido(string gtin)
        {
            if (string.IsNullOrEmpty(gtin) || TamanhosPermitidos.All(i => i != gtin.Length))
            {
                return false;
            }

            var gtinLen = gtin.Length;
            var gtinSemDigito = gtin.Substring(0, gtinLen - 1);
            var gtinDigito = gtin.Substring(gtinLen - 1, 1);

            var soma = 0;
            var multiplos = Multiplos.Skip(Multiplos.Length - gtinSemDigito.Length).ToArray();

            for (var i = 0; i < gtinSemDigito.Length; i++)
            {
                if (!int.TryParse(gtinSemDigito[i].ToString(), out var cint))
                {
                    return false;
                }

                soma += multiplos[i] * cint;
            }

            var mod = soma % 10;
            var digitoReal = mod == 0 ? 0 : 10 - mod;

            return digitoReal.ToString() == gtinDigito;
        }

        public static bool EhUmGtinDoBrasilValido(string gtin)
        {
            if (EhUmGtinValido(gtin) == false) return false;

            var paisOrigemBrasil = gtin.Substring(0, 3);
            var paisOrigemBrasil2 = gtin.Substring(0, 3);

            return paisOrigemBrasil == "789" || paisOrigemBrasil2 == "790";
        }
    }
}