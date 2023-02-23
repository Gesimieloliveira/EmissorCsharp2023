using System;
using System.Runtime.Serialization;
using FusionPdv.Annotations;

namespace FusionPdv.Visao.Pagamento
{
    public class ExceptionValorInvalido : Exception
    {
        public ExceptionValorInvalido()
        {
        }

        public ExceptionValorInvalido(string message) : base(message)
        {
        }

        public ExceptionValorInvalido(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExceptionValorInvalido([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
