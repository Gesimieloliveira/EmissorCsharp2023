using System;
using System.Text;

namespace FusionLibrary.Helper.Criptografia
{
    public static class HexHelper
    {
        public static string BytesToHex(byte[] input)
        {
            var sb = new StringBuilder(input.Length*2);

            foreach (var @byte in input)
                sb.AppendFormat("{0:X2}", @byte);

            return sb.ToString();
        }

        public static byte[] HexToBytes(string input)
        {
            var numberChars = input.Length;
            var bytes = new byte[numberChars/2];

            for (var i = 0; i < numberChars; i += 2)
                bytes[i/2] = Convert.ToByte(input.Substring(i, 2), 16);

            return bytes;
        }
    }
}