using System;
using System.Collections.Generic;
using FusionCore.Debug;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.CteEletronico.Inutilizacao;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using Sped.Dominio;

// ReSharper disable RedundantBoolCompare

namespace Sped.Repositorio
{
    public class RepositorioSpedCTe : RepositorioCte
    {
        public RepositorioSpedCTe(ISession sessao) : base(sessao)
        {
        }

        public IEnumerable<XmlAutorizado> BuscarXmlAutorizadosECancelados(EmpresaDTO empresa, DateTime inicio, DateTime fim)
        {
            Cte cteAlias = null;
            CteEmissao emissaoAlias = null;
            XmlAutorizado xmlDocumento = null;
            CteEmitente emitenteAlias = null;
            CteCancelamento cancelamento = null;

            var query = Sessao.QueryOver(() => cteAlias)
                .SelectList(list => list
                    .Select(() => emissaoAlias.XmlAutorizado).WithAlias(() => xmlDocumento.Xml)
                    .Select(() => cancelamento.StatusResposta).WithAlias(() => xmlDocumento.SituacaoCodigo)
                    .Select(() => emissaoAlias.Chave).WithAlias(() => xmlDocumento.Chave)
                )
                .JoinAlias(() => cteAlias.CteEmissao, () => emissaoAlias, JoinType.InnerJoin)
                .JoinAlias(() => cteAlias.CteEmitente, () => emitenteAlias, JoinType.InnerJoin)
                .JoinAlias(() => cteAlias.Cancelamento, () => cancelamento, JoinType.LeftOuterJoin);

            if (BuildMode.IsHomologacao)
            {
                query = query.Where(() => emissaoAlias.Autorizado == true &&
                                           emissaoAlias.RecebidoEm.IsBetween(inicio).And(fim) &&
                                           emitenteAlias.Emitente == empresa);
            }
            else
            {
                query = query.Where(() => emissaoAlias.Autorizado == true &&
                                          emissaoAlias.RecebidoEm.IsBetween(inicio).And(fim) &&
                                          emitenteAlias.Emitente == empresa && emissaoAlias.Ambiente == TipoAmbiente.Producao);
            }

            query.TransformUsing(Transformers.AliasToBean<XmlAutorizado>());
            var autorizadasNoPeriodo = query.List<XmlAutorizado>();

            return autorizadasNoPeriodo;
        }

        public IEnumerable<CteInutilizacao> BuscarTodasInutilizacoesPorPeriodo(DateTime filtroDataInicio, DateTime filtroDataFinal)
        {
            CteInutilizacao inutilizacao = null;

            var query = Sessao.QueryOver(() => inutilizacao);

            var conjunction = Restrictions.Conjunction();

            conjunction.Add(Restrictions.Between(Projections.Property(() => inutilizacao.InutilizacaoEm), filtroDataInicio, filtroDataFinal));

            query.Where(conjunction);

            return query.List();
        }
    }
}