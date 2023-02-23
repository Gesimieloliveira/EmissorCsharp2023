using System.Collections.Generic;
using System.Linq;

namespace FusionCore.Extencoes
{
    public static class ExtValidaListasEColecoes
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> lista)
        {
            return lista == null || !lista.Any();
        }

        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> lista)
        {
            return !lista.IsNullOrEmpty();
        }
    }
}