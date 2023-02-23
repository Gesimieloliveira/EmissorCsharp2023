using System.Collections.Generic;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.Pessoas;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioVeiculo : Repositorio<Veiculo, int>
    {
        public RepositorioVeiculo(ISession sessao) : base(sessao)
        {
        }

        public IList<Veiculo> BuscaTodosParaGrid(string input = null)
        {
            var query = Sessao.QueryOver<Veiculo>();

            if (!string.IsNullOrWhiteSpace(input))
            {
                var placa = Restrictions.Like(Projections.Property<Veiculo>(v => v.Placa), input, MatchMode.Anywhere);
                var descricao = Restrictions.Like(Projections.Property<Veiculo>(v => v.Descricao), input, MatchMode.Anywhere);
                var id = Restrictions.Eq(Projections.Cast(NHibernateUtil.String, Projections.Property<Veiculo>(v => v.Id)), input);

                query.Where(placa || descricao || id);
            }

            return query.List();
        }

        public void SalvarOuAtualizar(Veiculo veiculo)
        {
            Sessao.SaveOrUpdate(veiculo);
        }

        public ProprietarioVeiculo GetProprietario(Veiculo veiculo)
        {
            if (veiculo.TransportadoraId == null)
            {
                return default(ProprietarioVeiculo);
            }

            var query = CriaQueryOverTransportadora();

            var eqId = Restrictions.Eq(Projections.Property(() => _transportadora.Id), veiculo.TransportadoraId);
            query.Where(eqId);

            return query.Take(1).SingleOrDefault<ProprietarioVeiculo>();
        }

        private PessoaEntidade _pessoa = null;
        private Transportadora _transportadora = null;

        private IQueryOver<PessoaEntidade, PessoaEntidade> CriaQueryOverTransportadora()
        {
            ProprietarioVeiculo alias = null;

            var isTransportadora = Projections.Conditional(
                Restrictions.IsNotNull(Projections.Property(() => _transportadora.Id)),
                Projections.Constant(true),
                Projections.Constant(false)
            );

            var query = Sessao.QueryOver(() => _pessoa)
                .JoinAlias(() => _pessoa.Transportadora, () => _transportadora, JoinType.LeftOuterJoin)
                .SelectList(list => list
                    .Select(() => _pessoa.Id).WithAlias(() => alias.Id)
                    .Select(() => _pessoa.Nome).WithAlias(() => alias.Nome)
                    .Select(() => _pessoa.Cpf.Valor).WithAlias(() => alias.Cpf)
                    .Select(() => _pessoa.Cnpj.Valor).WithAlias(() => alias.Cnpj)
                    .Select(() => _pessoa.InscricaoEstadual).WithAlias(() => alias.InscricaoEstadual)
                    .Select(() => _transportadora.Rntrc).WithAlias(() => alias.Rntrc)
                    .Select(() => _transportadora.TipoProprietario).WithAlias(() => alias.Tipo)
                    .Select(() => _transportadora.Taf).WithAlias(() => alias.Taf)
                    .Select(() => _transportadora.NumeroDoRegistroEstadual).WithAlias(() => alias.NumeroRegistroEstadual)
                    .Select(isTransportadora).WithAlias(() => alias.IsTransportadora));

            query.TransformUsing(Transformers.AliasToBean<ProprietarioVeiculo>());

            return query;
        }

        public IEnumerable<ProprietarioVeiculo> BuscaProprietarios(string input = null)
        {
            var query = CriaQueryOverTransportadora();

            if (!string.IsNullOrWhiteSpace(input))
            {
                query.Where(
                    Restrictions.Eq(Projections.Cast(NHibernateUtil.String, Projections.Property(() => _transportadora.Id)), input) ||
                    Restrictions.Like(Projections.Property(() => _pessoa.Nome), input, MatchMode.Anywhere) ||
                    Restrictions.Like(Projections.Property(() => _pessoa.NomeFantasia), input, MatchMode.Anywhere)
                );
            }

            return query.List<ProprietarioVeiculo>();
        }
    }
}