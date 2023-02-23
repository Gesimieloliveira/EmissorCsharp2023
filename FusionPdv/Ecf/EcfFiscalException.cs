using System;
using System.Runtime.Serialization;
using FusionPdv.Annotations;

namespace FusionPdv.Ecf
{
    public class EcfFiscalException : Exception
    {
        public EcfFiscalException()
        {
        }

        public EcfFiscalException(string message) : base(message)
        {
        }

        public EcfFiscalException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EcfFiscalException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
