using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpacaoVendasTransmitidasNaNfce : OpcaoRelatorioFixo<RVendasTrasmitidasNaNfce>
    {
        public override string Descricao { get; } = "Relatório de vendas transmitidas na NFC-e";
        public override string Grupo { get; } = "Analises";

        protected override RVendasTrasmitidasNaNfce CriaRelatorio()
        {
            return new RVendasTrasmitidasNaNfce(SessaoManager);
        }
    }
}