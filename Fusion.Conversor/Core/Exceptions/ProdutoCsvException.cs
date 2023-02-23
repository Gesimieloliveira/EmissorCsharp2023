using System;

namespace Fusion.Conversor.Core.Exceptions
{
    public class ProdutoCsvException : Exception
    {
        public ProdutoCsvException(string message) : base(message)
        {
        }

        public ProdutoCsvException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}