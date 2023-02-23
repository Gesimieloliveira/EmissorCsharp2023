using Fusion.FastReport.Relatorios;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoVendasAgrupadoPorCliente : 
        OpcaoRelatorioFixo<RelatorioFixo>
    {
        public override string Descricao { get; } = "Relatório de vendas agrupado pelo cliente";
        public override string Grupo { get; } = "Analises";

        protected override RelatorioFixo CriaRelatorio()
        {
            return new RelatorioFixo(
                SessaoManager,
                "FrVendasAgrupadoPorCliente.frx",
                Descricao
            );
        }
    }
}