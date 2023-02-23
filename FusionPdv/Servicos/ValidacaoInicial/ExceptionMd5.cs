using System;
using System.Runtime.Serialization;
using FusionPdv.Annotations;

namespace FusionPdv.Servicos.ValidacaoInicial
{
    public class ExceptionMd5 : Exception
    {
        public ExceptionMd5()
        {
        }

        public ExceptionMd5(string message) : base(message)
        {
        }

        public ExceptionMd5(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExceptionMd5([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
