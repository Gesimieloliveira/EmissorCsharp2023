using System;
using System.Runtime.Serialization;
using FusionPdv.Annotations;

namespace FusionPdv.Validacao
{
    public class ExceptionCpfOuCnpj : Exception
    {
        public ExceptionCpfOuCnpj()
        {
        }

        public ExceptionCpfOuCnpj(string message) : base(message)
        {
        }

        public ExceptionCpfOuCnpj(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExceptionCpfOuCnpj([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
