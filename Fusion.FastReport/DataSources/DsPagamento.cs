using System.Collections.Generic;
using FusionCore.Core.Flags;
using FusionCore.Helpers.Basico;

namespace Fusion.FastReport.DataSources
{
    public class DsPagamento
    {
        public int Id { get; set; }
        public ETipoPagamento TipoPagamento { get; set; }
        public decimal Valor { get; set; }
        public IList<DsParcela> Parcelas { get; set; }

        public string TipoPagamentoTexto => TipoPagamento.GetDescription();
    }
}