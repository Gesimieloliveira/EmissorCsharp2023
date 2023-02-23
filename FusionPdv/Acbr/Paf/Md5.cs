using System;
using System.IO;
using System.Security.Cryptography;

namespace FusionPdv.Acbr.Paf
{
    public class Md5
    {
        public string GerarMd5(string arquivo)
        {
            using (var stream = File.OpenRead(arquivo))
            {
                var md5 = new MD5CryptoServiceProvider();
                var checksum = md5.ComputeHash(stream);
                return (BitConverter.ToString(checksum)).Replace("-", string.Empty);
            }
        }
    }
}
