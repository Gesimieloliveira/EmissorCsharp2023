using System;

namespace FusionCore.Helpers.ChaveFiscal
{
    public class GerarNumerosAleatoriosFiscal
    {
        public static int GetRandom()
        {
            var rand = new Random();
            return rand.Next(11111111, 99999999);
        }
    }
}