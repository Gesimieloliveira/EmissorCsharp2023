using System;
using System.Collections.Generic;
using FusionCore.CadastroUsuario;
using FusionCore.ControleCaixa.Individual;
using FusionCore.Core.Flags;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace FusionCore.ControleCaixa.Repositorios
{
    public class RepositorioCaixaIndividual : RepositorioBase
    {
        public RepositorioCaixaIndividual(ISession sessao) : base(sessao)
        {
        }

        public CaixaIndividual BuscarPeloId(Guid id)
        {
            return Sessao.Get<CaixaIndividual>(id);
        }

        public void Persistir(CaixaIndividual caixa)
        {
            ThrowExceptionSeNaoTemTransacao();

            Sessao.Persist(caixa);
            Sessao.Flush();
        }

        public void Alterar(CaixaIndividual caixa)
        {
            ThrowExceptionSeNaoTemTransacao();

            Sessao.Update(caixa);
            Sessao.Flush();
        }

        public void Persistir(Fluxo fluxo)
        {
            ThrowExceptionSeNaoTemTransacao();

            Sessao.Persist(fluxo);
            Sessao.Flush();
        }

        private void ThrowExceptionSeNaoTemTransacao()
        {
            if (Sessao.Transaction.IsActive)
            {
                return;
            }

            throw new InvalidOperationException("Repositório de Caixa precisa de uma Transação Aberta");
        }

        public bool ExisteCaixaAbertoPara(IUsuario usuario, ELocalEventoCaixa local)
        {
            var query = Sessao.QueryOver<CaixaIndividual>()
                .And(i => i.Usuario == usuario)
                .And(i => i.EstadoAtual != EEstadoCaixa.Fechado)
                .And(i => i.LocalEvento == local);

            return query.RowCount() > 0;
        }

        public IEnumerable<CaixaIndividualDTO> ListarCaixas()
        {
            var query = new QueryBuilderCaixaIndividualDTO(Sessao)
                .Construir();

            return query.List<CaixaIndividualDTO>();
        }

        public CaixaIndividual BuscarCaixaAberto(IUsuario usuario, ELocalEventoCaixa localEvento)
        {
            var q = Sessao.QueryOver<CaixaIndividual>()
                .And(i => i.EstadoAtual == EEstadoCaixa.Aberto)
                .And(i => i.Usuario.Id == usuario.Id)
                .And(i => i.LocalEvento == localEvento);

            var caixa = q.SingleOrDefault();

            return caixa;
        }

        public IEnumerable<FluxoResumoDTO> BuscarResumoCaixa(CaixaIndividual caixa)
        {
            FluxoResumoDTO alias = null;
            Fluxo tbFluxo = null;

            var query = Sessao.QueryOver(() => tbFluxo)
                .OrderBy(() => tbFluxo.TipoPagamento).Asc
                .ThenBy(() => tbFluxo.OrigemEvento).Asc
                .ThenBy(() => tbFluxo.TipoOperacao).Asc
                .SelectList(list => list
                    .SelectGroup(() => tbFluxo.TipoPagamento).WithAlias(() => alias.MeioPagamento)
                    .SelectGroup(() => tbFluxo.OrigemEvento).WithAlias(() => alias.OrigemEvento)
                    .SelectGroup(() => tbFluxo.TipoOperacao).WithAlias(() => alias.TipoOperacao)
                    .SelectSum(() => tbFluxo.ValorOperacao).WithAlias(() => alias.TotalOperacao)
                );

            query.Where(() => tbFluxo.Caixa == caixa);
            query.TransformUsing(Transformers.AliasToBean<FluxoResumoDTO>());

            return query.List<FluxoResumoDTO>();
        }

        public decimal TotalizarSaldoEmDinheiro(CaixaIndividual caixa)
        {
            Fluxo tbFluxo = null;

            var query = Sessao.QueryOver(() => tbFluxo)
                .Select(Projections.Sum(() => tbFluxo.ValorOperacao))
                .Where(i => i.Caixa == caixa)
                .And(i => i.TipoPagamento == ETipoPagamento.Dinheiro);

            return query.FutureValue<decimal?>().Value ?? 0.00M;
        }
    }
}