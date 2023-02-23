using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoTotalVendidoPorVendedor : OpcaoRelatorioFixo<RTotalVendidoPorVendedor>
    {
        public override string Descricao { get; } = "Relatório de totalização de vendas por vendedor";
        public override string Grupo { get; } = "Analises";

        protected override RTotalVendidoPorVendedor CriaRelatorio()
        {
            return new RTotalVendidoPorVendedor(SessaoManager);
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