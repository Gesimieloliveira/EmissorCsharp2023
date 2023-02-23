using Fusion.FastReport.Relatorios;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoRelaotorioCompletoVendasPorModelo
        : OpcaoRelatorioFixo<RelatorioFixo>
    {
        public override string Descricao { get; } = "Relatório de vendas completo por modelo";
        public override string Grupo { get; } = "Analises";

        protected override RelatorioFixo CriaRelatorio()
        {
            return new RelatorioFixo(
                SessaoManager, 
                "FrRelatorioCompletoVendasPorModelo.frx", 
                Descricao
            );
        }
    }
}