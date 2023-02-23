using System;
using System.Runtime.Serialization;
using FusionPdv.Annotations;

namespace FusionPdv.Servicos.ArquivoAuxiliar
{
    public class ExceptionAtualizarNumeroEcf : Exception
    {
        public ExceptionAtualizarNumeroEcf()
        {
        }

        public ExceptionAtualizarNumeroEcf(string message) : base(message)
        {
        }

        public ExceptionAtualizarNumeroEcf(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExceptionAtualizarNumeroEcf([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
