using System;

namespace FusionCore.FusionPdv.Servico.Estoque
{
    public class EstoqueException : Exception
    {
        public EstoqueException()
        {
        }

        public EstoqueException(string message)
            : base(message)
        {
        }

        public EstoqueException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}