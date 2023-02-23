using System;

namespace FusionPdv.ManipulaValor
{
    public class ArredondarValor
    {
        public static decimal Arredonda(decimal valor)
        {
            return Math.Round(valor, 2);
        }

        public static decimal Arredonda(decimal valor, int precisao)
        {
            return Math.Round(valor, precisao);
        }
    }
}
