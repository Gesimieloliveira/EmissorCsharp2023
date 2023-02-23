using System;
using System.Runtime.Serialization;
using FusionPdv.Annotations;

namespace FusionPdv.Modelos.Pagamento
{
    public class PagamentoNegativoException : Exception
    {
        public PagamentoNegativoException()
        {
        }

        public PagamentoNegativoException(string message) : base(message)
        {
        }

        public PagamentoNegativoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PagamentoNegativoException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
