using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RNcmVencido : RelatorioBase
    {
        public RNcmVencido(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate
                .ObtemTemplate<RNcmVencido>("FrNcmVencido.frx");
        }

        protected override void PrepararDados()
        {
            // ignored
        }
    }
}