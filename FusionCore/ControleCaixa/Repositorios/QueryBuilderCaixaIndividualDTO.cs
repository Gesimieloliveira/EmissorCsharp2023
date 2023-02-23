using System;
using FusionCore.CadastroUsuario;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace FusionCore.ControleCaixa.Repositorios
{
    public class QueryBuilderCaixaIndividualDTO
    {
        private readonly CaixaIndividual _tbCaixa = null;
        private readonly IUsuario _tbUsuario = null;
        private readonly CaixaIndividualDTO _alias = null;

        private IQueryOver<CaixaIndividual, CaixaIndividual> _queryOver;

        public QueryBuilderCaixaIndividualDTO(ISession session)
        {
            _queryOver = session.QueryOver(() => _tbCaixa)
                .JoinAlias(() => _tbCaixa.Usuario, () => _tbUsuario)
                .SelectList(list => list
                    .Select(() => _tbCaixa.Id).WithAlias(() => _alias.Id)
                    .Select(() => _tbCaixa.EstadoAtual).WithAlias(() => _alias.Estado)
                    .Select(() => _tbCaixa.LocalEvento).WithAlias(() => _alias.LocalEvento)
                    .Select(() => _tbCaixa.DataAbertura).WithAlias(() => _alias.DataAbertura)
                    .Select(() => _tbCaixa.SaldoInicial).WithAlias(() => _alias.SaldoInicial)
                    .Select(() => _tbCaixa.DataFechamento).WithAlias(() => _alias.DataFechamento)
                    .Select(() => _tbCaixa.SaldoCalculado).WithAlias(() => _alias.SaldoCalculado)
                    .Select(() => _tbCaixa.SaldoInformado).WithAlias(() => _alias.SaldoInformado)
                    .Select(() => _tbUsuario.Login).WithAlias(() => _alias.NomeOperador)
                    .Select(() => _tbUsuario.Id).WithAlias(() => _alias.OperadorId)
                );

            _queryOver.TransformUsing(Transformers.AliasToBean<CaixaIndividualDTO>());
        }

        public QueryBuilderCaixaIndividualDTO ApenasAbertos()
        {
            _queryOver.And(i => i.EstadoAtual == EEstadoCaixa.Aberto);

            return this;
        }

        public QueryBuilderCaixaIndividualDTO ComPeriodo(DateTime inicio, DateTime fim)
        {
            var periodo = Restrictions.Between(
                Projections.Property(() => _tbCaixa.DataFechamento),
                inicio,
                fim
            );

            _queryOver.And(periodo);

            return this;
        }

        public QueryBuilderCaixaIndividualDTO OrdernarPorAberturaAsc()
        {
            _queryOver = _queryOver.OrderBy(i => i.DataAbertura).Asc;

            return this;
        }

        public IQueryOver<CaixaIndividual> Construir()
        {
            return _queryOver;
        }
    }
}