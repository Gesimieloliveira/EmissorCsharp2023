using System;

namespace FusionLibrary.Helper.Criptografia
{
    public static class GuuidHelper
    {
        public static string Computar(string input)
        {
            var md5 = Md5Helper.ComputarByte(input);
            return new Guid(md5).ToString();
        }

        public static string Computar()
        {
            var md5 = Md5Helper.ComputarByte(DateTime.Now.ToString("O"));
            return new Guid(md5).ToString();
        }

        public static string ComputarComPrefixo(string prefixo)
        {
            var md5 = Md5Helper.ComputarByte($"{prefixo} - {DateTime.Now:O}");
            return new Guid(md5).ToString();
        }
    }
}
