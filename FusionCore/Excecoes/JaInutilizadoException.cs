using System;

namespace FusionCore.Excecoes
{
    public class JaInutilizadoException : InvalidOperationException
    {
        public JaInutilizadoException(string message) : base(message)
        {
        }
    }
}