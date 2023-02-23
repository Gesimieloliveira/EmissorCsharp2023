using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Sistema.FaturamentoMei
{
    public class RImpressaoFaturamentoA4 : RImpressaoFaturamento
    {
        public RImpressaoFaturamentoA4(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RImpressaoFaturamentoA4>("FrFaturamentoA4.frx");
        }
    }
}