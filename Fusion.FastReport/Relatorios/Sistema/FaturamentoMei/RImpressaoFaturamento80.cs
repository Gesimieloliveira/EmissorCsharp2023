using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Sistema.FaturamentoMei
{
    public class RImpressaoFaturamento80 : RImpressaoFaturamento
    {
        public RImpressaoFaturamento80(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RImpressaoFaturamento80>("FrFaturamento80mm.frx");
        }

        public override void Imprimir(string printer, int? quantidadeCopia = null)
        {
            AtivarImpressaoModoSplit();

            base.Imprimir(printer, quantidadeCopia);
        }
    }
}