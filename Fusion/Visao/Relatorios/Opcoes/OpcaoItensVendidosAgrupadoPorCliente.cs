using Fusion.FastReport.Relatorios;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoItensVendidosAgrupadoPorCliente : 
        OpcaoRelatorioFixo<RelatorioFixo>
    {
        public override string Descricao { get; } = "Relatório de itens vendidos agrupado pelo cliente";
        public override string Grupo { get; } = "Analises";

        protected override RelatorioFixo CriaRelatorio()
        {
            return new RelatorioFixo(
                SessaoManager,
                "FrItensVendidosAgrupadoPorCliente.frx",
                Descricao
            );
        }
    }
}