using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Repositorio.Contratos;
using FusionCore.Repositorio.Filtros;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sintegra.Dto;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioInutilizacao : Repositorio<NfeInutilizacaoNumeracaoDTO, int>, IRepositorioInutilizacao
    {
        public RepositorioInutilizacao(ISession sessao) : base(sessao)
        {
        }

        public bool FaixaInutilizadaJa(short serie, ModeloDocumento modeloDocumento, int numeroInicial, int numeroFinal)
        {
            NfeInutilizacaoNumeracaoDTO alias = null;

            var query = Sessao.QueryOver(() => alias)
                .Where(() => alias.Serie == serie && alias.ModeloDocumento == modeloDocumento
                                                  && alias.NumeroInicial == numeroInicial
                                                  && alias.NumeroFinal == numeroFinal);

            return query.RowCount() > 0;
        }

        public void Salvar(NfeInutilizacaoNumeracaoDTO entidade)
        {
            Sessao.SaveOrUpdate(entidade);
        }

        public NfeInutilizacaoNumeracaoDTO BuscarPorUuid(string uuid)
        {
            NfeInutilizacaoNumeracaoDTO alias = null;

            var query = Sessao.QueryOver(() => alias)
                .Where(() => alias.Uuid == uuid);

            return query.SingleOrDefault<NfeInutilizacaoNumeracaoDTO>();
        }

        public IList<SintegraRegistro50InutilizacaoDto> BuscaRegistro50(DateTime dataInicio,
            DateTime dataFinal,
            EmpresaDTO empresa)
        {
            NfeInutilizacaoNumeracaoDTO alias = null;
            SintegraRegistro50InutilizacaoDto resultado = null;

            var query = Sessao.QueryOver(() => alias)
                .SelectList(list => list.Select(() => alias.InutilizacaoEm).WithAlias(() => resultado.EmissaoEm)
                    .Select(() => alias.ModeloDocumento).WithAlias(() => resultado.ModeloDocumento)
                    .Select(() => alias.Serie).WithAlias(() => resultado.Serie)
                    .Select(() => alias.NumeroInicial).WithAlias(() => resultado.NumeroInicial)
                    .Select(() => alias.NumeroFinal).WithAlias(() => resultado.NumeroFinal));

            var periodo = new FiltroPeriodo(dataInicio, dataFinal);
            var intervaloMensal = periodo.Restriction(Projections.Property(() => alias.InutilizacaoEm));

            var empresaIgual = Restrictions.Eq(Projections.Property(() => alias.CnpjEmitente), empresa.Cnpj);

            var modeloIgual = Restrictions.Eq(Projections.Property(() => alias.ModeloDocumento), ModeloDocumento.NFe);

            var and = Restrictions.Conjunction();

            and.Add(intervaloMensal);
            and.Add(empresaIgual);
            and.Add(modeloIgual);

            query.Where(and);

            query.TransformUsing(Transformers.AliasToBean<SintegraRegistro50InutilizacaoDto>());

            return query.OrderByAlias(() => alias.Serie).Asc
                .ThenByAlias(() => alias.NumeroInicial)
                .Asc.ThenByAlias(() => alias.NumeroFinal)
                .Asc.List<SintegraRegistro50InutilizacaoDto>();
        }
    }
}