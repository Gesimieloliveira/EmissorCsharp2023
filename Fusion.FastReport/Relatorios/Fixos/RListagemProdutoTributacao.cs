using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RListagemProdutoTributacao : RelatorioBase
    {
        public RListagemProdutoTributacao(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate
                .ObtemTemplate<RListagemProdutoTributacao>("FrListagemProdutoTributacao.frx");
        }

        protected override void PrepararDados()
        {
            //ignore
        }
    }
}