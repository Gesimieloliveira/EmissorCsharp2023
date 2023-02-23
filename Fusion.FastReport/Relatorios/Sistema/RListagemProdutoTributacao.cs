using Fusion.FastReport.Repositorios;
using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RListagemProdutoTributacao : RelatorioBase
    {
        public RListagemProdutoTributacao(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate
                .ObtemTemplate<RListagemProdutoTributacao>("FrListagemProdutoTributacao.frx");
        }

        protected override void PrepararRelatorio()
        {
            using (var sessao = SessaoManager.CriaStatelessSession())
            {
                var repositorio = new RepositorioProdutoTributacao(sessao);
                var listagem = repositorio.BuscaTodos();

                RegistraDados("dsProdutos", listagem);
            }
        }
    }
}