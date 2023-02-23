using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Flags;
using FusionCore.Vendas.Faturamentos;
using NHibernate;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace FusionCore.Vendas.Repositorio
{
    public class RepositorioFaturamento : Repositorio<FaturamentoVenda, int>
    {
        private readonly FaturamentoVenda _tbFaturamento = null;
        private readonly Destinatario _tbDestinatario = null;
        private readonly PessoaEntidade _tbPessoa = null;
        private readonly Cliente _tbCliente = null;
        private readonly FaturamentoSlim _slim = null;

        public RepositorioFaturamento(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(FaturamentoVenda faturamento)
        {
            ThrowExceptionSeNaoExisteTransacao();

            if (faturamento.Id == 0)
            {
                Sessao.Persist(faturamento);
                SalvarProdutos(faturamento);
                SalvarPagamentos(faturamento);
                Sessao.Flush();
                return;
            }

            Sessao.Update(faturamento);
            SalvarProdutos(faturamento);
            SalvarPagamentos(faturamento);
            Sessao.Flush();
        }

        private void SalvarProdutos(FaturamentoVenda faturamento)
        {
            foreach (var produto in faturamento.Produtos)
            {
                if (produto.Id == 0)
                {
                    Sessao.Persist(produto);
                }
                else if (produto.FoiAlterado)
                {
                    Sessao.Update(produto);
                }
            }
        }

        private void SalvarPagamentos(FaturamentoVenda faturamento)
        {
            foreach (var pg in faturamento.Pagamentos)
            {
                if (pg.Id == 0)
                {
                    Sessao.Persist(pg);
                }
            }
        }

        public void CancelarEstoque(FaturamentoVenda faturamento, UsuarioDTO usuario)
        {
            ThrowExceptionSeNaoExisteTransacao();

            var servico = EstoqueServicoAdmFactory.Cria(Sessao);

            foreach (var items in faturamento.Produtos)
            {
                var estoque = new EstoqueModel(
                    items.Produto,
                    items.Quantidade,
                    usuario,
                    OrigemEventoEstoque.FaturamentoCancelado
                );

                servico.Acrescentar(estoque);
            }
        }

        public IList<FaturamentoSlim> Lista(FaturamentoFiltroBuilder filtroBuilder = null, bool orderByDescId = false)
        {
            var query = Sessao.QueryOver(() => _tbFaturamento)
                .JoinAlias(() => _tbFaturamento.Destinatario, () => _tbDestinatario, JoinType.LeftOuterJoin)
                .JoinAlias(() => _tbDestinatario.Cliente, () => _tbCliente, JoinType.LeftOuterJoin)
                .JoinAlias(() => _tbCliente.Pessoa, () => _tbPessoa, JoinType.LeftOuterJoin)
                .SelectList(list => list
                    .Select(() => _tbFaturamento.Id).WithAlias(() => _slim.Id)
                    .Select(() => _tbFaturamento.EstadoAtual).WithAlias(() => _slim.EstadoAtual)
                    .Select(() => _tbFaturamento.CriadoEm).WithAlias(() => _slim.CriadoEm)
                    .Select(() => _tbFaturamento.FinalizadoEm).WithAlias(() => _slim.FinalizadoEm)
                    .Select(() => _tbPessoa.Nome).WithAlias(() => _slim.NomeCliente)
                    .Select(() => _tbFaturamento.TotalProdutos).WithAlias(() => _slim.TotalProdutos)
                    .Select(() => _tbFaturamento.PercentualDesconto).WithAlias(() => _slim.PercentualDesconto)
                    .Select(() => _tbFaturamento.Total).WithAlias(() => _slim.Total)
                    .Select(() => _tbFaturamento.SituacaoFiscal).WithAlias(() => _slim.SituacaoFiscal)
                );

            query.TransformUsing(Transformers.AliasToBean<FaturamentoSlim>());

            if (orderByDescId)
            {
                query = query.OrderBy(() => _tbFaturamento.Id).Desc;
            }

            if (filtroBuilder != null)
            {
                query.Where(filtroBuilder.Build());
            }

            return query.List<FaturamentoSlim>();
        }

        public bool PossuiFinanceiroAberto(FaturamentoVenda faturamento)
        {
            var query = Sessao.QueryOver<DocumentoReceber>()
                .Where(i => i.Malote == faturamento.Malote && i.Situacao != Situacao.Cancelado);

            return query.RowCount() > 0;
        }

        public FaturamentoVenda GetPeloIdCompleto(int faturamentoId)
        {
            var venda = GetPeloId(faturamentoId);

            foreach (var faturamentoProduto in venda.Produtos)
            {
                NHibernateUtil.Initialize(faturamentoProduto.Produto.ProdutosAlias);
            }

            if (venda.Destinatario != null)
            {
                NHibernateUtil.Initialize(venda.Destinatario.Cliente.Pessoa.Emails);
            }

            return venda;
        }

        public Estado ObterEstadoFaturamento(FaturamentoVenda faturamento)
        {
            var query = Sessao.QueryOver<FaturamentoVenda>()
                .Where(i => i.Id == faturamento.Id)
                .Select(i => i.EstadoAtual);

            return query.SingleOrDefault<Estado>();
        }
    }
}