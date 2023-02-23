using System;
using System.Linq;
using FusionCore.FusionNfce.Produto;
using FusionCore.Repositorio.FusionNfce;
using FusionLibrary.Helper.Diversos;
using NHibernate;

namespace FusionCore.FusionNfce.Servico.BuscarProduto
{
    public class BuscarProdutoFrenteCaixa
    {
        private readonly ISession _sessao;
        private readonly string _codigoBarras;

        public BuscarProdutoFrenteCaixa(ISession sessao, string codigoBarras)
        {
            _sessao = sessao;
            _codigoBarras = codigoBarras;
        }

        public ProdutoNfce Buscar()
        {
            var codigoBarras = _codigoBarras;

            if (codigoBarras.Length > 1 && codigoBarras.PossuiApenasNumeros() && codigoBarras.First() == '0')
            {
                codigoBarras = long.Parse(codigoBarras).ToString();
            }

            var repositorioProduto = new RepositorioProdutoNfce(_sessao);

            var produtos = repositorioProduto.BuscaPorCodigoOuCodigoBarras(codigoBarras);

            if (produtos.Count == 0)
            {
                produtos = repositorioProduto.BuscarPorCodigoBarrasComZeroAEsquerda(_codigoBarras);
            }

            if (produtos.Count == 0)
            {
                throw new InvalidOperationException("Não encontrei produto para esse código");
            }

            if (produtos.Count > 1)
            {
                throw new InvalidOperationException(
                    "Encontrei mais de 1 produto para esse código, preciso que acesse o administrador e corrija.");
            }

            var produto = produtos.FirstOrDefault();
            NHibernateUtil.Initialize(produto.ProdutosAlias);

            return produto;
        }

    }
}