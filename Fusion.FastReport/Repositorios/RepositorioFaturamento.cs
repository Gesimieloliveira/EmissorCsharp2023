using System.Collections.Generic;
using Fusion.FastReport.DataSources;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Vendas.Faturamentos;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace Fusion.FastReport.Repositorios
{
    public class RepositorioFaturamento : Repositorio
    {
        private readonly FaturamentoVenda _tbFaturamento = null;
        private readonly FaturamentoProduto _tbItem = null;
        private readonly EmpresaDTO _tbEmpresa = null;
        private readonly Destinatario _tbDestinatario = null;
        private readonly Cliente _tbCliente = null;
        private readonly FaturamentoVendedor _tbFaturamentoVendedor = null;
        private readonly Vendedor _tbVendedor = null;
        private readonly PessoaEntidade _tbPessoa = null;
        private readonly CidadeDTO _tbCidadeDestinatario = null;
        private readonly CidadeDTO _tbCidadeEmpresa = null;
        private readonly ProdutoDTO _tbProduto = null;
        private readonly ProdutoUnidadeDTO _tbUnidade = null;
        private readonly PessoaTelefone _tbTelefone = null;
        private readonly FPagamento _tbPagamento = null;
        private readonly FParcela _tbParcela = null;
        private readonly DsFaturamento _dsFaturamento;
        private readonly DsTelefone _dsTelefone;
        private readonly DsFaturamentoItem _dsItem;
        private readonly DsPagamento _dsPagamento;
        private readonly DsParcela _dsParcela;
        
        public RepositorioFaturamento(IStatelessSession sessao) : base(sessao)
        {
        }

        public DsFaturamento GetFaturamento(int id)
        {
            //TODO: Corrigir busca de faturamento para fasreport

            PessoaEntidade pessoaVendedor = null;

            var query = Sessao.QueryOver(() => _tbFaturamento)
                .JoinAlias(() => _tbFaturamento.Empresa, () => _tbEmpresa, JoinType.LeftOuterJoin)                
                .JoinAlias(() => _tbFaturamento.Destinatario, () => _tbDestinatario, JoinType.LeftOuterJoin)
                .JoinAlias(() => _tbDestinatario.Cliente, () => _tbCliente, JoinType.LeftOuterJoin)
                .JoinAlias(() => _tbFaturamento.Vendedor, () => _tbFaturamentoVendedor, JoinType.LeftOuterJoin)
                .JoinAlias(() => _tbFaturamentoVendedor.Vendedor, () => _tbVendedor, JoinType.LeftOuterJoin)
                .JoinAlias(() => _tbVendedor.Pessoa, () => pessoaVendedor, JoinType.LeftOuterJoin)
                .JoinAlias(() => _tbDestinatario.Endereco.Cidade, () => _tbCidadeDestinatario, JoinType.LeftOuterJoin)
                .JoinAlias(() => _tbCliente.Pessoa, () => _tbPessoa, JoinType.LeftOuterJoin)
                .JoinAlias(() => _tbEmpresa.CidadeDTO, () => _tbCidadeEmpresa, JoinType.LeftOuterJoin)
                .SelectList(list => list
                    .Select(() => _tbFaturamento.Id).WithAlias(() => _dsFaturamento.Id)
                    .Select(() => _tbFaturamento.FinalizadoEm).WithAlias(() => _dsFaturamento.FinalizadoEm)
                    .Select(() => _tbPessoa.Id).WithAlias(() => _dsFaturamento.ClienteId)
                    .Select(() => _tbPessoa.Nome).WithAlias(() => _dsFaturamento.NomeCliente)
                    .Select(() => pessoaVendedor.Nome).WithAlias(() => _dsFaturamento.NomeVendedor)
                    .Select(() => _tbPessoa.Cpf.Valor).WithAlias(() => _dsFaturamento.CpfCliente)
                    .Select(() => _tbPessoa.Cnpj.Valor).WithAlias(() => _dsFaturamento.CnpjCliente)
                    .Select(() => _tbDestinatario.Endereco.Bairro).WithAlias(() => _dsFaturamento.BairroCliente)
                    .Select(() => _tbDestinatario.Endereco.Numero).WithAlias(() => _dsFaturamento.NumeroCliente)
                    .Select(() => _tbDestinatario.Endereco.Logradouro).WithAlias(() => _dsFaturamento.LogradouroCliente)
                    .Select(() => _tbDestinatario.Endereco.Cidade).WithAlias(() => _dsFaturamento.CidadeCliente)
                    .Select(() => _tbEmpresa.Id).WithAlias(() => _dsFaturamento.EmpresaId)
                    .Select(() => _tbEmpresa.NomeFantasia).WithAlias(() => _dsFaturamento.NomeEmpresa)
                    .Select(() => _tbEmpresa.Logradouro).WithAlias(() => _dsFaturamento.LogradouroEmpresa)
                    .Select(() => _tbEmpresa.Numero).WithAlias(() => _dsFaturamento.NumeroEmpresa)
                    .Select(() => _tbEmpresa.Bairro).WithAlias(() => _dsFaturamento.BairroEmpresa)
                    .Select(() => _tbEmpresa.CidadeDTO).WithAlias(() => _dsFaturamento.CidadeEmpresa)
                    .Select(() => _tbEmpresa.Cnpj).WithAlias(() => _dsFaturamento.CnpjEmpresa)
                    .Select(() => _tbEmpresa.InscricaoEstadual).WithAlias(() => _dsFaturamento.IeEmpresa)
                    .Select(() => _tbEmpresa.LogoMarcaNfce).WithAlias(() => _dsFaturamento.Logo)
                    .Select(() => _tbFaturamento.Observacao).WithAlias(() => _dsFaturamento.Observacao)
                    .Select(() => _tbFaturamento.TotalProdutos).WithAlias(() => _dsFaturamento.TotalProdutos)
                    .Select(() => _tbFaturamento.TotalDesconto).WithAlias(() => _dsFaturamento.TotalDesconto)
                    .Select(() => _tbFaturamento.Total).WithAlias(() => _dsFaturamento.Total)
                    .Select(() => _tbFaturamento.Troco).WithAlias(() => _dsFaturamento.Troco)
                );

            query.TransformUsing(Transformers.AliasToBean<DsFaturamento>());
            query.Where(Restrictions.Eq(Projections.Property(() => _tbFaturamento.Id), id));

            var faturamento = query.SingleOrDefault<DsFaturamento>();

            faturamento.TelefonesDestinatario = GetTelefones(faturamento);
            faturamento.Pagamentos = GetPagamentos(faturamento);

            return faturamento;
        }

        private IList<DsTelefone> GetTelefones(DsFaturamento ds)
        {
            var query = Sessao.QueryOver(() => _tbTelefone)
                .SelectList(list => list
                    .Select(() => _tbTelefone.Descricao).WithAlias(() => _dsTelefone.Descricao)
                    .Select(() => _tbTelefone.Numero).WithAlias(() => _dsTelefone.Numero)
                );

            query.TransformUsing(Transformers.AliasToBean<DsTelefone>());
            query.Where(Restrictions.Eq(Projections.Property(() => _tbTelefone.Contato), ds.ClienteId));
            
            return query.List<DsTelefone>();
        }

        public IList<DsPagamento> GetPagamentos(DsFaturamento faturamento)
        {
            var qEspecie = Sessao.QueryOver(() => _tbPagamento)
                .SelectList(list => list
                    .Select(() => _tbPagamento.Id).WithAlias(() => _dsPagamento.Id)
                    .Select(() => _tbPagamento.Valor).WithAlias(() => _dsPagamento.Valor)
                    .Select(() => _tbPagamento.Especie).WithAlias(() => _dsPagamento.TipoPagamento)
                );

            qEspecie.TransformUsing(Transformers.AliasToBean<DsPagamento>());
            qEspecie.Where(() => _tbPagamento.Faturamento.Id == faturamento.Id);

            var pagamentos = qEspecie.List<DsPagamento>();

            foreach (var pg in pagamentos)
            {
                if (pg.TipoPagamento != ETipoPagamento.CreditoLoja)
                {
                    continue;
                }

                var prc = Sessao.QueryOver(() => _tbParcela)
                    .SelectList(list => list
                        .Select(() => _tbParcela.Numero).WithAlias(() => _dsParcela.Numero)
                        .Select(() => _tbParcela.Vencimento).WithAlias(() => _dsParcela.Vencimento)
                        .Select(() => _tbParcela.Valor).WithAlias(() => _dsParcela.Valor)
                    );

                prc.TransformUsing(Transformers.AliasToBean<DsParcela>());
                prc.Where(() => _tbParcela.Pagamento.Id == pg.Id);

                pg.Parcelas = prc.List<DsParcela>();
            }

            return pagamentos;
        }

        public IList<DsFaturamentoItem> GetItens(int faturamentoId)
        {
            var query = Sessao.QueryOver(() => _tbItem)
                .JoinAlias(() => _tbItem.Produto, () => _tbProduto, JoinType.InnerJoin)
                .SelectList(list => list
                    .Select(() => _tbItem.Id).WithAlias(() => _dsItem.ItemId)
                    .Select(() => _tbProduto.Id).WithAlias(() => _dsItem.ProdutoId)
                    .Select(() => _tbItem.Faturamento.Id).WithAlias(() => _dsItem.FaturamentoId)
                    .Select(() => _tbProduto.Nome).WithAlias(() => _dsItem.Descricao)
                    .Select(() => _tbItem.Quantidade).WithAlias(() => _dsItem.Quantidade)
                    .Select(() => _tbItem.SiglaUnidade).WithAlias(() => _dsItem.SiglaUnidade)
                    .Select(() => _tbItem.PrecoUnitario).WithAlias(() => _dsItem.ValorUnitario)
                    .Select(() => _tbItem.Total).WithAlias(() => _dsItem.Total)
                );

            query.TransformUsing(Transformers.AliasToBean<DsFaturamentoItem>());
            query.Where(Restrictions.Eq(Projections.Property(() => _tbItem.Faturamento.Id), faturamentoId));

            return query.List<DsFaturamentoItem>();
        }
    }
}