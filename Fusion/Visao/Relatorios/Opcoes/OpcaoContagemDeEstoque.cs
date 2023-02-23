using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoContagemDeEstoque : OpcaoRelatorioFixo<RContagemEstoque>
    {
        public override string Descricao { get; } = "Relatório de contagem de estoque";
        public override string Grupo { get; } = "Relatório de Estoque";

        protected override RContagemEstoque CriaRelatorio()
        {
            return new RContagemEstoque(SessaoManager);
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