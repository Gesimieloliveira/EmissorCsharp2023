using System;
using System.Runtime.Serialization;
using FusionPdv.Annotations;

namespace FusionPdv.Modelos.FormaPagamento
{
    public class ExceptionCartao : Exception
    {
        public ExceptionCartao()
        {
        }

        public ExceptionCartao(string message) : base(message)
        {
        }

        public ExceptionCartao(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExceptionCartao([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}