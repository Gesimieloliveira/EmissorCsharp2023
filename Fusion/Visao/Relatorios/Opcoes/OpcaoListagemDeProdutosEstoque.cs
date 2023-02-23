using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoListagemDeProdutosEstoque : OpcaoRelatorioFixo<RListagemProdutoEstoque>
    {
        public override string Descricao { get; } = "Relatório de listagem de produtos no estoque";
        public override string Grupo { get; } = "Relatório de Estoque";

        protected override RListagemProdutoEstoque CriaRelatorio()
        {
            return new RListagemProdutoEstoque(SessaoManager);
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