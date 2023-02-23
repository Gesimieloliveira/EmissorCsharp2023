using System;
using System.Collections.Generic;
using FusionCore.Debug;
using FusionCore.ExportacaoPacote.Empacotadores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Nfce;
using FusionCore.FusionAdm.Nfce.SatFiscal;
using FusionCore.FusionNfce.Fiscal.Flags;
using FusionCore.Repositorio.Contratos;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioSatAdm : Repositorio<NfceAdm, int>, IRepositorioSatAdm
    {
        public RepositorioSatAdm(ISession sessao) : base(sessao)
        {
        }

        public IEnumerable<IEnvelope> BuscarXmlExportacao(DateTime inicio, DateTime fim, EmpresaDTO empresa)
        {
            XmlExportacao alias = null;
            NfceAdm nfce = null;
            NfceEmitenteAdm emitente = null;
            NfceEmissaoAdm emissaoSatAdm = null;
            CancelamentoSatAdm cancelamentoSatAdm = null;

            var query = Sessao.QueryOver(() => emitente)
                .SelectList(list => list
                    .Select(() => emissaoSatAdm.Chave).WithAlias(() => alias.Chave)
                    .Select(() => emissaoSatAdm.XmlAutorizado).WithAlias(() => alias.Xml)
                    .Select(() => cancelamentoSatAdm.CodigoRetorno).WithAlias(() => alias.StatusRetorno))
                .JoinAlias(() => emitente.Nfce, () => nfce, JoinType.InnerJoin)
                .JoinAlias(() => nfce.Emissao, () => emissaoSatAdm, JoinType.InnerJoin)
                .JoinAlias(() => nfce.CancelamentoSat, () => cancelamentoSatAdm, JoinType.LeftOuterJoin);

            var and = Restrictions.Conjunction();

            var pStatus = Projections.Property(() => nfce.Status);
            var pCodigoAutorizacao = Projections.Property(() => emissaoSatAdm.CodigoAutorizacao);
            var pEmitidaEm = Projections.Cast(NHibernateUtil.Date, Projections.Property(() => emissaoSatAdm.RecebidoEm));
            var pEmpresa = Projections.Property(() => emitente.Empresa);

            and.Add(Restrictions.Or(Restrictions.Eq(pStatus, Status.Transmitida), Restrictions.Eq(pStatus, Status.Cancelada)));
            and.Add(Restrictions.Eq(pCodigoAutorizacao, 0));
            and.Add(Restrictions.Between(pEmitidaEm, inicio, fim));
            and.Add(Restrictions.Eq(pEmpresa, empresa));

            if (BuildMode.IsProducao)
            {
                var pAmbiente = Projections.Property(() => emissaoSatAdm.TipoAmbiente);

                and.Add(Restrictions.Eq(pAmbiente, TipoAmbiente.Producao));
            }

            query.Where(and);
            query.TransformUsing(Transformers.AliasToBean<XmlExportacao>());

            return query.List<XmlExportacao>();
        }
    }
}