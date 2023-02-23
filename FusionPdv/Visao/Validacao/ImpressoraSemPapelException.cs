using System;
using System.Runtime.Serialization;
using FusionPdv.Annotations;

namespace FusionPdv.Visao.Validacao
{
    public class ImpressoraSemPapelException : Exception
    {
        public ImpressoraSemPapelException()
        {
        }

        public ImpressoraSemPapelException(string message) : base(message)
        {
        }

        public ImpressoraSemPapelException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ImpressoraSemPapelException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
