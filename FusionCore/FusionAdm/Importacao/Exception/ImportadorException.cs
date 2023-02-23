using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace FusionCore.FusionAdm.Importacao.Exception
{
    public class ImportadorException : System.Exception
    {
        public ImportadorException(string message) : base(message)
        {
        }

        public ImportadorException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected ImportadorException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}