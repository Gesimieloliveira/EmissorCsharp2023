using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RDocumentoPagar : RelatorioBase
    {
        public RDocumentoPagar(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RDocumentoPagar>("FrDocumentoPagar.frx");
        }

        protected override void PrepararDados()
        {
        }
    }
}