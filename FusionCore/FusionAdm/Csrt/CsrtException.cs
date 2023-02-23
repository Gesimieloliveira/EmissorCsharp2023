using System;

namespace FusionCore.FusionAdm.Csrt
{
    public class CsrtException : Exception
    {
        private CsrtException(string mensagem, Exception excecaoInterior) : base(mensagem, excecaoInterior)
        {
        }

        public static CsrtException Lancar(string mensagem, Exception excecaoInterior)
        {
            return new CsrtException(mensagem, excecaoInterior);
        }
    }
}