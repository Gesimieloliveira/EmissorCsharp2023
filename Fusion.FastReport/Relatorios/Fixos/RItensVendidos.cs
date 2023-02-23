using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RItensVendidos : RelatorioBase
    {
        public RItensVendidos(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RItensVendidos>("FrItemVendido.frx");
        }

        protected override void PrepararDados()
        {
        }
    }
}