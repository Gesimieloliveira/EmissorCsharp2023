using System;
using System.Runtime.Serialization;
using FusionPdv.Annotations;

namespace FusionPdv.Servicos.ValidacaoInicial
{
    public class ExceptionDataInvalidaEcf : Exception
    {
        public ExceptionDataInvalidaEcf()
        {
        }

        public ExceptionDataInvalidaEcf(string message) : base(message)
        {
        }

        public ExceptionDataInvalidaEcf(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExceptionDataInvalidaEcf([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
