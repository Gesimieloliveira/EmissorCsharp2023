using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace FusionCore.Permissoes
{
    public class PermissaoException : Exception
    {
        public PermissaoException()
        {
        }

        public PermissaoException(string message) : base(message)
        {
        }

        public PermissaoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PermissaoException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}