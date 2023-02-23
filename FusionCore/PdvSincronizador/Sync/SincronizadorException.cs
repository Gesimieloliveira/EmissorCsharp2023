using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace FusionCore.PdvSincronizador.Sync
{
    public class SincronizadorException : Exception
    {
        public SincronizadorException()
        {
        }

        public SincronizadorException(string message)
            : base(message)
        {
        }

        public SincronizadorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected SincronizadorException([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public SincronizadorException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }
    }
}