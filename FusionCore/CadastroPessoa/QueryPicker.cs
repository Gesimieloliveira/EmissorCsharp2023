using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Dtos.Consultas;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace FusionCore.CadastroPessoa
{
    public static class QueryPicker
    {
        public static PessoaEntidade TbPessoa { get; } = null;
        public static Cliente TbCliente { get; } = null;
        public static Vendedor TbVendedor { get; } = null;
        public static Transportadora TbTransp { get; } = null;
        public static Fornecedor TbFornecedor { get; } = null;
        public static PessoaEmail TbPessoaEmail { get; } = null;
        public static PessoaTelefone TbPessoaTelefone { get; } = null;
        public static PessoaEndereco TbPessoaEndereco { get; } = null;
        public static PessoaGrid Alias { get; } = null;

        public static IQueryOver<PessoaEntidade, PessoaEntidade> Criar(ISession session)
        {
            var clienteCondition = Projections.Conditional(
                Restrictions.Ge(Projections.Property<Cliente>(c => c.Id), 1),
                Projections.Constant(true, NHibernateUtil.Boolean),
                Projections.Constant(false, NHibernateUtil.Boolean));

            var isClienteSubQuery = QueryOver.Of(() => TbCliente)
                .Where(c => c.Id == TbPessoa.Id)
                .Select(clienteCondition)
                .Take(1);

            var vendedorCondition = Projections.Conditional(
                Restrictions.Ge(Projections.Property<Vendedor>(v => v.Id), 1),
                Projections.Constant(true, NHibernateUtil.Boolean),
                Projections.Constant(false, NHibernateUtil.Boolean)
            );

            var isVendedorSubQuery = QueryOver.Of(() => TbVendedor)
                .Where(v => v.Id == TbPessoa.Id)
                .Select(vendedorCondition).Take(1);

            var conditionTansp = Projections.Conditional(
                Restrictions.Ge(Projections.Property<Transportadora>(c => c.Id), 1),
                Projections.Constant(true, NHibernateUtil.Boolean),
                Projections.Constant(false, NHibernateUtil.Boolean));

            var isTransportadoraSubQuery = QueryOver.Of(() => TbTransp)
                .Where(c => c.Id == TbPessoa.Id)
                .Select(conditionTansp)
                .Take(1);

            var conditionFornecedor = Projections.Conditional(
                Restrictions.Ge(Projections.Property<Transportadora>(c => c.Id), 1),
                Projections.Constant(true, NHibernateUtil.Boolean),
                Projections.Constant(false, NHibernateUtil.Boolean));

            var isFornecedorSubQuery = QueryOver.Of(() => TbFornecedor)
                .Where(c => c.Id == TbPessoa.Id)
                .Select(conditionFornecedor)
                .Take(1);

            var query = session.QueryOver(() => TbPessoa)
                .SelectList(list => list
                    .Select(() => TbPessoa.Id).WithAlias(() => Alias.Id)
                    .Select(() => TbPessoa.Nome).WithAlias(() => Alias.Nome)
                    .Select(() => TbPessoa.Tipo).WithAlias(() => Alias.Tipo)
                    .Select(() => TbPessoa.Cpf.Valor).WithAlias(() => Alias.Cpf)
                    .Select(() => TbPessoa.Cnpj.Valor).WithAlias(() => Alias.Cnpj)
                    .Select(() => TbPessoa.DocumentoExterior).WithAlias(() => Alias.DocumentoExtrangeiro)
                    .Select(() => TbPessoa.InscricaoEstadual).WithAlias(() => Alias.InscricaoEstadual)
                    .Select(() => TbPessoa.Rg.Rg).WithAlias(() => Alias.Rg)
                    .SelectSubQuery(isClienteSubQuery).WithAlias(() => Alias.IsCliente)
                    .SelectSubQuery(isVendedorSubQuery).WithAlias(() => Alias.IsVendedor)
                    .SelectSubQuery(isTransportadoraSubQuery).WithAlias(() => Alias.IsTransportadora)
                    .SelectSubQuery(isFornecedorSubQuery).WithAlias(() => Alias.IsFornecedor));

            query.TransformUsing(Transformers.AliasToBean<PessoaGrid>());

            return query;
        }

        public static IQueryOver<PessoaEntidade, PessoaEntidade> CriarApenasAtivos(ISession session)
        {
            var query = Criar(session);
            query.And(Restrictions.Eq(Projections.Property(() => QueryPicker.TbPessoa.Ativo), true));

            return query;
        }
    }
}