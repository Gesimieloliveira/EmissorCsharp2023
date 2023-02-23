using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RAnaliseLucroPorItem : RelatorioBase
    {
        public RAnaliseLucroPorItem(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RAnaliseLucroPorItem>("FrAnaliseLucroPorItem.frx");
        }

        protected override void PrepararRelatorio()
        {
        }
    }
}