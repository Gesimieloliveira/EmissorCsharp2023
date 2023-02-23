using System;

namespace FusionPdv.ManipulaValor
{
    public static class TruncarValor
    {
        public static decimal Trunca(decimal valorReal)
        {
            var valor = valorReal;
                valor *= 100;
                valor = Math.Truncate(valor);
                valor /= 100;

            return valor;
        }

        public static decimal Trunca(decimal value, int precision)
        {
            var step = (decimal)Math.Pow(10, precision);
            var tmp = (int)Math.Truncate(step * value);
            return tmp / step;
        }
    }
}
