using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RTotalVendidoPorVendedor : RelatorioBase
    {
        public RTotalVendidoPorVendedor(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RAnaliseLucroPorItem>("FrTotalVendidoPorVendedor.frx");
        }

        protected override void PrepararDados()
        {
        }
    }
}