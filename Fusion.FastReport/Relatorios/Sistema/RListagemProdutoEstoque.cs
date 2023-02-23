using Fusion.FastReport.Repositorios;
using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RListagemProdutoEstoque : RelatorioBase
    {
        public RListagemProdutoEstoque(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RListagemProdutoEstoque>("FrListagemProdutoEstoque.frx");
        }

        protected override void PrepararRelatorio()
        {
            using (var sessao = SessaoManager.CriaStatelessSession())
            {
                var repositorio = new RepositorioProduto(sessao);
                var listagem = repositorio.BuscaDsProdutoEstoque();

                RegistraDados("dsProdutos", listagem);
            }
        }
    }
}