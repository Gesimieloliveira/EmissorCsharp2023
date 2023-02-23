using System.Runtime.Serialization;
using FusionCore.FusionAdm.Fiscal.Contratos.Retorno;
using JetBrains.Annotations;

namespace FusionCore.FusionAdm.Fiscal.NF.Exception
{
    public class NFeException : System.Exception
    {
        public NfeRetorno Retorno { get; set; }

        public string MensagemErroComXml => "\n\n\n XML DE ENVIO \n\n\n " +
                                            Retorno?.Envio + 
                                            "\n\n\n XML DE RETORNO \n\n\n " +
                                            Retorno?.RetornoCompleto + 
                                            " \n\n\n Mensagem Exception \n\n\n " +
                                            Message;

        public NFeException()
        {
        }

        public NFeException(string message) : base(message)
        {
        }

        public NFeException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected NFeException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
