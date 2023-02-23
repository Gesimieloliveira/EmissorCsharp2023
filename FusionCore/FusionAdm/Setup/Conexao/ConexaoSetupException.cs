using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace FusionCore.FusionAdm.Setup.Conexao
{
    public class ConexaoSetupException : Exception
    {
        public ConexaoSetupException()
        {
        }

        public ConexaoSetupException(string message) : base(message)
        {
        }

        public ConexaoSetupException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConexaoSetupException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}