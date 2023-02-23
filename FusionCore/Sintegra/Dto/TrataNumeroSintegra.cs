using FusionCore.Helpers.Hidratacao;

namespace FusionCore.Sintegra.Dto
{
    public static class TrataNumeroSintegra
    {
        public static string Trata(string numero)
        {
            if (numero.IsNullOrEmpty()) return numero;

            if (numero.Length > 6)
            {
                var ultimosSeisNumeros = numero.Remove(0, numero.Length - 6);

                return ultimosSeisNumeros;
            }

            return numero;
        }

        public static int Trata(int numero)
        {
            return int.Parse(Trata(numero.ToString()));
        }
    }
}