using System;
using System.Runtime.Serialization;
using FusionPdv.Annotations;

namespace FusionPdv.Visao.Principal
{
    public class NaoExisteCupomAbertoException : Exception
    {
        public NaoExisteCupomAbertoException()
        {
        }

        public NaoExisteCupomAbertoException(string message) : base(message)
        {
        }

        public NaoExisteCupomAbertoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NaoExisteCupomAbertoException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
