using FusionCore.Core.Flags;

namespace FusionCore.ControleCaixa.Individual
{
    public class FluxoResumoDTO
    {
        public ETipoPagamento MeioPagamento { get; set; }
        public TipoOperacao TipoOperacao { get; set; }
        public EOrigemFluxoCaixaIndividual OrigemEvento { get; set; }
        public decimal TotalOperacao { get; set; }
    }
}