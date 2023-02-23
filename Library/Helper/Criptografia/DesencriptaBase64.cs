using System;

namespace FusionLibrary.Helper.Criptografia
{
    public class DesencriptaBase64
    {
        private string _dados;

        public DesencriptaBase64(string dados)
        {
            _dados = dados;
        }

        public string Processa()
        {
            try
            {
                var b = Convert.FromBase64String(_dados);

                _dados = System.Text.Encoding.ASCII.GetString(b);

                return _dados;
            }
            catch (FormatException ex)
            {
                throw new ArquivoAuxiliarInvalidoException("Arquivo corrompido.\nTente fechar o sistema e abrilo novamente.\n" +
                                                           "Se o erro persistir porfavor ligar para o suporte.\nObrigado!", ex);
            }
        }
    }
}
