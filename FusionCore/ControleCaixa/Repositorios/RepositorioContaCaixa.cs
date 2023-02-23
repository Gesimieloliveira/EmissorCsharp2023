using System;
using System.Collections.Generic;
using FusionCore.CadastroUsuario;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.ControleCaixa.Repositorios
{
    public class RepositorioContaCaixa : RepositorioBase
    {
        public RepositorioContaCaixa(ISession sessao) : base(sessao)
        {
        }

        public bool ExisteRegistroDeAberturaDoCaixa(CaixaIndividual caixa)
        {
            var q = Sessao.QueryOver<FluxoContaCaixa>()
                .And(i => i.OrigemReferencia == caixa.Id)
                .And(i => i.OrigemEvento == EOrigemFluxoContaCaixa.AberturaDeCaixaIndividual);

            return q.RowCount() > 0;
        }

        public bool ExisteRegistroDeFechamentoDoCaixa(CaixaIndividual caixa)
        {
            var q = Sessao.QueryOver<FluxoContaCaixa>()
                .And(i => i.OrigemReferencia == caixa.Id)
                .And(i => i.OrigemEvento == EOrigemFluxoContaCaixa.FechamentoDeCaixaIndividual);

            return q.RowCount() > 0;
        }

        public IEnumerable<FluxoContaCaixa> BuscarOperacoesApartirDe(DateTime data)
        {
            var q = Sessao.QueryOver<FluxoContaCaixa>()
                .Where(i => i.DataOperacao >= data)
                .OrderBy(i => i.Fluxo).Asc;

            return q.List();
        }

        public decimal ObtemSaldoAtualCaixaLoja()
        {
            var querySaldoAnterior = Sessao.QueryOver<FluxoContaCaixa>()
                .SelectList(list => list
                    .SelectSum(i => i.TotalOperacao)
                );

            return querySaldoAnterior.FutureValue<decimal?>().Value ?? 0.00M;
        }

        public void Persistir(FluxoContaCaixa fluxoContaCaixa)
        {
            Sessao.Persist(fluxoContaCaixa);
            Sessao.Flush();
        }

        public UsuarioDTO Parse(IUsuario usuario)
        {
            return Sessao.Get<UsuarioDTO>(usuario.Id);
        }
    }
}