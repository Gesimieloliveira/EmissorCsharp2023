using System;

namespace Fusion.Exceptions
{
    public class PrecisaAtualizarDatabaseException : InvalidOperationException
    {
        public PrecisaAtualizarDatabaseException(string message) : base(message)
        {
        }
    }
}