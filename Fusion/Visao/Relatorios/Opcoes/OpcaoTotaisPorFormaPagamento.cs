using Fusion.FastReport.Relatorios;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoTotaisPorFormaPagamento : OpcaoRelatorioFixo<RelatorioFixo>
    {
        public override string Descricao { get; } = "Relatório de totais por forma de pagamento";
        public override string Grupo { get; } = "Analises";

        protected override RelatorioFixo CriaRelatorio()
        {
            return new RelatorioFixo(
                SessaoManager,
                "FrTotaisPorFormaDePagamento.frx",
                Descricao
            );
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