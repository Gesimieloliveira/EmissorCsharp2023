using System;

namespace FusionLibrary.Helper.Criptografia
{
    public class EncriptaBase64
    {
        private string _dados;

        public EncriptaBase64(string dados)
        {
            _dados = dados;
        }

        public string Processa()
        {
            var b = System.Text.Encoding.ASCII.GetBytes(_dados);
            _dados = Convert.ToBase64String(b);

            return _dados;
        }
    }
}
