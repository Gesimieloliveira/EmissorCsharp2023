using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace FusionCore.NfceSincronizador.Excecoes
{
    public class SincronizacaoException : Exception
    {
        public SincronizacaoException()
        {
        }

        public SincronizacaoException(string message) : base(message)
        {
        }

        public SincronizacaoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SincronizacaoException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}