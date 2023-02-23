using System.Security.Cryptography;
using System.Text;

namespace FusionLibrary.Helper.Criptografia
{
    public static class Sha1Helper
    {
        public static string Computar(string input)
        {
            using (var sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length*2);

                foreach (var b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        public static byte[] ComputarByte(string input)
        {
            using (var sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));

                return hash;
            }
        }
    }
}