using System;
using FusionCore.FusionNfce.Produto;
using FusionCore.FusionNfce.Servico.Estoque;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Repositorio.Legacy.Flags;
using NHibernate;

namespace FusionCore.FusionNfce.Servico
{
    public class EstoqueServicoNfce
    {
        private EstoqueServicoNfce() { }

        private ISession _sessao;
        private EstoqueModelNfce _estoqueModel;

        public void Acrescentar()
        {
            ValidarQuantidade();
            var temTransacao = _sessao.Transaction.IsActive;
            ITransaction transacao = null;

            if (!temTransacao)
                transacao = _sessao.BeginTransaction();

            try
            {
                var repositorio = new RepositorioProdutoNfce(_sessao);
                repositorio.AcrescentaEstoque(_estoqueModel.Produto, _estoqueModel.Movimento);

                RegistraEvento();

                if (!temTransacao) transacao.Commit();
            }
            catch (Exception e)
            {
                if (!temTransacao) transacao.Rollback();
                throw new InvalidOperationException("Ocorreu um erro ao acrescentar o estoque do produto. Detalhes: " + e.Message, e);
            }

        }

        public void Descontar()
        {
            ValidarQuantidade();

            var temTransacao = _sessao.Transaction.IsActive;
            ITransaction transacao = null;

            if (!temTransacao)
                transacao = _sessao.BeginTransaction();

            try
            {
                var repositorio = new RepositorioProdutoNfce(_sessao);
                repositorio.DescontaEstoque(_estoqueModel.Produto, _estoqueModel.Movimento);

                RegistraEvento();

                if (!temTransacao) transacao.Commit();
            }
            catch (Exception ex)
            {
                if (!temTransacao) transacao.Rollback();
                throw new InvalidOperationException("Ocorreu um erro descontar o saldo do produto. Detalhes " + ex.Message, ex);
            }
        }

        private void RegistraEvento()
        {
            var repositorio = new RepositorioEstoqueModelNfce(_sessao);
            repositorio.SalvarESincronizar(_estoqueModel);
        }

        private void ValidarQuantidade()
        {
            if (_estoqueModel.Movimento < 0)
                throw new InvalidOperationException("Qantidade não pode ser menor que zero (0)"); 

            var quantidadeInteira = (int)_estoqueModel.Movimento;
            var podeFracionar = _estoqueModel.Produto.UnidadeMedida.PodeFracionar;

            if (Equals(podeFracionar, false) && quantidadeInteira != _estoqueModel.Movimento)
                throw new InvalidOperationException("Unidade do produto não permite ser fracionada.");
        }


        public static EstoqueServicoNfce Cria(ISession sessao, ProdutoNfce produto, OrigemEventoEstoque origemEventoEstoque, TipoEventoEstoque tipoEventoEstoque, decimal movimento)
        {
            return new EstoqueServicoNfce
            {
                _sessao = sessao,
                _estoqueModel = EstoqueModelNfce.Cria(produto, origemEventoEstoque, tipoEventoEstoque, movimento)
            };
        }
    }
}