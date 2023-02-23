using System;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.ExceptionCustom
{
    public class PagamentoNfeException : Exception
    {
        public PagamentoNfeException(string msg) : base(msg)
        {

        }
    }
}