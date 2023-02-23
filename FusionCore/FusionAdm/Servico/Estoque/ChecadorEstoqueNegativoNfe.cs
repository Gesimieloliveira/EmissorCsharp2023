using System;
using FusionCore.Configuracoes;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.FusionAdm.Servico.Estoque
{
    public static class ChecadorEstoqueNegativoNfe
    {
        public class NovoMovimento
        {
            public NovoMovimento(int produtoId, decimal qtdeMovimento, TipoOperacao tipoOperacao, int? itemId = null)
            {
                ProdutoId = produtoId;
                ItemId = itemId;
                TipoOperacao = tipoOperacao;
                QtdeMovimento = qtdeMovimento;
            }

            public int ProdutoId { get; private set; }
            public int? ItemId { get; private set; }
            public TipoOperacao TipoOperacao { get; private set; }
            public decimal QtdeMovimento { get; private set; }
        }

        private static ConfiguracaoEstoque GetConfiguracao(ISession sessao)
        {
            var repositorio = new RepositorioConfiguracaoEstoque(sessao);
            return repositorio.GetConfiguracaoUnica();
        }

        public static void ThrowExceptionSeQuantidadeNegativarEstoque(NovoMovimento movimento)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var configuracao = GetConfiguracao(sessao);

                if (configuracao.BloqueiaEstoqueNegativo == false)
                {
                    return;
                }

                var quantidadeAnterior = 0.0M;

                if (movimento.ItemId != null)
                {
                    quantidadeAnterior = sessao.QueryOver<ItemNfe>()
                        .Select(i => i.Quantidade)
                        .Where(i => i.Id == movimento.ItemId && i.Produto.Id == movimento.ProdutoId)
                        .SingleOrDefault<decimal>();
                }

                var repositorio = new RepositorioProduto(sessao);
                var estoqueAtual = repositorio.SaldoEstoque(movimento.ProdutoId);
                var estoqueFuturo = estoqueAtual + quantidadeAnterior - movimento.QtdeMovimento;

                if (movimento.TipoOperacao == TipoOperacao.Entrada)
                {
                    estoqueFuturo = estoqueAtual - quantidadeAnterior + movimento.QtdeMovimento;
                }

                if (estoqueFuturo < 0)
                {
                    throw new EstoqueException("Produto não possui saldo suficiente para essa transação");
                }
            }
        }

        public static void ThrowExceptionSeRemoverItemNotaEntradaNegativarEstoque(int produtoId, decimal qtdeRemover)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var configuracao = GetConfiguracao(sessao);

                if (configuracao.BloqueiaEstoqueNegativo == false)
                {
                    return;
                }

                var repositorio = new RepositorioProduto(sessao);
                var estoqueAtual = repositorio.SaldoEstoque(produtoId);
                var estoqueFuturo = estoqueAtual - qtdeRemover;

                if (estoqueFuturo < 0)
                {
                    throw new EstoqueException("Item não pode ser removido, o saldo do Produto ficará negativo!");
                }
            }
        }
    }
}