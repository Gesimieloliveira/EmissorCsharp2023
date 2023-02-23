using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoIventarioEstoque : OpcaoRelatorioFixo<RIventarioEstoque>
    {
        public override string Descricao { get; } = "Relatório de iventário de estoque";
        public override string Grupo { get; } = "Relatório de Estoque";

        protected override RIventarioEstoque CriaRelatorio()
        {
            return new RIventarioEstoque(SessaoManager);
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