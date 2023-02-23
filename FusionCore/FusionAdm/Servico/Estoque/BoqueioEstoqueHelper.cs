using FusionCore.Configuracoes;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionNfce.Produto;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using NHibernate;

namespace FusionCore.FusionAdm.Servico.Estoque
{
    public static class BoqueioEstoqueHelper
    {
        public static void ChecaEstoqueNegativoAdm(ProdutoDTO produto, decimal movimento)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var configuracao = GetConfiguracao(sessao);

                if (configuracao.BloqueiaEstoqueNegativo == false)
                {
                    return;
                }

                var repositorio = new RepositorioProduto(sessao);
                var estoque = repositorio.SaldoEstoque(produto);

                if (estoque - movimento < 0)
                {
                    throw new EstoqueException("Produto não possui saldo suficiente para essa transação");
                }
            }
        }

        private static ConfiguracaoEstoque GetConfiguracao(ISession sessao)
        {
            var repositorio = new RepositorioConfiguracaoEstoque(sessao);
            return repositorio.GetConfiguracaoUnica();
        }

        public static void ChecaEstoqueNegativoPdv(ProdutoDt produto, decimal movimento, ISession sessaoPdv)
        {
            var configuracao = GetConfiguracao(sessaoPdv);

            if (configuracao?.BloqueiaEstoqueNegativo != true)
            {
                return;
            }

            if (produto.Estoque - movimento < 0)
            {
                throw new EstoqueException("Produto não possui saldo suficiente para essa transação");
            }
        }

        public static void ChecaEstoqueNegativoNfce(ProdutoNfce produto, decimal movimento, ISession sessao)
        {
            var configuracao = GetConfiguracao(sessao);

            if (configuracao?.BloqueiaEstoqueNegativo != true)
            {
                return;
            }

            if (produto.Estoque - movimento < 0)
            {
                throw new EstoqueException("Produto não possui saldo suficiente para essa transação");
            }
        }
    }
}