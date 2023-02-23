using System;

namespace FusionCore.FusionAdm.Financeiro
{
    public interface IDocumentoReceber
    {
        DateTime Vencimento { get; }
        bool EstaVencido { get; }
        decimal ValorRestante { get; }
        DateTime? UltimoCalculoJuros { get; }
    }
}