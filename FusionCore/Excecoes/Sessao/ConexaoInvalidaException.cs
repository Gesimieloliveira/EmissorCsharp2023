using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace FusionCore.Excecoes.Sessao
{
    public class ConexaoInvalidaException : Exception
    {
        public ConexaoInvalidaException()
        {
        }

        public ConexaoInvalidaException(string message) : base(message)
        {
        }

        public ConexaoInvalidaException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConexaoInvalidaException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
