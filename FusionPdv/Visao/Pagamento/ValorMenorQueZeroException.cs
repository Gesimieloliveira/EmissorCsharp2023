using System;
using System.Runtime.Serialization;
using FusionPdv.Annotations;

namespace FusionPdv.Visao.Pagamento
{
    public class ValorMenorQueZeroException : Exception
    {
        public ValorMenorQueZeroException()
        {
        }

        public ValorMenorQueZeroException(string message) : base(message)
        {
        }

        public ValorMenorQueZeroException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ValorMenorQueZeroException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
