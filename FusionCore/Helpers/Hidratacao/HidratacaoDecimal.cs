using System;

namespace FusionCore.Helpers.Hidratacao
{
    public static class HidratacaoDecimal
    {
        public static decimal Format(this decimal valor, string format)
        {
            var valorFormatado = valor.ToString(format);

            return Convert.ToDecimal(valorFormatado);
        } 
    }
}