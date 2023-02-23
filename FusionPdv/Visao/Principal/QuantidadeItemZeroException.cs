using System;
using System.Runtime.Serialization;
using FusionPdv.Annotations;

namespace FusionPdv.Visao.Principal
{
    public class QuantidadeItemZeroException : Exception
    {
        public QuantidadeItemZeroException()
        {
        }

        public QuantidadeItemZeroException(string message) : base(message)
        {
        }

        public QuantidadeItemZeroException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected QuantidadeItemZeroException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
