using System;
using System.Runtime.Serialization;
using FusionPdv.Annotations;

namespace FusionPdv.Servicos.ArquivoAuxiliar
{
    public class ExceptionAtualizarSerieEcf : Exception
    {
        public ExceptionAtualizarSerieEcf()
        {
        }

        public ExceptionAtualizarSerieEcf(string message) : base(message)
        {
        }

        public ExceptionAtualizarSerieEcf(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExceptionAtualizarSerieEcf([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
