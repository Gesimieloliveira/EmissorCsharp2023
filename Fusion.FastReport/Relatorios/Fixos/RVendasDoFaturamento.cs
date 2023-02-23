using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RVendasDoFaturamento : RelatorioBase
    {
        public RVendasDoFaturamento(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RVendasDoFaturamento>("FrVendaFaturamento.frx");
        }

        protected override void PrepararDados()
        {
        }
    }
}