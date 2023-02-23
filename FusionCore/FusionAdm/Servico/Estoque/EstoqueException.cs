using System;

namespace FusionCore.FusionAdm.Servico.Estoque
{
    public class EstoqueException : InvalidOperationException
    {
        public EstoqueException(string message) : base(message)
        {
        }

        public EstoqueException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}