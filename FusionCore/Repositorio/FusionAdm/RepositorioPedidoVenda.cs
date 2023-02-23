using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.PedidoVenda;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.Repositorio.Dtos.Consultas.PedidoDeVenda;
using FusionCore.Repositorio.Filtros;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Flags;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Util;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioPedidoVenda : Repositorio<PedidoVenda, int>
    {
        private readonly PedidoVenda _tbPedido = null;
        private readonly PedidoDestinatario _tbDestinatario = null;
        private readonly PessoaEntidade _tbPessoa = null;
        private readonly Cliente _tbCliente = null;
        private readonly PedidoVendaDTO _pedidoDto = null;

        public RepositorioPedidoVenda(ISession sessao) : base(sessao)
        {
        }

        public void SalvarAlteracoes(PedidoVenda pedidoVenda)
        {
            if (pedidoVenda.Id == 0)
            {
                Sessao.Persist(pedidoVenda);
                SalvarNegociacao(pedidoVenda);
                SalvarItens(pedidoVenda);

                Sessao.Flush();
                return;
            }

            Sessao.Update(pedidoVenda);
            SalvarNegociacao(pedidoVenda);
            SalvarItens(pedidoVenda);

            Sessao.Flush();
        }

        private void SalvarNegociacao(PedidoVenda pedidoVenda)
        {
            foreach (var negociacao in pedidoVenda.Negociacao)
            {
                if (negociacao.Id == 0)
                {
                    Sessao.Persist(negociacao);
                }
            }
        }

        private void SalvarItens(PedidoVenda pedidoVenda)
        {
            foreach (var item in pedidoVenda.ItensPedidoVenda)
            {
                if (item.Id == 0)
                {
                    Sessao.Persist(item);
                    continue;
                }

                if (item.SavePendente)
                {
                    Sessao.Update(item);
                    item.MarcaComoSalvo();
                }
            }
        }

        public void CancelarEstoque(PedidoVenda pedidoVenda, UsuarioDTO usuarioLogado)
        {
            ThrowExceptionSeNaoExisteTransacao();

            if (pedidoVenda.IsOrcamento) return;

            if (pedidoVenda.EstadoAtual != EstadoAtual.Cancelado)
            {
                throw new InvalidOperationException("É possível cancelar o estoque apenas de pedido de venda cancelado!");
            }

            var servico = EstoqueServicoAdmFactory.Cria(Sessao);

            foreach (var item in pedidoVenda.ItensPedidoVenda)
            {
                var estoque = new EstoqueModel(
                    item.Produto,
                    item.Quantidade,
                    usuarioLogado,
                    OrigemEventoEstoque.PedidoVendaCancelado
                )
                {
                    QuantidadeReservaEstoque = item.Quantidade
                };

                servico.AcrescentarEstoqueComReserva(estoque);
            }
        }

        public void DescontarEstoqueEReservaEstoque(PedidoVenda pedidoVenda, UsuarioDTO usuarioLogado)
        {
            ThrowExceptionSeNaoExisteTransacao();

            if (pedidoVenda.IsOrcamento) return;

            var servico = EstoqueServicoAdmFactory.Cria(Sessao);

            foreach (var item in pedidoVenda.ItensPedidoVenda)
            {
                var estoque = new EstoqueModel(
                    item.Produto,
                    item.Quantidade,
                    usuarioLogado,
                    OrigemEventoEstoque.ItemAdicionadoPedidoVenda
                )
                {
                    QuantidadeReservaEstoque = item.Quantidade
                };

                servico.DescontarEstoqueComReserva(estoque);
            }
        }

        public void RetirarDaReservaEstoquePedidoVenda(PedidoVenda pedidoVenda, UsuarioDTO usuarioLogado, OrigemEventoEstoque origemEventoEstoque)
        {
            ThrowExceptionSeNaoExisteTransacao();

            if (pedidoVenda.IsOrcamento) return;

            var servico = EstoqueServicoAdmFactory.Cria(Sessao);

            foreach (var item in pedidoVenda.ItensPedidoVenda)
            {
                var estoque = new EstoqueModel(
                    item.Produto,
                    item.Quantidade,
                    usuarioLogado,
                    origemEventoEstoque
                )
                {
                    QuantidadeReservaEstoque = item.Quantidade
                };

                servico.DescontarReservaVendaEfetuada(estoque);
            }
        }

        public void RetirarDaReservaEAdicionarNoEstoque(PedidoVendaProduto item, decimal quantidadeReserva, UsuarioDTO usuarioLogado, OrigemEventoEstoque origemEventoEstoque)
        {
            ThrowExceptionSeNaoExisteTransacao();

            var servico = EstoqueServicoAdmFactory.Cria(Sessao);

            var estoque = new EstoqueModel(
                item.Produto,
                quantidadeReserva,
                usuarioLogado,
                origemEventoEstoque
            )
            {
                QuantidadeReservaEstoque = quantidadeReserva
            };

            servico.AcrescentarEstoqueComReserva(estoque);
        }

        public void RetirarDoEstoqueEAdicionarNaReserva(PedidoVendaProduto item, decimal quantidadeReserva, UsuarioDTO usuarioLogado, OrigemEventoEstoque origemEventoEstoque)
        {
            ThrowExceptionSeNaoExisteTransacao();

            var servico = EstoqueServicoAdmFactory.Cria(Sessao);

            var estoque = new EstoqueModel(
                item.Produto,
                quantidadeReserva,
                usuarioLogado,
                origemEventoEstoque
            )
            {
                QuantidadeReservaEstoque = quantidadeReserva
            };

            servico.DescontarEstoqueComReserva(estoque);
        }

        public void BaixaDeOrçamentoEstoquePedidoVenda(PedidoVenda pedidoVenda, UsuarioDTO usuarioLogado, OrigemEventoEstoque origemEventoEstoque)
        {
            ThrowExceptionSeNaoExisteTransacao();

            if (!pedidoVenda.IsOrcamento) return;

            var servico = EstoqueServicoAdmFactory.Cria(Sessao);

            foreach (var item in pedidoVenda.ItensPedidoVenda)
            {
                var estoque = new EstoqueModel(
                    item.Produto,
                    item.Quantidade,
                    usuarioLogado,
                    origemEventoEstoque
                )
                {
                    QuantidadeReservaEstoque = item.Quantidade
                };

                servico.Descontar(estoque);
            }
        }

        public IList<PedidoVendaDTO> BuscaPedidosDto(IFiltro filtro)
        {
            var queryover = CriaQueryOverPedidoDto();
            filtro.Aplicar(queryover);

            return queryover.List<PedidoVendaDTO>();
        }

        private IQueryOver<PedidoVenda, PedidoVenda> CriaQueryOverPedidoDto()
        {
            var pNomeDestinatario = Projections.Conditional(
                Restrictions.IsNull(Projections.Property(() => _tbDestinatario.Cliente)),
                Projections.Property(() => _tbDestinatario.Visitante.Nome),
                Projections.Property(() => _tbPessoa.Nome)
            );

            var queryOver = Sessao.QueryOver(() => _tbPedido)
                .JoinAlias(() => _tbPedido.Destinatario, () => _tbDestinatario, JoinType.LeftOuterJoin)
                .JoinAlias(() => _tbDestinatario.Cliente, () => _tbCliente, JoinType.LeftOuterJoin)
                .JoinAlias(() => _tbCliente.Pessoa, () => _tbPessoa, JoinType.LeftOuterJoin)
                .SelectList(list => list.Select(() => _tbPedido.Id).WithAlias(() => _pedidoDto.Id)
                    .Select(() => _tbCliente.Id).WithAlias(() => _pedidoDto.IdCliente)
                    .Select(pNomeDestinatario).WithAlias(() => _pedidoDto.NomeCliente)
                    .Select(() => _tbPedido.EstadoAtual).WithAlias(() => _pedidoDto.EstadoAtual)
                    .Select(() => _tbPedido.Referencia).WithAlias(() => _pedidoDto.Referencia)
                    .Select(() => _tbPedido.AbertoEm).WithAlias(() => _pedidoDto.CriadoEm)
                    .Select(() => _tbPedido.TotalProdutos).WithAlias(() => _pedidoDto.TotalProdutos)
                    .Select(() => _tbPedido.PercentualDesconto).WithAlias(() => _pedidoDto.PercentualDesconto)
                    .Select(() => _tbPedido.Total).WithAlias(() => _pedidoDto.Total)
                    .Select(() => _tbPedido.TipoPedido).WithAlias(() => _pedidoDto.TipoPedido)
                    .Select(() => _tbDestinatario.Visitante.Nome).WithAlias(() => _pedidoDto.NomeVisitante)
                );

            queryOver.TransformUsing(Transformers.AliasToBean<PedidoVendaDTO>());

            return queryOver;
        }

        public PedidoVenda GetPeloIdLazy(int id)
        {
            var pedido = Sessao.Get<PedidoVenda>(id);

            if (pedido.Destinatario?.Cliente != null)
            {
                NHibernateUtil.Initialize(pedido.Destinatario.Cliente.Enderecos);
                NHibernateUtil.Initialize(pedido.Destinatario.Cliente.Telefones);
            }

            pedido.ItensPedidoVenda.ForEach(item =>
            {
                NHibernateUtil.Initialize(item.Produto.ProdutosAlias);
            });

            return pedido;
        }

        public void Salvar(PedidoDestinatario destinatario)
        {
            Sessao.SaveOrUpdate(destinatario);
        }
    }
}