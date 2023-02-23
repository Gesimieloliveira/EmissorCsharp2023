using System;
using System.Runtime.Serialization;
using FusionPdv.Annotations;

namespace FusionPdv.Servicos.ValidacaoInicial
{
    public class ExceptionExisteAliquota : Exception
    {
        public ExceptionExisteAliquota()
        {
        }

        public ExceptionExisteAliquota(string message) : base(message)
        {
        }

        public ExceptionExisteAliquota(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExceptionExisteAliquota([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
