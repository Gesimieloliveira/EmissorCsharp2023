using System;

namespace FusionLibrary.Helper.Diversos
{
    public static class DateTimeHelper
    {
        public static DateTime FixaEmZeroHoras(this DateTime valor)
        {
            return new DateTime(valor.Year, valor.Month, valor.Day);
        }

        public static DateTime PrimeiroDiaDoMesAtual(this DateTime valor)
        {
            var firstDayOfMonth = new DateTime(valor.Year, valor.Month, 1, 0, 0, 0);

            return firstDayOfMonth;
        }

        public static DateTime UltimoDiaDoMesAtual(this DateTime valor)
        {
            var lastDayOfMonth = valor.PrimeiroDiaDoMesAtual().AddMonths(1).AddDays(-1);

            return lastDayOfMonth;
        }
    }
}