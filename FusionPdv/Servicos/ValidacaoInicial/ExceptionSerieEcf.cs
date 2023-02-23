using System;
using System.Runtime.Serialization;
using FusionPdv.Annotations;

namespace FusionPdv.Servicos.ValidacaoInicial
{
    public class ExceptionSerieEcf : Exception
    {
        public ExceptionSerieEcf(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ExceptionSerieEcf(string message) : base(message)
        {
        }

        public ExceptionSerieEcf()
        {
        }

        protected ExceptionSerieEcf([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
