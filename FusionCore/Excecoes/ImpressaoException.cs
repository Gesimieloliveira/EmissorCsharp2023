using System;

namespace FusionCore.Excecoes
{
    public class ImpressaoException : Exception
    {
        public ImpressaoException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}