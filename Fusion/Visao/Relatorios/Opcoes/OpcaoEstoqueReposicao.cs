using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoEstoqueReposicao : OpcaoRelatorioFixo<REstoqueReposicao>
    {
        public override string Descricao { get; } = REstoqueReposicao.Descricao;
        public override string Grupo { get; } = "Relatório de Estoque";

        protected override REstoqueReposicao CriaRelatorio()
        {
            return new REstoqueReposicao(SessaoManager);
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