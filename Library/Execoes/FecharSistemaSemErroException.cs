using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace FusionLibrary.Execoes
{
    public class FecharSistemaSemErroException : Exception
    {
        public FecharSistemaSemErroException(string message) : base(message)
        {
        }

        public FecharSistemaSemErroException()
        {
        }

        public FecharSistemaSemErroException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FecharSistemaSemErroException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
