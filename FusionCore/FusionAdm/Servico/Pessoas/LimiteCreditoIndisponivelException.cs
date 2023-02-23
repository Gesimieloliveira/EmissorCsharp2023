using System;

namespace FusionCore.FusionAdm.Servico.Pessoas
{
    public class LimiteCreditoIndisponivelException : InvalidOperationException
    {
        public LimiteCreditoIndisponivelException(string message) : base(message)
        {
        }
    }
}