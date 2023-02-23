using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace FusionLibrary.Helper.Criptografia
{
    public class ArquivoAuxiliarInvalidoException : Exception
    {
        public ArquivoAuxiliarInvalidoException()
        {
        }

        public ArquivoAuxiliarInvalidoException(string message) : base(message)
        {
        }

        public ArquivoAuxiliarInvalidoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ArquivoAuxiliarInvalidoException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
