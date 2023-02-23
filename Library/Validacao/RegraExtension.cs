using FusionLibrary.Validacao.Regras;

namespace FusionLibrary.Validacao
{
    public static class RegraExtension
    {
        public static bool Valido(this IRegra regra, object value)
        {
            return regra.AplicaRegra(value);
        }

        public static bool NaoValido(this IRegra regra, object value)
        {
            return !regra.AplicaRegra(value);
        }
    }
}