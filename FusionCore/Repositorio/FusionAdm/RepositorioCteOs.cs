using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Debug;
using FusionCore.ExportacaoPacote.Empacotadores;
using FusionCore.FusionAdm.CteEletronico.CCe;
using FusionCore.FusionAdm.CteEletronicoOs.Cancelamento;
using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.FusionAdm.CteEletronicoOs.Historicos;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Contratos;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

// ReSharper disable RedundantBoolCompare

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioCteOs : Repositorio<CteOs, int>, IRepositorioCteOs, IRepositorioExportacaoXml, IRepositorioCartaCorrecaoCte
    {
        private readonly CteOs _tbCte = null;
        private readonly CteOsEmissaoFinalizada _tbEmissao = null;
        private readonly CteOsCancelado _tbCancelamento = null;
        private readonly EmpresaDTO _tbEmpresa = null;
        private readonly XmlExportacaoCte _aXml = null;

        public RepositorioCteOs(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(CteOs cteOs)
        {
            Sessao.SaveOrUpdate(cteOs);
            if (cteOs.TributacaoFederal == null) DeleteImpostoFederal(cteOs.Id);
            Sessao.Flush();
        }

        private void DeleteImpostoFederal(int id)
        {
            if (id == 0) return;
            var query = $"from {nameof(CteOsTributacaoFederal)} e" +
                        $" where e.{nameof(CteOsTributacaoFederal.CteOsId)} = ?";
            Sessao.Delete(query, id, NHibernateUtil.Int32);
        }

        public IEnumerable<GridCteOsDTO> BuscarTodosGridCteOsDTO()
        {
            CteOs cteOs = null;
            EmpresaDTO emitente = null;
            GridCteOsDTO resposta = null;
            PessoaEntidade tomador = null;
            CteOsNormal normal = null;
            CteOsEmissaoFinalizada emissao = null;

            var todos = Sessao.QueryOver(() => cteOs)
                .JoinAlias(() => cteOs.Emitente, () => emitente, JoinType.InnerJoin)
                .JoinAlias(() => cteOs.Tomador, () => tomador, JoinType.LeftOuterJoin)
                .JoinAlias(() => cteOs.Normal, () => normal, JoinType.LeftOuterJoin)
                .JoinAlias(() => cteOs.Emissao, () => emissao, JoinType.LeftOuterJoin)
                .SelectList(list => list.Select(() => cteOs.Id).WithAlias(() => resposta.Id)
                    .Select(() => cteOs.Status).WithAlias(() => resposta.Status)
                    .Select(() => cteOs.NumeroEmissao).WithAlias(() => resposta.NumeroDocumento)
                    .Select(() => cteOs.SerieEmissao).WithAlias(() => resposta.SerieDocumento)
                    .Select(() => tomador.Nome).WithAlias(() => resposta.TomadorNome)
                    .Select(() => emitente.RazaoSocial).WithAlias(() => resposta.EmitenteNome)
                    .Select(() => cteOs.PrecoServico.Valor).WithAlias(() => resposta.ValorServico)
                    .Select(() => cteOs.PrecoServico.AReceber).WithAlias(() => resposta.ValorReceber)
                    .Select(() => normal.DescricaoServicoPrestado).WithAlias(() => resposta.DescricaoServico)
                    .Select(() => emissao.Chave).WithAlias(() => resposta.Chave)
                )
                .TransformUsing(Transformers.AliasToBean<GridCteOsDTO>()).OrderByAlias(() => cteOs.Id).Desc.List<GridCteOsDTO>();

            return todos;
        }

        public IEnumerable<GridCteOsDTO> BuscarTodosGridCteOsDTOFiltrando(string textoFiltrado)
        {
            CteOs cteOs = null;
            GridCteOsDTO resposta = null;
            EmpresaDTO emitente = null;
            PessoaEntidade tomador = null;
            CteOsNormal normal = null;
            CteOsEmissaoFinalizada emissao = null;

            var query = Sessao.QueryOver(() => cteOs)
                .JoinAlias(() => cteOs.Emitente, () => emitente, JoinType.InnerJoin)
                .JoinAlias(() => cteOs.Tomador, () => tomador, JoinType.LeftOuterJoin)
                .JoinAlias(() => cteOs.Normal, () => normal, JoinType.LeftOuterJoin)
                .JoinAlias(() => cteOs.Emissao, () => emissao, JoinType.LeftOuterJoin)
                .SelectList(list => list.Select(() => cteOs.Id).WithAlias(() => resposta.Id)
                    .Select(() => cteOs.Status).WithAlias(() => resposta.Status)
                    .Select(() => cteOs.NumeroEmissao).WithAlias(() => resposta.NumeroDocumento)
                    .Select(() => cteOs.SerieEmissao).WithAlias(() => resposta.SerieDocumento)
                    .Select(() => tomador.Nome).WithAlias(() => resposta.TomadorNome)
                    .Select(() => emitente.RazaoSocial).WithAlias(() => resposta.EmitenteNome)
                    .Select(() => cteOs.PrecoServico.Valor).WithAlias(() => resposta.ValorServico)
                    .Select(() => cteOs.PrecoServico.AReceber).WithAlias(() => resposta.ValorReceber)
                    .Select(() => normal.DescricaoServicoPrestado).WithAlias(() => resposta.DescricaoServico)
                    .Select(() => emissao.Chave).WithAlias(() => resposta.Chave)
                );


            if (textoFiltrado.IsNotNullOrEmpty())
            {
                var dijunction = Restrictions.Disjunction();

                dijunction.Add(Restrictions.Like(Projections.Property(() => normal.DescricaoServicoPrestado),
                    textoFiltrado,
                    MatchMode.Anywhere));

                dijunction.Add(Restrictions.Like(Projections.Property(() => tomador.Nome),
                    textoFiltrado,
                    MatchMode.Anywhere));

                dijunction.Add(Restrictions.Like(Projections.Property(() => tomador.NomeFantasia),
                    textoFiltrado,
                    MatchMode.Anywhere));

                dijunction.Add(Restrictions.Like(Projections.Property(() => emitente.RazaoSocial),
                    textoFiltrado,
                    MatchMode.Anywhere));

                dijunction.Add(Restrictions.Like(Projections.Property(() => emitente.NomeFantasia),
                    textoFiltrado,
                    MatchMode.Anywhere));

                dijunction.Add(Restrictions.Eq(Projections.Property(() => emissao.Chave),
                    textoFiltrado));

                dijunction.Add(Restrictions.Eq(Projections.Cast(NHibernateUtil.String, Projections.Property(() => cteOs.NumeroEmissao)),
                    textoFiltrado));

                query.Where(dijunction);
            }

            var todos = query.TransformUsing(Transformers.AliasToBean<GridCteOsDTO>()).OrderByAlias(() => cteOs.Id).Desc.List<GridCteOsDTO>();

            return todos;
        }


        public override CteOs GetPeloId(int id)
        {
            var cteOs = Sessao.Get<CteOs>(id);

            if (cteOs.Perfil != null)
                if (cteOs.Perfil.Tomador != null)
                {
                    NHibernateUtil.Initialize(cteOs.Perfil.Tomador.Enderecos);
                    NHibernateUtil.Initialize(cteOs.Perfil.Tomador.Emails);
                    NHibernateUtil.Initialize(cteOs.Perfil.Tomador.Telefones);
                }

            if (cteOs.Tomador == null) return cteOs;

            NHibernateUtil.Initialize(cteOs.Tomador.Enderecos);
            NHibernateUtil.Initialize(cteOs.Tomador.Emails);
            NHibernateUtil.Initialize(cteOs.Tomador.Telefones);

            return cteOs;
        }


        public CteOsEmissaoHistorico BuscaUltimaEmissao(CteOs cteOs)
        {
            var historico = Sessao.QueryOver<CteOsEmissaoHistorico>()
                .Where(e => e.CteOs == cteOs && e.Finalizada == false)
                .OrderBy(e => e.Id).Desc
                .SingleOrDefault<CteOsEmissaoHistorico>();

            return historico;
        }

        public void Salvar(CteOsSeguro cteOsSeguro)
        {
            Sessao.SaveOrUpdate(cteOsSeguro);
            Sessao.Flush();
        }

        public void Deletar(CteOsSeguro seguro)
        {
            Sessao.Delete(seguro);
            Sessao.Flush();
        }

        public void Salvar(CteOsPercurso percurso)
        {
            Sessao.SaveOrUpdate(percurso);
            Sessao.Flush();
        }

        public void Deletar(CteOsPercurso percurso)
        {
            Sessao.Delete(percurso);
            Sessao.Flush();
        }

        public void Deletar(CteOsRodoviario rodoviario)
        {
            Sessao.Delete(rodoviario);
            Sessao.Flush();
        }

        public void Deletar(CteOsComponenteValorPrestacao componente)
        {
            Sessao.Delete(componente);
            Sessao.Flush();
        }

        public void Deletar(CteOsDocumentoReferenciado documentoReferenciado)
        {
            Sessao.Delete(documentoReferenciado);
            Sessao.Flush();
        }

        public void Salvar(CteOsEmissaoHistorico historico)
        {
            Sessao.SaveOrUpdate(historico);
            Sessao.Flush();
        }

        public void Salvar(CteOsEmissaoFinalizada emissaoFinalizada)
        {
            Sessao.SaveOrUpdate(emissaoFinalizada);
            Sessao.Flush();
        }

        public void Salvar(CteOsCancelado cancelamento)
        {
            Sessao.SaveOrUpdate(cancelamento);
            Sessao.Flush();
        }

        public IEnumerable<IEnvelope> BuscarXmlExportacao(DateTime inicio, DateTime fim, EmpresaDTO empresa)
        {
            var query = Sessao.QueryOver(() => _tbCte)
                .SelectList(list => list
                    .Select(() => _tbEmissao.Chave).WithAlias(() => _aXml.Chave)
                    .Select(() => _tbEmissao.XmlAutorizado).WithAlias(() => _aXml.Conteudo)
                    .Select(() => _tbCancelamento.StatusResposta).WithAlias(() => _aXml.StatusCancelamento))
                .JoinAlias(() => _tbCte.Emissao, () => _tbEmissao, JoinType.InnerJoin)
                .JoinAlias(() => _tbCte.Emitente, () => _tbEmpresa, JoinType.InnerJoin)
                .JoinAlias(() => _tbCte.Cancelado, () => _tbCancelamento, JoinType.LeftOuterJoin);

            query.TransformUsing(Transformers.AliasToBean<XmlExportacaoCte>());

            var and = Restrictions.Conjunction();

            var pAutorizado = Projections.Property(() => _tbEmissao.Autorizado);
            var pEnviadoEm = Projections.Cast(NHibernateUtil.Date, Projections.Property(() => _tbEmissao.EnviadoEm));
            var pEmpresaId = Projections.Property(() => _tbEmpresa.Id);

            and.Add(Restrictions.Eq(pAutorizado, true));
            and.Add(Restrictions.Between(pEnviadoEm, inicio, fim));
            and.Add(Restrictions.Eq(pEmpresaId, empresa.Id));

            if (BuildMode.IsProducao)
            {
                var pAmbiente = Projections.Property(() => _tbEmissao.AmbienteSefaz);

                and.Add(Restrictions.Eq(pAmbiente, TipoAmbiente.Producao));
            }

            query.Where(and);
            return query.List<XmlExportacaoCte>();
        }

        public void Salvar(CteOsComponenteValorPrestacao componente)
        {
            Sessao.Save(componente);
        }

        public void Salvar(CteOsDocumentoReferenciado documentoReferenciado)
        {
            Sessao.Save(documentoReferenciado);
        }

        public IEnumerable<ICartaCorrecaoCteDTO> ListarCartaCorrecao(ICartaCorrecaoCte correcaoCte)
        {
            CteOsCartaCorrecao cartaCorrecao = null;
            CartaCorrecaoDTO cartaCorrecaoDTO = null;
            CteOs joinCte = null;
            CteOsEmissaoFinalizada emissao = null;

            var query = Sessao.QueryOver(() => cartaCorrecao)
                .Inner.JoinAlias(() => cartaCorrecao.CteOs, () => joinCte)
                .Inner.JoinAlias(() => joinCte.Emissao, () => emissao)
                .SelectList(list =>
                    list.Select(() => cartaCorrecao.Id).WithAlias(() => cartaCorrecaoDTO.Id)
                        .Select(() => cartaCorrecao.OcorreuEm).WithAlias(() => cartaCorrecaoDTO.OcorreuEm)
                        .Select(() => cartaCorrecao.XmlEnvio).WithAlias(() => cartaCorrecaoDTO.XmlEnvio)
                        .Select(() => cartaCorrecao.XmlRetorno).WithAlias(() => cartaCorrecaoDTO.XmlRetorno)
                        .Select(() => cartaCorrecao.ChaveId).WithAlias(() => cartaCorrecaoDTO.ChaveId)
                        .Select(() => emissao.XmlAutorizado).WithAlias(() => cartaCorrecaoDTO.XmlCte))
                .Where(p => p.CteOs.Id == correcaoCte.Id);

            query.TransformUsing(Transformers.AliasToBean<CartaCorrecaoDTO>());

            var lista = query.List<CartaCorrecaoDTO>();

            return lista;
        }

        public byte ObterSequenciaCCe(ICartaCorrecaoCte cte)
        {
            var query = Sessao.QueryOver<CteOsCartaCorrecao>()
                .Where(c => c.CteOs.Id == cte.Id);


            var lista = query.List<CteOsCartaCorrecao>();

            var ultimo = lista.LastOrDefault();

            return (byte)((ultimo?.SequenciaEvento ?? 0) + 1);
        }

        public void SalvarCartaCorrecao(CteOsCartaCorrecao cartaCorrecao)
        {
            Sessao.SaveOrUpdate(cartaCorrecao);
        }
    }
}