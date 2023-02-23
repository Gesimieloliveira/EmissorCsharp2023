using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RVendasTrasmitidasNaNfce : RelatorioBase
    {
        public RVendasTrasmitidasNaNfce(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RVendasTrasmitidasNaNfce>("FrVendasTrasmitidasNaNfce.frx");
        }

        protected override void PrepararDados()
        {
        }
    }
}