using System;
using FusionCore.FusionAdm.Servico.Estoque.Evento;
using FusionCore.Repositorio.Legacy.Comandos.Adm.Estoque;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Flags;
using NHibernate;

namespace FusionCore.FusionAdm.Servico.Estoque
{
    public class EstoqueServicoAdm
    {
        private readonly ISession _sessao;

        protected internal EstoqueServicoAdm(ISession sessao)
        {
            _sessao = sessao;
        }

        public void CriarEstoque(EstoqueModel estoqueModel)
        {
            try
            {
                var estoque = CriarProdutoEstoque(estoqueModel);
                _sessao.Persist(estoque);

                RegistrarEvento(estoqueModel, estoque, TipoEventoEstoque.Entrada);
            }
            catch (Exception e)
            {
                throw new EstoqueException("Ocorreu um erro inesperado ao criar o estoque para o produto. Detalhes: " + e.Message, e);
            }
        }

        private void RegistrarEvento(
            EstoqueModel estoqueModel,
            ProdutoEstoqueDTO estoqueAtual,
            TipoEventoEstoque tipoEvento)
        {
            var eventoBuilder = new EventoEstoqueBuilder
            {
                EstoqueAtualizado = estoqueAtual,
                EstoqueModel = estoqueModel,
                TipoEvento = tipoEvento
            };

            var evento = eventoBuilder.Build();
            _sessao.Persist(evento);
        }

        private static ProdutoEstoqueDTO CriarProdutoEstoque(EstoqueModel estoqueModel)
        {
            return new ProdutoEstoqueDTO
            {
                Estoque = estoqueModel.Quantidade,
                ProdutoDTO = estoqueModel.Produto,
                AlteradoEm = DateTime.Now,
                EstoqueMinimo = estoqueModel.EstoqueMinimo,
                EstoqueMaximo = estoqueModel.EstoqueMaximo
            };
        }

        public void Acrescentar(EstoqueModel estoqueModel)
        {
            try
            {
                var comando = new AcrescentaEstoque(estoqueModel.Produto, estoqueModel.Quantidade);
                var estoque = comando.ExecutaComando(_sessao);

                RegistrarEvento(estoqueModel, estoque, TipoEventoEstoque.Entrada);
            }
            catch (Exception e)
            {
                throw new EstoqueException(
                    "Ocorreu um erro ao acrescentar o estoque do produto. Detalhes: " + e.Message,
                    e);
            }
        }

        public void AcrescentarEstoqueComReserva(EstoqueModel estoqueModel)
        {
            try
            {
                var comando = new AcresentaEstoqueComReserva(estoqueModel.Produto, estoqueModel.Quantidade, estoqueModel.QuantidadeReservaEstoque);
                var estoque = comando.ExecutaComando(_sessao);

                RegistrarEvento(estoqueModel, estoque, TipoEventoEstoque.Entrada);
            }
            catch (Exception e)
            {
                throw new EstoqueException(
                    "Ocorreu um erro ao acrescentar o estoque do produto. Detalhes: " + e.Message,
                    e);
            }
        }

        public void Descontar(EstoqueModel estoqueModel)
        {
            try
            {
                var produto = estoqueModel.Produto;
                var quantidade = estoqueModel.Quantidade;

                var comando = new DescontaEstoque(produto, quantidade);
                var estoque = comando.ExecutaComando(_sessao);

                RegistrarEvento(estoqueModel, estoque, TipoEventoEstoque.Saida);
            }
            catch (InvalidOperationException ex)
            {
                throw new EstoqueException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new EstoqueException("Ocorreu um erro descontar o saldo do produto. Detalhes " + ex.Message, ex);
            }
        }

        public void DescontarEstoqueComReserva(EstoqueModel estoqueModel)
        {
            try
            {
                var produto = estoqueModel.Produto;
                var quantidade = estoqueModel.Quantidade;
                var quantidadeReservado = estoqueModel.QuantidadeReservaEstoque;

                var comando = new DescontaEstoqueComReserva(produto, quantidade, quantidadeReservado);
                var estoque = comando.ExecutaComando(_sessao);

                RegistrarEvento(estoqueModel, estoque, TipoEventoEstoque.Saida);
            }
            catch (InvalidOperationException ex)
            {
                throw new EstoqueException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new EstoqueException("Ocorreu um erro descontar o saldo do produto. Detalhes " + ex.Message, ex);
            }
        }

        public void DescontarReservaVendaEfetuada(EstoqueModel estoqueModel)
        {
            try
            {
                var produto = estoqueModel.Produto;
                var quantidadeReservado = estoqueModel.QuantidadeReservaEstoque;

                var comando = new DescontaEstoqueReservaVendaEfetuada(produto, quantidadeReservado);
                var estoque = comando.ExecutaComando(_sessao);

                RegistrarEvento(estoqueModel, estoque, TipoEventoEstoque.Saida);
            }
            catch (InvalidOperationException ex)
            {
                throw new EstoqueException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new EstoqueException("Ocorreu um erro descontar o saldo do produto. Detalhes " + ex.Message, ex);
            }
        }

        public EstoqueEventoDTO ReceberMovimentacao(EventoEstoqueBuilder eventoBuilder)
        {
            try
            {
                var estoque = MovimentaEstoque(eventoBuilder);
                eventoBuilder.EstoqueAtualizado = estoque;

                var evento = eventoBuilder.Build();
                _sessao.Persist(evento);

                return evento;
            }
            catch (Exception ex)
            {
                throw new EstoqueException("Erro ao movimentar estoque por evento builder: " + ex.Message, ex);
            }
        }

        private ProdutoEstoqueDTO MovimentaEstoque(EventoEstoqueBuilder eventoBuilder)
        {
            var produto = eventoBuilder.EstoqueModel.Produto;
            var quantidade = eventoBuilder.EstoqueModel.Quantidade;

            switch (eventoBuilder.TipoEvento)
            {
                case TipoEventoEstoque.Entrada:
                    return new AcrescentaEstoque(produto, quantidade).ExecutaComando(_sessao);
                case TipoEventoEstoque.Saida:
                    return new DescontaEstoque(produto, quantidade).ExecutaComando(_sessao);
                default:
                    throw new InvalidOperationException("Receber movimentação pdv sem tipo do evento definido");
            }
        }
    }
}