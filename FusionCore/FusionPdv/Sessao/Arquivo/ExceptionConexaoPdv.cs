using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace FusionCore.FusionPdv.Sessao.Arquivo
{
    public class ExceptionConexaoPdv : Exception
    {
        public ExceptionConexaoPdv()
        {
        }

        public ExceptionConexaoPdv(string message) : base(message)
        {
        }

        public ExceptionConexaoPdv(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExceptionConexaoPdv([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
