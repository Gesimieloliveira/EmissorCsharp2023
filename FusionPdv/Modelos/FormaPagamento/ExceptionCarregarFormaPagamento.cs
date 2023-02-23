using System;
using System.Runtime.Serialization;
using FusionPdv.Annotations;

namespace FusionPdv.Modelos.FormaPagamento
{
    public class ExceptionCarregarFormaPagamento : Exception
    {
        public ExceptionCarregarFormaPagamento()
        {
        }

        public ExceptionCarregarFormaPagamento(string message) : base(message)
        {
        }

        public ExceptionCarregarFormaPagamento(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExceptionCarregarFormaPagamento([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
