using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RProdutosComNcmVencido : RelatorioBase
    {
        public RProdutosComNcmVencido(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate
                .ObtemTemplate<RProdutosComNcmVencido>("FrProdutosComNcmVencido.frx");
        }

        protected override void PrepararDados()
        {
            // ignore
        }
    }
}