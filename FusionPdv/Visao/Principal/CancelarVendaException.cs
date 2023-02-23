using System;
using System.Runtime.Serialization;
using FusionPdv.Annotations;

namespace FusionPdv.Visao.Principal
{
    public class CancelarVendaException : Exception
    {
        public CancelarVendaException()
        {
        }

        public CancelarVendaException(string message) : base(message)
        {
        }

        public CancelarVendaException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CancelarVendaException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
