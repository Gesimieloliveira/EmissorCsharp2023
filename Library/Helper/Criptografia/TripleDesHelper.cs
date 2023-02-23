using System.Security.Cryptography;

namespace FusionLibrary.Helper.Criptografia
{
    public static class TripleDesHelper
    {
        public static byte[] ComputaBytes(byte[] key, byte[] iv, byte[] input)
        {
            using (var des = new TripleDESCryptoServiceProvider())
            {
                des.Key = key;
                des.IV = iv ?? new byte[] {49, 49, 49, 49, 49, 49, 49, 49};
                des.Padding = PaddingMode.Zeros;

                var encryptor = des.CreateEncryptor();

                return encryptor.TransformFinalBlock(input, 0, input.Length);
            }
        }

        public static byte[] DescomputaBytes(byte[] key, byte[] iv, byte[] input)
        {
            using (var des = new TripleDESCryptoServiceProvider())
            {
                des.Key = key;
                des.IV = iv ?? new byte[] {49, 49, 49, 49, 49, 49, 49, 49};
                des.Padding = PaddingMode.Zeros;

                var decryptor = des.CreateDecryptor();

                return decryptor.TransformFinalBlock(input, 0, input.Length);
            }
        }
    }
}