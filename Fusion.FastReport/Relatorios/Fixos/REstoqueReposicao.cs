using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class REstoqueReposicao : RelatorioBase
    {
        public static string Descricao { get; } = "Relatório de reposição estoque mínimo e máximo";

        public REstoqueReposicao(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RAnaliseLucroPorItem>("FrEstoqueReposicao.frx");
        }

        protected override void PrepararDados()
        {
            RegistrarDescricao(Descricao);
        }
    }
}