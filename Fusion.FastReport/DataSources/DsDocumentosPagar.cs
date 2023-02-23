using System;

namespace Fusion.FastReport.DataSources
{
    public struct DsDocumentosPagar
    {
        private static readonly DateTime Hoje = DateTime.Today;

        public int Id { get; set; }
        public string NomeRecebedor { get; set; }
        public DateTime Vencimento { get; set; }
        public int Parcela { get; set; }
        public string NumeroDocumento { get; set; }
        public decimal ValorOriginal { get; set; }
        public decimal ValorAjustado { get; set; }
        public decimal ValorQuitado { get; set; }
        public decimal ValorRestante => ValorAjustado - ValorQuitado;
        public bool IsVencido => Vencimento < Hoje;
        public bool VenceHoje => Vencimento == Hoje;
    }
}