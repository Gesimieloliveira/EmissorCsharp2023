using System;

namespace FusionCore.FusionAdm.Setup.Conexao
{
    public class ArquivoNaoExisteException : Exception
    {
        public ArquivoNaoExisteException(string message) : base(message)
        {
        }
    }
}