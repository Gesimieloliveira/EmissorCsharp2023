using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace FusionCore.FusionNfce.Setup.Conexao.Excecao
{
    public class ConexaoBancoDadosNfceException : Exception
    {
        public ConexaoBancoDadosNfceException()
        {
        }

        public ConexaoBancoDadosNfceException(string message) : base(message)
        {
        }

        public ConexaoBancoDadosNfceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConexaoBancoDadosNfceException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
