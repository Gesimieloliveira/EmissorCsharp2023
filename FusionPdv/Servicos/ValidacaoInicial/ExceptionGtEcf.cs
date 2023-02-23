using System;
using System.Runtime.Serialization;
using FusionPdv.Annotations;

namespace FusionPdv.Servicos.ValidacaoInicial
{
    public class ExceptionGtEcf : Exception
    {
        public ExceptionGtEcf()
        {
        }

        public ExceptionGtEcf(string message) : base(message)
        {
        }

        public ExceptionGtEcf(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExceptionGtEcf([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
