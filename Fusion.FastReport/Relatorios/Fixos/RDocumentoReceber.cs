using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RDocumentoReceber : RelatorioBase
    {
        public RDocumentoReceber(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RDocumentoPagar>("FrDocumentoReceber.frx");
        }

        protected override void PrepararDados()
        {
        }
    }
}