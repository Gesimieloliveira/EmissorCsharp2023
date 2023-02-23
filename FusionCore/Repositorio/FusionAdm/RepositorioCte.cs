using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Debug;
using FusionCore.ExportacaoPacote.Empacotadores;
using FusionCore.FusionAdm.CteEletronico.CCe;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionCore.FusionAdm.CteEletronico.Inutilizacao;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.Filtros;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sintegra.Dto;
using FusionCore.Tributacoes.Estadual;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

// ReSharper disable RedundantBoolCompare

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioCte : Repositorio<Cte, int>, IRepositorioExportacaoXml, IRepositorioCartaCorrecaoCte
    {
        public RepositorioCte(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(Cte cte)
        {
            Sessao.SaveOrUpdate(cte);

            Sessao.SaveOrUpdate(cte.CteRemetente);
            Sessao.SaveOrUpdate(cte.CteDestinatario);
            Sessao.SaveOrUpdate(cte.CteTomador);

            if (cte.CteExpedidor != null)
                Sessao.SaveOrUpdate(cte.CteExpedidor);

            if (cte.CteRecebedor != null)
                Sessao.SaveOrUpdate(cte.CteRecebedor);

            if (cte.CteConfigImposto != null) 
                Sessao.SaveOrUpdate(cte.CteConfigImposto);

            if (cte.CteConfigImposto != null)
                Sessao.SaveOrUpdate(cte.CteConfigImposto);

            if (cte.CteImpostoCst != null) 
                Sessao.SaveOrUpdate(cte.CteImpostoCst);

            if (cte.CteImpostoDifal != null) 
                Sessao.SaveOrUpdate(cte.CteImpostoDifal);

            if (cte.CteSubstituicao != null)
                Sessao.SaveOrUpdate(cte.CteSubstituicao);
        }

        public void SalvarDocumentoNfe(CteDocumentoNfe documentoNfe)
        {
            Sessao.SaveOrUpdate(documentoNfe);
        }

        public void SalvarDocumentoImpresso(CteDocumentoImpresso documentoImpresso)
        {
            Sessao.SaveOrUpdate(documentoImpresso);
        }

        public void SalvarDocumentoOutro(CteDocumentoOutro documentoOutro)
        {
            Sessao.SaveOrUpdate(documentoOutro);
        }

        public void SalvarInfoCarga(CteInfoQuantidadeCarga cteInfoQuantidadeCarga)
        {
            Sessao.SaveOrUpdate(cteInfoQuantidadeCarga);
        }

        public void SalvarCteRodoviario(CteRodoviario rodoviario)
        {
            Sessao.SaveOrUpdate(rodoviario);
        }

        public void SalvarVeiculoTransportado(CteVeiculoTransportado veiculo)
        {
            Sessao.SaveOrUpdate(veiculo);
        }

        public void DeletarDocumentoNfe(CteDocumentoNfe documentoNfe)
        {
            Sessao.Delete(documentoNfe);
        }

        public void DeletarDocumentoImpresso(CteDocumentoImpresso documentoImpresso)
        {
            Sessao.Delete(documentoImpresso);
        }

        public void DeletarDocumentoOutro(CteDocumentoOutro documentoOutro)
        {
            Sessao.Delete(documentoOutro);
        }

        public void DeletarInfoQuantidadeCarga(CteInfoQuantidadeCarga infoQuantidadeCarga)
        {
            Sessao.Delete(infoQuantidadeCarga);
        }

        public void DeletaVeiculoTransportado(CteVeiculoTransportado veiculoTransportado)
        {
            Sessao.Delete(veiculoTransportado);
        }

        public void DeletaExpedidor(Cte cte)
        {
            if (cte.CteExpedidor == null)
            {
                return;
            }

            Sessao.Delete(cte.CteExpedidor);
            Sessao.Flush();

            cte.CteExpedidor = null;
        }

        public void DeletaRecebedor(Cte cte)
        {
            if (cte.CteRecebedor == null)
            {
                return;
            }

            Sessao.Delete(cte.CteRecebedor);
            Sessao.Flush();

            cte.CteRecebedor = null;
        }

        public IList<CteGridDto> BuscarTodosParaGrid(CteFiltroGridDto filtros)
        {
            Cte cte = null;
            CteEmissao emissao = null;
            EmpresaDTO empresa = null;
            CteEmitente emitente = null;
            CteTomador tomador = null;
            CteCancelamento cancelamento = null;
            PessoaEntidade tomadorPessoa = null;
            CteDestinatario destinatario = null;
            PessoaEntidade destinatarioPessoa = null;
            CteRemetente remetente = null;
            PessoaEntidade remetentePessoa = null;
            CteGridDto resultado = null;

            var queryOver = Sessao.QueryOver(() => cte)
                .JoinAlias(() => cte.CteEmissao, () => emissao, JoinType.LeftOuterJoin)
                .JoinAlias(() => cte.Cancelamento, () => cancelamento, JoinType.LeftOuterJoin)
                .JoinAlias(() => cte.CteTomador, () => tomador)
                .JoinAlias(() => tomador.Tomador, () => tomadorPessoa)
                .JoinAlias(() => cte.CteDestinatario, () => destinatario)
                .JoinAlias(() => destinatario.Destinatario, () => destinatarioPessoa)
                .JoinAlias(() => cte.CteRemetente, () => remetente)
                .JoinAlias(() => remetente.Remetente, () => remetentePessoa)
                .JoinAlias(() => cte.CteEmitente, () => emitente, JoinType.LeftOuterJoin)
                .JoinAlias(() => emitente.Emitente, () => empresa)
                .SelectList(
                    list => list
                        .Select(() => cte.Id).WithAlias(() => resultado.Id)
                        .Select(() => emissao.Numero).WithAlias(() => resultado.NumeroDocumento)
                        .Select(() => cte.EmissaoEm).WithAlias(() => resultado.EmissaoEm)
                        .Select(() => cte.ValorServico).WithAlias(() => resultado.ValorServico)
                        .Select(() => cte.ValorReceber).WithAlias(() => resultado.ValorReceber)
                        .Select(() => cte.ValorTotalCarga).WithAlias(() => resultado.ValorTotalCarga)
                        .Select(() => cte.NaturezaOperacao).WithAlias(() => resultado.NaturezaOperacao)
                        .Select(() => emissao.Autorizado).WithAlias(() => resultado.Autorizado)
                        .Select(() => emissao.RecebidoEm).WithAlias(() => resultado.RecebidoEm)
                        .Select(() => emissao.CodigoAutorizacao).WithAlias(() => resultado.CodigoAutorizacao)
                        .Select(() => tomadorPessoa.Nome).WithAlias(() => resultado.TomadorNome)
                        .Select(() => destinatarioPessoa.Nome).WithAlias(() => resultado.DestinatarioNome)
                        .Select(() => remetentePessoa.Nome).WithAlias(() => resultado.RemetenteNome)
                        .Select(() => cancelamento.StatusResposta).WithAlias(() => resultado.StatusCancelamento)
                        .Select(() => emissao.XmlAutorizado).WithAlias(() => resultado.XmlAutorizado)
                        .Select(() => cancelamento.XmlRetorno).WithAlias(() => resultado.XmlCancelamento)
                        .Select(() => emissao.Chave).WithAlias(() => resultado.Chave)
                        .Select(() => empresa.RazaoSocial).WithAlias(() => resultado.EmitenteNome)
                        .Select(() => cte.Inutilizado).WithAlias(() => resultado.Inutilizado)
                );

            var dijunction = Restrictions.Disjunction();


            if (filtros.TextoPesquisado.IsNotNullOrEmpty())
            {
                dijunction.Add(Restrictions.Eq(Projections.Cast(NHibernateUtil.String, Projections.Property(() => cte.Id)), filtros.TextoPesquisado));

                dijunction.Add(Restrictions.Eq(Projections.Property(() => emissao.Chave), filtros.TextoPesquisado));

                dijunction.Add(Restrictions.Like(Projections.Property(() => cte.NaturezaOperacao), filtros.TextoPesquisado));

                dijunction.Add(Restrictions.Eq(Projections.Property(() => tomadorPessoa.Cnpj.Valor),
                    filtros.TextoPesquisado));

                dijunction.Add(Restrictions.Eq(Projections.Property(() => tomadorPessoa.Cpf.Valor),
                    filtros.TextoPesquisado));

                dijunction.Add(Restrictions.Like(Projections.Property(() => tomadorPessoa.Nome),
                    filtros.TextoPesquisado, MatchMode.Anywhere));

                dijunction.Add(Restrictions.Like(Projections.Property(() => tomadorPessoa.NomeFantasia),
                    filtros.TextoPesquisado, MatchMode.Anywhere));

                dijunction.Add(Restrictions.Eq(Projections.Property(() => destinatarioPessoa.Cnpj.Valor),
                    filtros.TextoPesquisado));

                dijunction.Add(Restrictions.Eq(Projections.Property(() => destinatarioPessoa.Cpf.Valor),
                    filtros.TextoPesquisado));

                dijunction.Add(Restrictions.Like(Projections.Property(() => destinatarioPessoa.Nome),
                    filtros.TextoPesquisado, MatchMode.Anywhere));

                dijunction.Add(Restrictions.Like(Projections.Property(() => destinatarioPessoa.NomeFantasia),
                    filtros.TextoPesquisado, MatchMode.Anywhere));

                dijunction.Add(Restrictions.Eq(Projections.Property(() => remetentePessoa.Cnpj.Valor),
                    filtros.TextoPesquisado));

                dijunction.Add(Restrictions.Eq(Projections.Property(() => remetentePessoa.Cpf.Valor),
                    filtros.TextoPesquisado));

                dijunction.Add(Restrictions.Like(Projections.Property(() => remetentePessoa.Nome),
                    filtros.TextoPesquisado, MatchMode.Anywhere));

                dijunction.Add(Restrictions.Like(Projections.Property(() => remetentePessoa.NomeFantasia),
                    filtros.TextoPesquisado, MatchMode.Anywhere));

                dijunction.Add(Restrictions.Eq(Projections.Property(() => remetentePessoa.Cnpj.Valor),
                    filtros.TextoPesquisado));

                dijunction.Add(Restrictions.Eq(Projections.Property(() => remetentePessoa.Cpf.Valor),
                    filtros.TextoPesquisado));

                dijunction.Add(Restrictions.Like(Projections.Property(() => remetentePessoa.Nome),
                    filtros.TextoPesquisado, MatchMode.Anywhere));

                dijunction.Add(Restrictions.Like(Projections.Property(() => remetentePessoa.NomeFantasia),
                    filtros.TextoPesquisado, MatchMode.Anywhere));

                dijunction.Add(Restrictions.Eq(Projections.Property(() => empresa.Cnpj),
                    filtros.TextoPesquisado));

                dijunction.Add(Restrictions.Like(Projections.Property(() => empresa.RazaoSocial),
                    filtros.TextoPesquisado, MatchMode.Anywhere));

                dijunction.Add(Restrictions.Like(Projections.Property(() => empresa.NomeFantasia),
                    filtros.TextoPesquisado, MatchMode.Anywhere));
            }

            if (filtros.DataEmissaoInicial != null || filtros.DataEmissaoFinal != null)
            {
                var conjunctionData = Restrictions.Conjunction();

                conjunctionData.Add(Restrictions.Ge(Projections.Cast(NHibernateUtil.Date, Projections.Property(() => emissao.EmitidaEm)), filtros.DataEmissaoInicial));

                conjunctionData.Add(Restrictions.Le(Projections.Cast(NHibernateUtil.Date, Projections.Property(() => emissao.EmitidaEm)), filtros.DataEmissaoFinal));

                dijunction.Add(conjunctionData);
            }

            if (filtros.NumeroDocumento.IsNotNullOrEmpty())
            {
                dijunction.Add(Restrictions.Like(Projections.Cast(NHibernateUtil.String, Projections.Property(() => emissao.Numero)),
                    filtros.NumeroDocumento));
            }

            if (filtros.Status == CteStatus.Autorizado)
            {
                dijunction.Add(Restrictions.And(Restrictions.Eq(Projections.Property(() => emissao.CodigoAutorizacao), 100), 
                    Restrictions.IsNull(Projections.Property(() => cancelamento.Cte))));
            }

            if (filtros.Status == CteStatus.EmDigitacao)
            {

                var dijunctionStatus = Restrictions.Disjunction();

                dijunctionStatus.Add(Restrictions.IsNull(Projections.Property(() => emissao.Cte)));

                dijunction.Add(dijunctionStatus);
            }

            if (filtros.Status == CteStatus.Cancelada)
            {
                var dijunctionCancelada = Restrictions.Disjunction();

                dijunctionCancelada.Add(Restrictions.Eq(Projections.Property(() => cancelamento.StatusResposta), 135));
                dijunctionCancelada.Add(Restrictions.Eq(Projections.Property(() => cancelamento.StatusResposta), 136));
                dijunctionCancelada.Add(Restrictions.Eq(Projections.Property(() => cancelamento.StatusResposta), 134));

                dijunction.Add(dijunctionCancelada);
            }

            if (filtros.Status == CteStatus.Pendente)
            {
                var conjunctionPendete = Restrictions.Conjunction();

                conjunctionPendete.Add(Restrictions.Not(
                    Restrictions.Eq(Projections.Property(() => emissao.CodigoAutorizacao), 100)));

                conjunctionPendete.Add(Restrictions.Not(
                    Restrictions.Eq(Projections.Property(() => emissao.CodigoAutorizacao), 0)));

                dijunction.Add(conjunctionPendete);
            }


            if (filtros.IsEfetuaFiltro())
            {
                queryOver.Where(dijunction);
            }

            queryOver.TransformUsing(Transformers.AliasToBean<CteGridDto>());

            var lista = queryOver
                .OrderBy(() => cte.Id).Desc.ThenBy(() => emissao.EmitidaEm).Desc.List<CteGridDto>();

            return lista;
        }

        public override Cte GetPeloId(int id)
        {
            var cte = base.GetPeloId(id);

            InicializaObjetoFilhos(cte);

            return cte;
        }

        private static void InicializaObjetoFilhos(Cte cte)
        {
            NHibernateUtil.Initialize(cte.CteRemetente.Remetente.Enderecos);
            NHibernateUtil.Initialize(cte.CteRemetente.Remetente.Telefones);

            NHibernateUtil.Initialize(cte.CteDestinatario.Destinatario.Enderecos);
            NHibernateUtil.Initialize(cte.CteDestinatario.Destinatario.Telefones);

            NHibernateUtil.Initialize(cte.CteTomador.Tomador.Enderecos);
            NHibernateUtil.Initialize(cte.CteTomador.Tomador.Telefones);

            NHibernateUtil.Initialize(cte.CteExpedidor?.Expedidor?.Enderecos);
            NHibernateUtil.Initialize(cte.CteExpedidor?.Expedidor?.Telefones);

            NHibernateUtil.Initialize(cte.CteRecebedor?.Recebedor?.Enderecos);
            NHibernateUtil.Initialize(cte.CteRecebedor?.Recebedor?.Telefones);
        }

        public void SalvarEmissao(CteEmissao emissao)
        {
            Sessao.SaveOrUpdate(emissao);
            Sessao.Flush();
        }

        public void SalvarCancelamento(CteCancelamento cancelamento)
        {
            Sessao.SaveOrUpdate(cancelamento);
        }

        public void SalvarCteEmitente(CteEmitente emitente)
        {
            Sessao.SaveOrUpdate(emitente);
        }

        public void SalvarCteInutilizacao(CteInutilizacao inutilizacao)
        {
            Sessao.SaveOrUpdate(inutilizacao);
        }

        public byte ObterSequenciaCCe(ICartaCorrecaoCte cte)
        {
            var query = Sessao.QueryOver<CteCartaCorrecao>() 
                .Where(c => c.Cte.Id == cte.Id);


            var lista = query.List<CteCartaCorrecao>();

            var ultimo = lista.LastOrDefault();

            return (byte) ((ultimo?.SequenciaEvento ?? 0) + 1);
        }

        public void SalvarCartaCorrecao(CteCartaCorrecao cartaCorrecao)
        {
            Sessao.SaveOrUpdate(cartaCorrecao);
        }

        public void SalvarInformacaoCorrecao(CteInformacaoCorrecao correcao)
        {
            Sessao.SaveOrUpdate(correcao);
        }

        public IList<CartaCorrecaoDTO> BuscarHistoricoDeCorrecoes(ICartaCorrecaoCte correcaoCte)
        {
            CteCartaCorrecao cartaCorrecao = null;
            CartaCorrecaoDTO cartaCorrecaoDTO = null;
            Cte joinCte = null;
            CteEmissao emissao = null;

            var query = Sessao.QueryOver(() => cartaCorrecao)
                .Inner.JoinAlias(() => cartaCorrecao.Cte, () => joinCte)
                .Inner.JoinAlias(() => joinCte.CteEmissao, () => emissao)
                .SelectList(list =>
                    list.Select(() => cartaCorrecao.Id).WithAlias(() => cartaCorrecaoDTO.Id)
                        .Select(() => cartaCorrecao.OcorreuEm).WithAlias(() => cartaCorrecaoDTO.OcorreuEm)
                        .Select(() => cartaCorrecao.XmlEnvio).WithAlias(() => cartaCorrecaoDTO.XmlEnvio)
                        .Select(() => cartaCorrecao.XmlRetorno).WithAlias(() => cartaCorrecaoDTO.XmlRetorno)
                        .Select(() => cartaCorrecao.ChaveId).WithAlias(() => cartaCorrecaoDTO.ChaveId)
                        .Select(() => emissao.XmlAutorizado).WithAlias(() => cartaCorrecaoDTO.XmlCte))
                .Where(p => p.Cte.Id == correcaoCte.Id);

            query.TransformUsing(Transformers.AliasToBean<CartaCorrecaoDTO>());

            var lista = query.List<CartaCorrecaoDTO>();

            return lista;
        }

        public IList<CteInutilizacaoDTO> BuscarInutilizacoes()
        {
            CteInutilizacao inutilizacao = null;
            CteInutilizacaoDTO resultado = null;

            var query = Sessao.QueryOver(() => inutilizacao)
                .SelectList(list =>
                    list.Select(() => inutilizacao.Id).WithAlias(() => resultado.Id)
                        .Select(() => inutilizacao.NumeroInicial).WithAlias(() => resultado.NumeroInicial)
                        .Select(() => inutilizacao.NumeroFinal).WithAlias(() => resultado.NumeroFinal)
                        .Select(() => inutilizacao.Justificativa).WithAlias(() => resultado.Justificativa)
                        .Select(() => inutilizacao.ModeloDocumento).WithAlias(() => resultado.Documento)
                );

            query.TransformUsing(Transformers.AliasToBean<CteInutilizacaoDTO>());

            var lista = query.List<CteInutilizacaoDTO>();

            return lista;
        }

        public IEnumerable<IEnvelope> BuscarXmlExportacao(DateTime inicio, DateTime fim, EmpresaDTO empresa)
        {
            Cte tbCte = null;
            CteEmissao tbEmissao = null;
            CteCancelamento tbCancelamento = null;
            CteEmitente tbEmitente = null;
            XmlExportacaoCte xml = null;

            var query = Sessao.QueryOver(() => tbCte)
                .SelectList(list => list
                    .Select(() => tbEmissao.Chave).WithAlias(() => xml.Chave)
                    .Select(() => tbEmissao.XmlAutorizado).WithAlias(() => xml.Conteudo)
                    .Select(() => tbCancelamento.StatusResposta).WithAlias(() => xml.StatusCancelamento))
                .JoinAlias(() => tbCte.CteEmissao, () => tbEmissao, JoinType.InnerJoin)
                .JoinAlias(() => tbCte.CteEmitente, () => tbEmitente, JoinType.InnerJoin)
                .JoinAlias(() => tbCte.Cancelamento, () => tbCancelamento, JoinType.LeftOuterJoin);

            query.TransformUsing(Transformers.AliasToBean<XmlExportacaoCte>());

            var and = Restrictions.Conjunction();

            var pAutorizado = Projections.Property(() => tbEmissao.Autorizado);
            var pData = Projections.Cast(NHibernateUtil.Date, Projections.Property(() => tbEmissao.RecebidoEm));
            var pEmpresa = Projections.Property(() => tbEmitente.Emitente);

            and.Add(Restrictions.Eq(pAutorizado, true));
            and.Add(Restrictions.Between(pData, inicio, fim));
            and.Add(Restrictions.Eq(pEmpresa, empresa));

            if (BuildMode.IsProducao)
            {
                and.Add(Restrictions.Eq(Projections.Property(() => tbEmissao.Ambiente), TipoAmbiente.Producao));
            }

            query.Where(and);

            return query.List<XmlExportacaoCte>();
        }

        public IList<Registro70CteDto> BuscarRegistro70CteDtos(DateTime filtroDataInicio, DateTime filtroDataFinal, EmpresaDTO empresa)
        {
            Cte cte = null;
            CteTomador cteTomador = null;
            CteEmitente cteEmitente = null;
            PessoaEntidade tomadorPessoa = null;
            PessoaEndereco pessoaEndereco = null;
            CidadeDTO cidade = null;
            CteEmissao cteEmissao = null;
            CteCancelamento cteCancelamento = null;
            CteImpostoCst cteImpostoCst = null;
            TributacaoIcms tributacaoIcms = null;
            PerfilCfopDTO perfilCfop = null;
            CfopDTO cfop = null;

            Registro70CteDto alias = null;


            var subQuerySiglaUf = QueryOver.Of(() => tomadorPessoa)
                .JoinAlias(() => tomadorPessoa.Enderecos, () => pessoaEndereco, JoinType.InnerJoin)
                .JoinAlias(() => pessoaEndereco.Cidade, () => cidade, JoinType.InnerJoin)
                .Where(i => i.Id == tomadorPessoa.Id).SelectList(list =>
                    list.SelectGroup(() => cidade.SiglaUf).WithAlias(() => alias.SiglaUf)).Take(1);

            var queryOver = Sessao.QueryOver(() => cte)
                .JoinAlias(() => cte.CteTomador, () => cteTomador, JoinType.FullJoin)
                .JoinAlias(() => cteTomador.Tomador, () => tomadorPessoa, JoinType.InnerJoin)
                .JoinAlias(() => cte.CteEmissao, () => cteEmissao, JoinType.InnerJoin)
                .JoinAlias(() => cte.Cancelamento, () => cteCancelamento, JoinType.InnerJoin)
                .JoinAlias(() => cte.CteImpostoCst, () => cteImpostoCst, JoinType.InnerJoin)
                .JoinAlias(() => cteImpostoCst.TributacaoIcms, () => tributacaoIcms)
                .JoinAlias(() => cte.PerfilCfop, () => perfilCfop, JoinType.InnerJoin)
                .JoinAlias(() => perfilCfop.Cfop, () => cfop, JoinType.InnerJoin)
                .JoinAlias(() => cte.CteEmitente, () => cteEmitente, JoinType.InnerJoin);

            queryOver.SelectList(list =>
                list
                    .Select(() => tomadorPessoa.Cnpj).WithAlias(() => alias.Cnpj)
                    .Select(() => tomadorPessoa.Cpf).WithAlias(() => alias.Cpf)
                    .Select(() => tomadorPessoa.InscricaoEstadual).WithAlias(() => alias.InscricaoEstadual)
                    .Select(() => cte.EmissaoEm).WithAlias(() => alias.EmissaoEm)
                    .SelectSubQuery(subQuerySiglaUf)
                    .Select(() => cteEmissao.Serie).WithAlias(() => alias.Serie)
                    .Select(() => cteEmissao.Numero).WithAlias(() => alias.Numeracao)
                    .Select(() => cfop.Id).WithAlias(() => alias.Cfop)
                    .Select(() => cte.ValorServico).WithAlias(() => alias.ValorTotalDocumentoFiscal)
                    .Select(() => cteImpostoCst.BaseCalculoIcms).WithAlias(() => alias.BaseCalculoIcms)
                    .Select(() => cteImpostoCst.ValorIcms).WithAlias(() => alias.ValorIcms)
                    .Select(() => cteCancelamento.StatusResposta).WithAlias(() => alias.StatusCancelamento)
                    .Select(() => tributacaoIcms.Codigo).WithAlias(() => alias.Cst)
                );

            var periodo = new FiltroPeriodo(filtroDataInicio, filtroDataFinal);
            var intervaloMensal = periodo.Restriction(Projections.Property(() => cte.EmissaoEm));

            var empresaIgual = Restrictions.Eq(Projections.Property(() => cteEmitente.Emitente), empresa);

            var and = Restrictions.Conjunction();

            and.Add(intervaloMensal);
            and.Add(empresaIgual);

            queryOver.Where(and);

            queryOver.TransformUsing(Transformers.AliasToBean<Registro70CteDto>());

            var lista = queryOver.OrderByAlias(() => cte.Id).Asc.List<Registro70CteDto>();

            return lista;
        }

        public IList<CtePickerDTO> BuscarTodosParaPicker()
        {
            Cte cteAlias = null;
            CteEmissao emissaoAlias = null;
            CteRemetente cteRemetenteAlias = null;
            CteDestinatario cteDestinatarioAlias = null;
            CteTomador cteTomadorAlias = null;
            PessoaEntidade destinatarioAlias = null;
            PessoaEntidade tomadorAlias = null;
            PessoaEntidade remetenteAlias = null;
            CteEmitente cteEmitenteAlias = null;
            EmpresaDTO empresaAlias = null;
            CteCancelamento cancelamentoAlias = null;
            CidadeDTO cidadeFinal = null;

            CtePickerDTO resultado = null;

            var queryOver = Sessao.QueryOver(() => cteAlias)
                .JoinAlias(() => cteAlias.CteEmissao, () => emissaoAlias)
                .JoinAlias(() => cteAlias.CteDestinatario, () => cteDestinatarioAlias)
                .JoinAlias(() => cteDestinatarioAlias.Destinatario, () => destinatarioAlias)
                .JoinAlias(() => cteAlias.CteEmitente, () => cteEmitenteAlias)
                .JoinAlias(() => cteEmitenteAlias.Emitente, () => empresaAlias)
                .JoinAlias(() => cteAlias.CteTomador, () => cteTomadorAlias)
                .JoinAlias(() => cteTomadorAlias.Tomador, () => tomadorAlias)
                .JoinAlias(() => cteAlias.CteRemetente, () => cteRemetenteAlias)
                .JoinAlias(() => cteRemetenteAlias.Remetente, () => remetenteAlias)
                .JoinAlias(() => cteAlias.Cancelamento, () => cancelamentoAlias, JoinType.LeftOuterJoin)
                .JoinAlias(() => cteAlias.CidadeFinalOperacao, () => cidadeFinal, JoinType.InnerJoin)
                .SelectList(list => list
                    .Select(() => cteAlias.Id).WithAlias(() => resultado.Id)
                    .Select(() => cteAlias.CidadeFinalOperacao).WithAlias(() => resultado.CidadeFinalOperacao)
                    .Select(() => emissaoAlias.Chave).WithAlias(() => resultado.Chave)
                    .Select(() => destinatarioAlias.Nome).WithAlias(() => resultado.NomeDestinatario)
                    .Select(() => empresaAlias.RazaoSocial).WithAlias(() => resultado.NomeEmitente)
                    .Select(() => tomadorAlias.Nome).WithAlias(() => resultado.NomeTomador)
                    .Select(() => remetenteAlias.Nome).WithAlias(() => resultado.NomeRemetente)
                    .Select(() => cteAlias.ValorServico).WithAlias(() => resultado.ValorServico)
                    .Select(() => cancelamentoAlias.StatusResposta).WithAlias(() => resultado.StatusCancelamento)
                    .Select(() => emissaoAlias.CodigoAutorizacao).WithAlias(() => resultado.CodigoAutorizacao)
                    .Select(() => emissaoAlias.EmitidaEm).WithAlias(() => resultado.EmitidaEm)
                );


            queryOver.TransformUsing(Transformers.AliasToBean<CtePickerDTO>());

            var lista = queryOver.OrderByAlias(() => cteAlias.Id).Desc.List<CtePickerDTO>().Where(
                x => x.StatusCancelamento != 135 && x.StatusCancelamento != 136 && x.StatusCancelamento != 134).ToList();

            lista = lista.Where(x => x.CodigoAutorizacao != 0 && x.CodigoAutorizacao == 100).ToList();

            return lista;
        }

        public void Refresh(Cte cteRefresh)
        {
            Sessao.Refresh(cteRefresh);
            InicializaObjetoFilhos(cteRefresh);
        }

        public void SalvarDocumentoAnteior(CteDocumentoAnterior documentoAnterior)
        {
            Sessao.Save(documentoAnterior);
        }

        public void DeletarAnterior(CteDocumentoAnterior documentoAnterior)
        {
            Sessao.Delete(documentoAnterior);
        }

        public void SalvarComponente(CteComponenteValorPrestacao componente)
        {
            Sessao.Save(componente);
        }

        public void DeletarComponente(CteComponenteValorPrestacao componente)
        {
            Sessao.Delete(componente);
        }

        public IEnumerable<ICartaCorrecaoCteDTO> ListarCartaCorrecao(ICartaCorrecaoCte correcaoCte)
        {
            return BuscarHistoricoDeCorrecoes(correcaoCte);
        }

        public void DeletarCteSubstituto(int id)
        {
            Sessao.Delete($"from {nameof(CteSubstituicao)} as C where C.CteId = {id}");
        }

        public CteEmissaoHistorico BuscaUltimaEmissaoHistorico(Cte cte)
        {
            var query = Sessao.QueryOver<CteEmissaoHistorico>()
                .Where(e => e.Cte.Id == cte.Id)
                .Where(e => e.Finalizada == false);

            return query.SingleOrDefault();
        }

        public bool PossuiEmissaoPendente(Cte cte)
        {
            var query = Sessao.QueryOver<CteEmissaoHistorico>()
                .Where(e => e.Finalizada == false && e.Cte.Id == cte.Id);

            return query.RowCount() > 0;
        }

        public void SalvarEmissaoHistorico(CteEmissaoHistorico historico)
        {
            Sessao.SaveOrUpdate(historico);
        }

        public void AtualizarEmissor(EmissorFiscal emissor)
        {
            Sessao.Refresh(emissor);
        }
    }
}