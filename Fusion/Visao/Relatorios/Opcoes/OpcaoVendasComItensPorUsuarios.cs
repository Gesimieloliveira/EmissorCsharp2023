using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoVendasComItensPorUsuarios : OpcaoRelatorioFixo<RVendasComItensPorUsuario>
    {
        public override string Descricao { get; } = "Relatório de vendas com itens por usuários";
        public override string Grupo { get; } = "Analises";

        protected override RVendasComItensPorUsuario CriaRelatorio()
        {
            return new RVendasComItensPorUsuario(SessaoManager);
        }
    }
}