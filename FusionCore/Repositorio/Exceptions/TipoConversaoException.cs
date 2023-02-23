using System;
using System.Runtime.Serialization;
using NHibernate;

namespace FusionCore.Repositorio.Exceptions
{
    public class TipoConversaoException : InstantiationException
    {
        public TipoConversaoException(string message, Type type) : base(message, type)
        {
        }

        public TipoConversaoException(string message, Exception innerException, Type type)
            : base(message, innerException, type)
        {
        }

        protected TipoConversaoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}