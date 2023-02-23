using System;
using System.Collections.Generic;
using FusionCore.ExportacaoPacote.Empacotadores;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace FusionCore.GerenciarManifestacoesEletronicas
{
    public class RepositorioDistribuicaoDFe : 
        Repositorio<DistribuicaoDFe, int>,
        IRepositorioExportacaoXml
    {
        public RepositorioDistribuicaoDFe(ISession sessao) : base(sessao)
        {
        }

        public void Persistir(DistribuicaoDFe distribuicao)
        {
            ThrowExceptionSeNaoExisteTransacao();
            Sessao.Persist(distribuicao);
            Sessao.Flush();
        }

        public void Persistir(NfeResumida nfeResumida)
        {
            Sessao.Persist(nfeResumida);
            Sessao.Flush();
        }

        public void Update(NfeResumida nfeResumida)
        {
            Sessao.Persist(nfeResumida);
            Sessao.Flush();
        }

        public void Salvar(NfeResumida nfeResumida)
        {
            Sessao.SaveOrUpdate(nfeResumida);
            Sessao.Flush();
        }

        public NfeResumida BuscarNfeResumidaPela(string chave)
        {
            NfeResumida alias = null;

            return Sessao.QueryOver(() => alias).Where(() => alias.Chave == chave).SingleOrDefault();
        }

        public IEnumerable<NfeResumidaGrid> BuscaTodosNfeResumida(int? empresaId)
        {
            NfeResumida alias = null;
            EmpresaDTO empresaAlias = null;
            NfeResumidaGrid resposta = null;

            var queryOver = Sessao.QueryOver(() => alias)
                .JoinAlias(() => alias.Empresa, () => empresaAlias)
                .SelectList(list => list.Select(() => alias.Id).WithAlias(() => resposta.NfeResumidaId)
                    .Select(() => alias.Valor).WithAlias(() => resposta.ValorNFe)
                    .Select(() => alias.IsImportada).WithAlias(() => resposta.IsImportada)
                    .Select(() => alias.AutorizacaoEm).WithAlias(() => resposta.AutorizacaoEm)
                    .Select(() => alias.StatusManifestacao).WithAlias(() => resposta.SituacaoManifestacao)
                    .Select(() => alias.Chave).WithAlias(() => resposta.Chave)
                    .Select(() => alias.RazaoSocialEmitente).WithAlias(() => resposta.RazaoSocialEmitente)
                    .Select(() => alias.NumeroFiscal).WithAlias(() => resposta.NumeracaoFiscal)
                    .Select(() => alias.StatusNfe).WithAlias(() => resposta.StatusAtual)
                    .Select(() => alias.TipoOperacao).WithAlias(() => resposta.TipoOperacao)
                    .Select(() => alias.AmbienteSefaz).WithAlias(() => resposta.Ambiente)
                );

            if (empresaId != null)
            {
                queryOver.Where(() => empresaAlias.Id == empresaId);
            }

            queryOver.TransformUsing(Transformers.AliasToBean<NfeResumidaGrid>());

            return queryOver.OrderByAlias(() => alias.AutorizacaoEm).Desc.List<NfeResumidaGrid>();
        }

        public NfeResumida NfeResumidaPelo(int nfeResumidaId)
        {
            NfeResumida alias = null;

            return Sessao.QueryOver(() => alias).Where(() => alias.Id == nfeResumidaId).SingleOrDefault();
        }

        public IList<ItemDistribuicaoDFe> BuscarNaoProcessados(TipoDfe tipo, int? tipoEvento = null)
        {
            var query = Sessao.QueryOver<ItemDistribuicaoDFe>()
                .And(i => i.Processado == false)
                .And(i => i.TipoDfe == tipo);

            if (tipoEvento != null)
            {
                query.And(i => i.TipoEvento == tipoEvento);
            }

            return query.List();
        }

        public void Update(ItemDistribuicaoDFe itemDfe)
        {
            Sessao.Update(itemDfe);
            Sessao.Flush();
        }


        public MdeConsulta BuscarUltimaConsulta(string documentoUnico, TipoAmbiente ambiente, string uf)
        {
            var query = Sessao.QueryOver<MdeConsulta>()
                .Where(x => x.DocumentoUnico == documentoUnico)
                .And(x => x.AmbienteSefaz == ambiente)
                .And(x => x.Uf == uf)
                .OrderBy(e => e.DataCadastro).Desc
                .Take(1);

            return query.SingleOrDefault();
        }

        public long BuscarUltimoNsu(string documentoUnico, TipoAmbiente ambiente, string uf)
        {
            var ultimaConsulta = BuscarUltimaConsulta(documentoUnico, ambiente, uf);

            return ultimaConsulta?.UltimoNsu ?? 0;
        }

        public IEnumerable<IEnvelope> BuscarXmlExportacao(DateTime inicio, DateTime fim, EmpresaDTO empresa)
        {
            XmlExportacaoMde alias = null;
            NfeResumida tbResumo = null;
            DownloadXmlDFe tbXml = null;

            var query = Sessao.QueryOver(() => tbResumo)
                .JoinAlias(() => tbResumo.DownloadXml, () => tbXml)
                .SelectList(list => list
                    .Select(() => tbResumo.Chave).WithAlias(() => alias.Chave)
                    .Select(() => tbResumo.EmitidaEm).WithAlias(() => alias.EmitidaEm)
                    .Select(() => tbXml.Xml).WithAlias(() => alias.Xml)
                );

            var and = Restrictions.Conjunction();
            var dataAutorizacao = Projections.Cast(NHibernateUtil.Date, Projections.Property(() => tbResumo.AutorizacaoEm));
            var filtroEmpresa = Projections.Property(() => tbResumo.Empresa);

            and.Add(Restrictions.Between(dataAutorizacao, inicio, fim));
            and.Add(Restrictions.Eq(filtroEmpresa, empresa));

            query.Where(and);
            query.TransformUsing(Transformers.AliasToBean<XmlExportacaoMde>());

            return query.List<XmlExportacaoMde>();
        }
    }
}