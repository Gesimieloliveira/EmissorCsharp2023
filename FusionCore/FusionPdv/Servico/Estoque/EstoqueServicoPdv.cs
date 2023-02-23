using System;
using FusionCore.FusionPdv.Servico.Estoque.Evento;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Flags;
using NHibernate;

namespace FusionCore.FusionPdv.Servico.Estoque
{
    public class EstoqueServicoPdv
    {
        private readonly ISession _sessao;

        public EstoqueServicoPdv(ISession sessao)
        {
            _sessao = sessao;
        }

        public TipoControle TipoControle { get; set; }

        private static void ValidarQuantidade(EstoqueModel estoqueModel)
        {
            if (estoqueModel.Quantidade < 0)
                throw new EstoqueException("Qantidade não pode ser menor que zero (0)");

            var quantidadeInteira = (int) estoqueModel.Quantidade;
            var podeFracionar = estoqueModel.Produto.PodeFracionar;

            if (Equals(podeFracionar, IntBinario.Nao) && quantidadeInteira != estoqueModel.Quantidade)
                throw new EstoqueException("Unidade do produto não permite ser fracionada.");
        }

        public void Acrescentar(EstoqueModel estoqueModel)
        {
            ValidarQuantidade(estoqueModel);
            var temTransacao = _sessao.Transaction.IsActive;
            var transacao = _sessao.BeginTransaction();

            try
            {
                var repositorio = new ProdutoRepositorio(_sessao);
                repositorio.AcrescentaEstoque(estoqueModel.Produto, estoqueModel.Quantidade);
                RegistraEvento(estoqueModel, TipoEventoEstoque.Entrada);
                if (!temTransacao) transacao.Commit();
            }
            catch (Exception e)
            {
                if (!temTransacao) transacao.Rollback();
                throw new EstoqueException(
                    "Ocorreu um erro ao acrescentar o estoque do produto. Detalhes: " + e.Message, e);
            }
        }

        private void RegistraEvento(EstoqueModel estoqueModel, TipoEventoEstoque tipoEvento)
        {
            var eventoBuilder = new EventoEstoquePdvBuilder
            {
                EstoqueModel = estoqueModel,
                TipoEvento = tipoEvento
            };

            var evento = eventoBuilder.Build();

            var repositorioEvento = new EstoqueEventoPdvRepositorio(_sessao);
            repositorioEvento.Persistir(evento);
        }

        public void Descontar(EstoqueModel estoqueModel)
        {
            ValidarQuantidade(estoqueModel);
            var temTransacao = _sessao.Transaction.IsActive;
            var transacao = _sessao.BeginTransaction();

            try
            {
                var repositorio = new ProdutoRepositorio(_sessao);

                switch (TipoControle)
                {
                    case TipoControle.NaoControlar:
                        repositorio.DescontaEstoque(estoqueModel.Produto, estoqueModel.Quantidade);
                        break;
                    case TipoControle.Restringir:
                        if (!repositorio.PossuiSaldoDisponivel(estoqueModel.Produto, estoqueModel.Quantidade))
                            throw new InvalidOperationException("Produto não contém saldo suficiente");

                        repositorio.DescontaEstoque(estoqueModel.Produto, estoqueModel.Quantidade);
                        break;
                    default:
                        throw new InvalidOperationException("Tipo de Controle não definido");
                }

                RegistraEvento(estoqueModel, TipoEventoEstoque.Saida);
                if (!temTransacao) transacao.Commit();
            }
            catch (InvalidOperationException ex)
            {
                if (!temTransacao) transacao.Rollback();
                throw new EstoqueException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                if (!temTransacao) transacao.Rollback();
                throw new EstoqueException("Ocorreu um erro descontar o saldo do produto. Detalhes " + ex.Message, ex);
            }
        }
    }
}