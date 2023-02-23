using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoListagemDeEstoquePorGrupo : OpcaoRelatorioFixo<RListagemEstoquePorGrupo>
    {
        public override string Descricao { get; } = "Relatório de listagem de produtos no estoque por grupos";
        public override string Grupo { get; } = "Relatório de Estoque";

        protected override RListagemEstoquePorGrupo CriaRelatorio()
        {
            return new RListagemEstoquePorGrupo(SessaoManager);
        }
    }
}