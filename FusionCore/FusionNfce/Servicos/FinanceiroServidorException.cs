using System;

namespace FusionCore.FusionNfce.Servicos
{
    public class FinanceiroServidorException : InvalidOperationException
    {
        public FinanceiroServidorException(string message) : base(message)
        {
        }

        public FinanceiroServidorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}