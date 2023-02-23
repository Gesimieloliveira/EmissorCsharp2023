using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RComprasDetalhado : RelatorioBase
    {
        public RComprasDetalhado(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RComprasDetalhado>("FrCompraDetalhada.frx");
        }

        protected override void PrepararRelatorio()
        {
        }
    }
}