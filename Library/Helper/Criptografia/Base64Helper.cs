using System;
using System.Text;

namespace FusionLibrary.Helper.Criptografia
{
    public static class Base64Helper
    {
        public static String Computar(String input)
        {
            var b = Encoding.ASCII.GetBytes(input);
            input = Convert.ToBase64String(b);
            return input;
        }

        public static String Descomputar(String input)
        {
            var b = Convert.FromBase64String(input);
            input = Encoding.ASCII.GetString(b);
            return input;
        }
    }
}