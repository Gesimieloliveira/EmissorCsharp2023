using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Produtos;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Produto.Historico
{
    public sealed class HistoricoCompraModel : ViewModel
    {
        public IList<ProdutoHistoricoCompra> Historicos
        {
            get => GetValue<IList<ProdutoHistoricoCompra>>();
            private set => SetValue(value);
        }

        public ProdutoDTO Produto
        {
            get => GetValue<ProdutoDTO>();
            private set => SetValue(value);
        }

        public void Carregar(ProdutoDTO produto)
        {
            Produto = produto;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioProduto(sessao);
                var historicos = repositorio.BuscaHistoricoCompra(produto.Id);

                Historicos = historicos.OrderByDescending(x => x.DataEmissao).ToList();
            }
        }
    }
}