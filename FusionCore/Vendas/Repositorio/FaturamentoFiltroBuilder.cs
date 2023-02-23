using System;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Filtros;
using FusionCore.Vendas.Faturamentos;
using NHibernate.Criterion;
using static NHibernate.Criterion.Projections;
using static NHibernate.Criterion.Restrictions;

namespace FusionCore.Vendas.Repositorio
{
    public class FaturamentoFiltroBuilder
    {
        private readonly Conjunction _and;
        private readonly PessoaEntidade _tbPessoa = null;
        private readonly FaturamentoVenda _tbFaturamento = null;

        private FaturamentoFiltroBuilder()
        {
            _and = Conjunction();
        }

        public static FaturamentoFiltroBuilder Novo => new FaturamentoFiltroBuilder();

        public FaturamentoFiltroBuilder ComEstadoAtual(Estado? estado)
        {
            if (estado == null)
            {
                return this;
            }

            _and.Add(Eq(Property(() => _tbFaturamento.EstadoAtual), estado));
            return this;
        }

        public FaturamentoFiltroBuilder ComNumero(int? numero)
        {
            if (numero == null || numero == 0)
            {
                return this;
            }

            _and.Add(Eq(Property(() => _tbFaturamento.Id), numero));
            return this;
        }

        public FaturamentoFiltroBuilder ComNomeCliente(string nomeCliente)
        {
            if (string.IsNullOrWhiteSpace(nomeCliente))
            {
                return this;
            }

            _and.Add(Like(Property(() => _tbPessoa.Nome), nomeCliente, MatchMode.Anywhere));
            return this;
        }

        public FaturamentoFiltroBuilder ComPeriodoCriacao(FiltroPeriodo periodo)
        {
            _and.Add(periodo.Restriction(Property(() => _tbFaturamento.CriadoEm)));
            return this;
        }

        public FaturamentoFiltroBuilder ComPeriodoFinalizacao(FiltroPeriodo periodo)
        {
            _and.Add(periodo.Restriction(Property(() => _tbFaturamento.FinalizadoEm)));
            return this;
        }

        public FaturamentoFiltroBuilder ComInputLivre(string inputLivre)
        {
            if (string.IsNullOrWhiteSpace(inputLivre))
            {
                return this;
            }

            var or = Disjunction();

            or.Add(Eq(Property(() => _tbFaturamento.Id), inputLivre));
            or.Add(Like(Property(() => _tbPessoa.Nome), inputLivre, MatchMode.Anywhere));
            or.Add(Like(Property(() => _tbPessoa.NomeFantasia), inputLivre, MatchMode.Anywhere));

            _and.Add(or);

            return this;
        }

        public FaturamentoFiltroBuilder ComCriacaoApartir(DateTime? data)
        {
            if (data == null)
            {
                return this;
            }

            var periodo = new FiltroPeriodo(data.Value, DateTime.MaxValue);
            var restriction = periodo.Restriction(Property(() => _tbFaturamento.CriadoEm));

            _and.Add(restriction);

            return this;
        }

        public Conjunction Build()
        {
            return _and;
        }
    }
}