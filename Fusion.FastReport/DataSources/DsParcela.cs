using System;

namespace Fusion.FastReport.DataSources
{
    public struct DsParcela
    {
        public short Numero { get; set; }
        public DateTime Vencimento { get; set; }
        public decimal Valor { get; set; }
    }
}