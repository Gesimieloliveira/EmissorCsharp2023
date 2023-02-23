using System;

namespace FusionCore.ControleCaixa
{
    public class ControleCaixaException : InvalidOperationException
    {
        public ControleCaixaException(string message) : base(message)
        {
        }
    }
}