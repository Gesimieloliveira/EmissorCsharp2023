using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RIventarioEstoque : RelatorioBase
    {
        public RIventarioEstoque(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RIventarioEstoque>("FrIventarioEstoque.frx");
        }

        protected override void PrepararRelatorio()
        {
        }
    }
}