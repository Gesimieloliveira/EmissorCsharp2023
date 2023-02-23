using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RContagemEstoque : RelatorioBase
    {
        public RContagemEstoque(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RContagemEstoque>("FrContagemEstoque.frx");
        }

        protected override void PrepararDados()
        {
        }
    }
}