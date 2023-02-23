using System;
using FusionCore.FusionAdm.Financeiro.Flags;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Filtros;
using FusionLibrary.VisaoModel;
using NHibernate;
using NHibernate.Criterion;

namespace FusionCore.FusionAdm.Financeiro.Repositorios
{
    public class FiltroResumoDocumentoReceberDTO : ViewModel, IFiltro
    {
        public Situacao? SituacaoIgual
        {
            get => GetValue<Situacao?>();
            set => SetValue(value);
        }

        public Cliente ClienteIgual
        {
            get => GetValue<Cliente>();
            set => SetValue(value);
        }

        public int? IdIgual
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        public DateTime? VencMaiorOuIgual
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public DateTime? VencMenorOuIgual
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public DateTime? QuitacaoIgual
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        public bool ApenasVencidos
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public void Aplicar<TRoot, TSub>(IQueryOver<TRoot, TSub> queryover)
        {
            DocumentoReceber tbReceber = null;

            var and = Restrictions.Conjunction();

            if (SituacaoIgual != null)
            {
                and.Add(FiltroHelper.Eq(() => tbReceber.Situacao, SituacaoIgual));
            }

            if (ClienteIgual != null)
            {
                and.Add(FiltroHelper.Eq(() => tbReceber.Cliente, ClienteIgual));
            }

            if (IdIgual != null)
            {
                and.Add(FiltroHelper.Eq(() => tbReceber.Id, IdIgual));
            }

            if (VencMaiorOuIgual != null)
            {
                and.Add(FiltroHelper.DataGe(() => tbReceber.Vencimento, VencMaiorOuIgual.Value));
            }

            if (VencMenorOuIgual != null)
            {
                and.Add(FiltroHelper.DataLe(() => tbReceber.Vencimento, VencMenorOuIgual.Value));
            }

            if (QuitacaoIgual != null)
            {
                and.Add(FiltroHelper.DataEq(() => tbReceber.DataQuitacao, QuitacaoIgual.Value));
            }

            if (ApenasVencidos)
            {
                var vencidos = FiltroHelper.DataLt(() => tbReceber.Vencimento, DateTime.Today);
                var abertos = FiltroHelper.Eq(() => tbReceber.Situacao, Situacao.Aberto);

                and.Add(Restrictions.And(vencidos, abertos));
            }

            queryover.And(and);
        }
    }
}