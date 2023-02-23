using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;
using FusionCore.Sessao;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoVendasDoFaturamento : OpcaoRelatorioFixo<RVendasDoFaturamento>
    {
        public override string Descricao { get; } = "Relatório de vendas no faturamento";
        public override string Grupo { get; } = "Analises";

        protected override RVendasDoFaturamento CriaRelatorio()
        {
            return new RVendasDoFaturamento(new SessaoManagerAdm());
        }

        protected override void OnDevEditarDesenho(string arquivoFrx)
        {
            using (var r = CriaRelatorio())
            {
                r.DevEditarDesenho(arquivoFrx);
            }
        }
    }
}