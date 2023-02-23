using Fusion.FastReport.Relatorios.Sistema.FaturamentoMei;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes.Sistema
{
    public class OpcaoImpressaoFaturamento : OpcaoRelatorioBase<RImpressaoFaturamento>
    {
        public override string Descricao { get; } = "Impressão do faturamento";
        public override string Grupo { get; } = "Sistema";

        protected override RImpressaoFaturamento CriaRelatorio()
        {
            return new RImpressaoFaturamento(SessaoManager);
        }
    }
}