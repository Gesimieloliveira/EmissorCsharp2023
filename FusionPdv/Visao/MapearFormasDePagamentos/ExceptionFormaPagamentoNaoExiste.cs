using System;
using System.Runtime.Serialization;
using FusionPdv.Annotations;

namespace FusionPdv.Visao.MapearFormasDePagamentos
{
    public class ExceptionFormaPagamentoNaoExiste : Exception
    {
        public ExceptionFormaPagamentoNaoExiste(string message) : base(message)
        {
        }

        public ExceptionFormaPagamentoNaoExiste(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExceptionFormaPagamentoNaoExiste([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ExceptionFormaPagamentoNaoExiste()
        {
        }
    }
}
