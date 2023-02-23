using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RListagemProdutoEstoque : RelatorioBase
    {
        public RListagemProdutoEstoque(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RListagemProdutoEstoque>("FrListagemProdutoEstoque.frx");
        }

        protected override void PrepararDados()
        {
        }
    }
}