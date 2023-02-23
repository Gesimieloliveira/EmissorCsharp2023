using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Debug;
using FusionCore.ExportacaoPacote.Empacotadores;
using FusionCore.FusionAdm.Fiscal.Contratos;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Fiscal.NF.Autorizacao;
using FusionCore.FusionAdm.Fiscal.NF.Pagamentos;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.Filtros;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.NfeEletronica;
using FusionCore.Sintegra.Dto;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

// ReSharper disable RedundantTypeSpecificationInDefaultExpression
// ReSharper disable ImplicitlyCapturedClosure
#pragma warning disable 219

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioNfe : Repositorio<Nfeletronica, int>, IRepositorioExportacaoXml
    {
        public RepositorioNfe(ISession sessao) : base(sessao)
        {
        }

        public override Nfeletronica GetPeloId(int id)
        {
            var nfe = Sessao.Get<Nfeletronica>(id);
            LazyLoad(nfe);

            return nfe;
        }

        private static void LazyLoad(Nfeletronica nfe)
        {
            NHibernateUtil.Initialize(nfe.Itens);
            NHibernateUtil.Initialize(nfe.Referencias);
            NHibernateUtil.Initialize(nfe.ReferenciasCf);
            NHibernateUtil.Initialize(nfe.Volumes);
            NHibernateUtil.Initialize(nfe.Cobranca?.CobrancaDuplicatas);
            NHibernateUtil.Initialize(nfe.Malote);
        }

        public new void Refresh(Nfeletronica nfe)
        {
            Sessao.Refresh(nfe);
            LazyLoad(nfe);
        }

        public void SalvarAlteracoes(Nfeletronica nfe)
        {
            var ehUmaAlteracao = nfe.Id != 0;

            SalvarNfe(nfe);
            SalvarLocalEntrega(nfe);
            SalvarVolumes(nfe);
            SalvarPagamentos(nfe);

            if (ehUmaAlteracao)
            {
                AtualizarIcmsInterestadualSeExistir(nfe);
                FlushDeletaTransportadora(nfe);
            }

            Sessao.Flush();
        }

        private void SalvarLocalEntrega(Nfeletronica nfe)
        {
            Sessao.Delete("from LocalEntrega as le where le.NfeId = ?", nfe.Id, NHibernateUtil.Int32);

            if (nfe.LocalEntrega == null) return;

            Sessao.Save(nfe.LocalEntrega);
        }

        private void SalvarNfe(Nfeletronica nfe)
        {
            if (nfe.Id == 0)
            {
                Sessao.Persist(nfe);
                return;
            }

            Sessao.Update(nfe);
        }

        private void SalvarVolumes(Nfeletronica nfe)
        {
            if (nfe.Volumes == null)
            {
                return;
            }

            foreach (var v in nfe.Volumes)
            {
                if (v.Id == 0)
                {
                    Sessao.Persist(v);
                    continue;
                }

                Sessao.Update(v);
            }
        }

        private void SalvarPagamentos(Nfeletronica nfe)
        {
            foreach (var pg in nfe.Pagamentos)
            {
                if (pg.Id == 0)
                {
                    Sessao.Persist(pg);
                    continue;
                }

                Sessao.Update(pg);
            }
        }

        public void Persistir(ReferenciaNfe referencia)
        {
            Sessao.Persist(referencia);
            Sessao.Flush();
        }

        public void Persistir(ReferenciaCf referencia)
        {
            Sessao.Persist(referencia);
            Sessao.Flush();
        }

        public void Persistir(IVolume volume)
        {
            Sessao.SaveOrUpdate(volume);
            Sessao.Flush();
        }

        public void Persistir(EmissaoNfe emissao)
        {
            Sessao.Persist(emissao);
            Sessao.Flush();
        }

        private void AtualizarIcmsInterestadualSeExistir(Nfeletronica nfe)
        {
            foreach (var item in nfe.Itens)
            {
                if (item.IcmsInterstadual != null)
                {
                    Sessao.Update(item.IcmsInterstadual);
                }
            }
        }

        public void Merge(Nfeletronica nfe)
        {
            Sessao.Merge(nfe);
            FlushDeletaTransportadora(nfe);
        }

        private void FlushDeletaTransportadora(Nfeletronica nfe)
        {
            Sessao.Flush();

            if (nfe.Transportadora != null) return;

            Sessao.Delete($"from {nameof(TransportadoraNfe)} where _nfeId = {nfe.Id}");
            Sessao.Flush();
        }

        public void Alterar(EmissaoNfe emissao)
        {
            Sessao.Update(emissao);
            Sessao.Flush();
        }

        public void Deletar(ReferenciaNfe referencia)
        {
            Sessao.Delete(referencia);
            Sessao.Flush();
        }

        public void Deletar(ReferenciaCf referencia)
        {
            Sessao.Delete(referencia);
            Sessao.Flush();
        }

        public void Deletar(IVolume volume)
        {
            Sessao.Delete($"from {nameof(VolumeNfe)} where id = {volume.Id}");
            Sessao.Flush();
        }

        public void Salvar(IcmsInterstadual icms)
        {
            if (icms.IsNovo())
            {
                Sessao.Persist(icms);
                Sessao.Flush();
                return;
            }

            Sessao.Update(icms);
            Sessao.Flush();
        }

        public void DeletaIcmsInterstadual(ItemNfe item)
        {
            if (item.IcmsInterstadual == null)
                return;

            Sessao.Delete(item.IcmsInterstadual);
            Sessao.Flush();
        }

        public IEnumerable<IEnvelope> BuscarXmlExportacao(DateTime inicio, DateTime fim, EmpresaDTO empresa)
        {
            Nfeletronica tbNfe = null;
            EmissaoFinalizadaNfe tbEmissao = null;
            XmlExportacao alias = null;
            CancelamentoNfe tbCancelamento = null;

            var query = Sessao.QueryOver(() => tbNfe)
                .SelectList(list => list
                    .Select(() => tbEmissao.Chave.Chave).WithAlias(() => alias.Chave)
                    .Select(() => tbEmissao.XmlAutorizado).WithAlias(() => alias.Xml)
                    .Select(() => tbCancelamento.Status).WithAlias(() => alias.Status))
                .JoinAlias(() => tbNfe.Finalizacao, () => tbEmissao, JoinType.InnerJoin)
                .JoinAlias(() => tbNfe.Cancelamento, () => tbCancelamento, JoinType.LeftOuterJoin);

            var and = Restrictions.Conjunction();

            var pData = Projections.Cast(NHibernateUtil.Date, Projections.Property(() => tbEmissao.RecebidoEm));
            var pEmpresa = Projections.Property(() => tbNfe.Emitente.Empresa);

            and.Add(Restrictions.Between(pData, inicio, fim));
            and.Add(Restrictions.Eq(pEmpresa, empresa));

            if (BuildMode.IsProducao)
            {
                var pAmbiente = Projections.Property(() => tbEmissao.TipoAmbiente);

                and.Add(Restrictions.Eq(pAmbiente, TipoAmbiente.Producao));
            }

            query.Where(and);
            query.TransformUsing(Transformers.AliasToBean<XmlExportacao>());

            return query.List<XmlExportacao>();
        }

        public IList<NfeletronicaGrid> BuscaRegistros(NfeGridFiltroBuilder filtro)
        {
            var query = CriaQueryBaseNfeletronicaGrid();
            var conjunction = filtro.Build();

            query.Where(conjunction);
            query.TransformUsing(Transformers.AliasToBean<NfeletronicaGrid>());

            return query.List<NfeletronicaGrid>();
        }

        private IQueryOver<Nfeletronica, Nfeletronica> CriaQueryBaseNfeletronicaGrid()
        {
            NfeletronicaGrid alias = null;

            var subQueryTotal = QueryOver.Of(() => QueryMap.TbItem)
                .Where(i => i.Nfe.Id == QueryMap.TbNfe.Id)
                .Select(Projections.Sum(() => QueryMap.TbItem.TotalFiscal))
                .Take(1);

            var query = Sessao.QueryOver(() => QueryMap.TbNfe)
                .SelectList(list => list
                    .Select(() => QueryMap.TbNfe.Id).WithAlias(() => alias.Id)
                    .Select(() => QueryMap.TbNfe.TipoOperacao).WithAlias(() => alias.TipoOperacao)
                    .Select(() => QueryMap.TbNfe.FinalidadeEmissao).WithAlias(() => alias.FinalidadeEmissao)
                    .Select(() => QueryMap.TbNfe.NumeroEmissao).WithAlias(() => alias.NumeroDocumento)
                    .Select(() => QueryMap.TbNfe.SerieEmissao).WithAlias(() => alias.Serie)
                    .Select(() => QueryMap.TbDestinatario.Nome).WithAlias(() => alias.NomeDestinatario)
                    .Select(() => QueryMap.TbDestinatario.DocumentoUnico).WithAlias(() => alias.DocUnicoDestinatario)
                    .Select(() => QueryMap.TbNfe.EmitidaEm).WithAlias(() => alias.EmitidaEm)
                    .Select(() => QueryMap.TbNfe.StatusAtual).WithAlias(() => alias.StatusAtual)
                    .Select(() => QueryMap.TbFinalizacao.Protocolo).WithAlias(() => alias.ProtocoloAutorizacao)
                    .Select(() => QueryMap.TbEmpresa.RazaoSocial).WithAlias(() => alias.RazaoSocialEmitente)
                    .Select(() => QueryMap.TbFinalizacao.Chave.Chave).WithAlias(() => alias.Chave)
                    .SelectSubQuery(subQueryTotal).WithAlias(() => alias.TotalFiscal))
                .JoinAlias(() => QueryMap.TbNfe.Destinatario, () => QueryMap.TbDestinatario, JoinType.InnerJoin)
                .JoinAlias(() => QueryMap.TbNfe.Finalizacao, () => QueryMap.TbFinalizacao, JoinType.LeftOuterJoin)
                .JoinAlias(() => QueryMap.TbNfe.Emitente.Empresa, () => QueryMap.TbEmpresa, JoinType.InnerJoin);

            return query;
        }
         
        public IList<Registro54NfeDto> BuscarRegistros54(DateTime filtroDataInicio, DateTime filtroDataFinal, EmpresaDTO empresa)
        {
            Nfeletronica nfe = null;
            ItemNfe itemNfe = null;
            ProdutoDTO produto = null;
            ImpostoIcms impostoIcms = null;
            ImpostoIpi impostoIpi = null;
            PerfilCfopDTO perfilCfop = null;
            CfopDTO cfop = null;
            DestinatarioNfe destinatarioNfe = null;
            EmissaoFinalizadaNfe tbEmissao = null;

            Registro54NfeDto registro54NfeDto = null;

            var query = Sessao.QueryOver(() => nfe)
                .JoinAlias(() => nfe.Destinatario, () => destinatarioNfe, JoinType.InnerJoin)
                .JoinAlias(() => nfe.Finalizacao, () => tbEmissao, JoinType.InnerJoin)
                .JoinAlias(Nfeletronica.Expressions.Itens, () => itemNfe, JoinType.InnerJoin)
                .JoinAlias(() => itemNfe.ImpostoIcms, () => impostoIcms, JoinType.InnerJoin)
                .JoinAlias(() => itemNfe.Cfop, () => perfilCfop, JoinType.InnerJoin)
                .JoinAlias(() => perfilCfop.Cfop, () => cfop, JoinType.InnerJoin)
                .JoinAlias(() => itemNfe.Produto, () => produto, JoinType.InnerJoin)
                .JoinAlias(() => itemNfe.Ipi, () => impostoIpi, JoinType.InnerJoin)
                .SelectList(list =>
                    list
                        .Select(() => nfe.Id)
                        .Select(() => destinatarioNfe.DocumentoUnico).WithAlias(() => registro54NfeDto.DocumentoUnico)
                        .Select(() => nfe.SerieEmissao).WithAlias(() => registro54NfeDto.Serie)
                        .Select(() => nfe.NumeroEmissao).WithAlias(() => registro54NfeDto.Numero)
                        .Select(() => cfop.Id).WithAlias(() => registro54NfeDto.Cfop)
                        .Select(() => impostoIcms.Cst.Id).WithAlias(() => registro54NfeDto.Cst)
                        .Select(() => itemNfe.NumeroItem).WithAlias(() => registro54NfeDto.NumeroItem)
                        .Select(() => produto.Id).WithAlias(() => registro54NfeDto.CodigoProdutoServico)
                        .Select(() => itemNfe.Quantidade).WithAlias(() => registro54NfeDto.Quantidade)
                        .Select(() => itemNfe.TotalBruto).WithAlias(() => registro54NfeDto.ValorProduto)
                        .Select(() => itemNfe.TotalDescontoItem).WithAlias(() => registro54NfeDto.ValorTotalDescontos)
                        .Select(() => impostoIcms.ValorBcIcms).WithAlias(() => registro54NfeDto.BaseCalculoIcms)
                        .Select(() => impostoIcms.ValorBcSt).WithAlias(() => registro54NfeDto.BaseCalculoIcmsSt)
                        .Select(() => impostoIpi.ValorIpi).WithAlias(() => registro54NfeDto.ValorIpi)
                        .Select(() => impostoIcms.AliquotaIcms).WithAlias(() => registro54NfeDto.AliquotaIcms)
                        );

            var periodo = new FiltroPeriodo(filtroDataInicio, filtroDataFinal);
            var intervaloMensal = periodo.Restriction(Projections.Property(() => nfe.EmitidaEm));

            var empresaIgual = Restrictions.Eq(Projections.Property(() => nfe.Emitente.Empresa), empresa);

            var and = Restrictions.Conjunction();

            and.Add(intervaloMensal);
            and.Add(empresaIgual);

            query.Where(and);


            query.TransformUsing(Transformers.AliasToBean<Registro54NfeDto>());

            var lista = query.OrderByAlias(() => nfe.Id).Asc.List<Registro54NfeDto>();

            return lista;

        }

        public IList<Registro50NfeDto> BuscarRegistros50(DateTime dataInicio, DateTime dataFinal, EmpresaDTO empresa)
        {
            Nfeletronica nfe = null;
            ItemNfe itemNfe = null;
            ImpostoIcms impostoIcms = null;
            PerfilCfopDTO perfilCfop = null;
            CfopDTO cfop = null;
            DestinatarioNfe destinatarioNfe = null;
            EmissaoFinalizadaNfe tbEmissao = null;
            CancelamentoNfe tbCancelamento = null;

            Registro50NfeDto registro50Dto = null;

            var query = Sessao.QueryOver(() => nfe)
                .JoinAlias(() => nfe.Destinatario, () => destinatarioNfe, JoinType.InnerJoin)
                .JoinAlias(() => nfe.Finalizacao, () => tbEmissao, JoinType.InnerJoin)
                .JoinAlias(Nfeletronica.Expressions.Itens, () => itemNfe, JoinType.InnerJoin)
                .JoinAlias(() => itemNfe.ImpostoIcms, () => impostoIcms, JoinType.InnerJoin)
                .JoinAlias(() => itemNfe.Cfop, () => perfilCfop, JoinType.InnerJoin)
                .JoinAlias(() => perfilCfop.Cfop, () => cfop, JoinType.InnerJoin)
                .JoinAlias(() => nfe.Cancelamento, () => tbCancelamento, JoinType.LeftOuterJoin)
                .SelectList(list =>
                    list
                        .SelectGroup(() => nfe.Id)
                        .SelectGroup(() => destinatarioNfe.DocumentoUnico).WithAlias(() => registro50Dto.DocumentoUnico)
                        .SelectGroup(() => destinatarioNfe.InscricaoEstadual).WithAlias(() => registro50Dto.InscricaoEstadual)
                        .SelectGroup(() => nfe.EmitidaEm).WithAlias(() => registro50Dto.EmitidaEm)
                        .SelectGroup(() => destinatarioNfe.Endereco.Localizacao.SiglaUF).WithAlias(() => registro50Dto.SiglaUf)
                        .SelectGroup(() => nfe.SerieEmissao).WithAlias(() => registro50Dto.Serie)
                        .SelectGroup(() => nfe.NumeroEmissao).WithAlias(() => registro50Dto.Numero)
                        .SelectGroup(() => cfop.Id).WithAlias(() => registro50Dto.Cfop)
                        .SelectGroup(() => impostoIcms.Cst.Id).WithAlias(() => registro50Dto.Csosn)
                        .SelectSum(() => itemNfe.TotalFiscal).WithAlias(() => registro50Dto.ValorTotal)
                        .SelectSum(() => impostoIcms.ValorBcIcms).WithAlias(() => registro50Dto.BaseCalculo)
                        .SelectSum(() => impostoIcms.ValorIcms).WithAlias(() => registro50Dto.ValorIcms)
                        .SelectGroup(() => impostoIcms.AliquotaIcms).WithAlias(() => registro50Dto.AliquotaIcms)
                        .SelectGroup(() => tbCancelamento.Status).WithAlias(() => registro50Dto.Cancelamento)
                        .SelectGroup(() => tbEmissao.IsDenegado).WithAlias(() => registro50Dto.IsDenegado)
                        );


            var periodo = new FiltroPeriodo(dataInicio, dataFinal);
            var intervaloMensal = periodo.Restriction(Projections.Property(() => nfe.EmitidaEm));

            var empresaIgual = Restrictions.Eq(Projections.Property(() => nfe.Emitente.Empresa), empresa);

            var and = Restrictions.Conjunction();

            and.Add(intervaloMensal);
            and.Add(empresaIgual);

            query.Where(and);

            query.TransformUsing(Transformers.AliasToBean<Registro50NfeDto>());

            var lista = query.OrderByAlias(() => nfe.Id).Asc.List<Registro50NfeDto>();

            return lista;
        }

        public IList<Registro75Dto> BuscarRegistros75(DateTime dataInicio, DateTime dataFinal, EmpresaDTO empresa)
        {
            Nfeletronica nfe = null;
            ItemNfe itemNfe = null;
            EmissaoFinalizadaNfe tbEmissao = null;
            ProdutoDTO produto = null;
            ProdutoUnidadeDTO produtoUnidade = null;
            Registro75Dto registro75Dto = null;

            var query = Sessao.QueryOver(() => nfe)
                .JoinAlias(Nfeletronica.Expressions.Itens, () => itemNfe, JoinType.InnerJoin)
                .JoinAlias(() => itemNfe.Produto, () => produto, JoinType.InnerJoin)
                .JoinAlias(() => nfe.Finalizacao, () => tbEmissao, JoinType.InnerJoin)
                .JoinAlias(() => produto.ProdutoUnidadeDTO, () => produtoUnidade, JoinType.InnerJoin)
                .SelectList(
                    list => list.SelectGroup(() => produto.Id).WithAlias(() => registro75Dto.CodigoProdutoServico)
                        .SelectGroup(() => produto.Ncm).WithAlias(() => registro75Dto.CodigoNcm)
                        .SelectGroup(() => produto.Nome).WithAlias(() => registro75Dto.Descricao)
                        .SelectGroup(() => produtoUnidade.Sigla).WithAlias(() => registro75Dto.UnidadeMedida)
                        .SelectGroup(() => produto.AliquotaIpi).WithAlias(() => registro75Dto.AliquotaIpi)
                        .SelectGroup(() => produto.AliquotaIcms).WithAlias(() => registro75Dto.AliquotaIcms)
                        .SelectGroup(() => produto.ReducaoIcms).WithAlias(() => registro75Dto.ReducaoBaseCalculoIcms)
                    );

            var periodo = new FiltroPeriodo(dataInicio, dataFinal);
            var intervaloMensal = periodo.Restriction(Projections.Property(() => nfe.EmitidaEm));

            var empresaIgual = Restrictions.Eq(Projections.Property(() => nfe.Emitente.Empresa), empresa);

            var and = Restrictions.Conjunction();

            and.Add(intervaloMensal);
            and.Add(empresaIgual);

            query.Where(and);

            query.TransformUsing(Transformers.AliasToBean<Registro75Dto>());

            var lista = query.List<Registro75Dto>();

            return lista;
        }

        public IList<Registro53NfeDto> BuscarRegistros53(DateTime filtroDataInicio,
            DateTime filtroDataFinal,
            EmpresaDTO empresa)
        {
            Nfeletronica nfe = null;
            ItemNfe itemNfe = null;
            ImpostoIcms impostoIcms = null;
            PerfilCfopDTO perfilCfop = null;
            CfopDTO cfop = null;
            DestinatarioNfe destinatarioNfe = null;
            EmissaoFinalizadaNfe tbEmissao = null;
            CancelamentoNfe tbCancelamento = null;

            Registro53NfeDto registro53Dto = null;

            //TODO: Repositório montagem do registro 53 - Conferir uso do CST após impl. do Regime Normal

            var query = Sessao.QueryOver(() => nfe)
                .JoinAlias(() => nfe.Destinatario, () => destinatarioNfe)
                .JoinAlias(() => nfe.Finalizacao, () => tbEmissao)
                .JoinAlias(Nfeletronica.Expressions.Itens, () => itemNfe)
                .JoinAlias(() => itemNfe.ImpostoIcms, () => impostoIcms)
                .JoinAlias(() => itemNfe.Cfop, () => perfilCfop)
                .JoinAlias(() => perfilCfop.Cfop, () => cfop)
                .JoinAlias(() => nfe.Cancelamento, () => tbCancelamento, JoinType.LeftOuterJoin)
                .SelectList(list =>
                    list
                        .SelectGroup(() => nfe.Id)
                        .SelectGroup(() => destinatarioNfe.DocumentoUnico).WithAlias(() => registro53Dto.DocumentoUnico)
                        .SelectGroup(() => destinatarioNfe.InscricaoEstadual).WithAlias(() => registro53Dto.InscricaoEstadual)
                        .SelectGroup(() => nfe.EmitidaEm).WithAlias(() => registro53Dto.EmitidaEm)
                        .SelectGroup(() => destinatarioNfe.Endereco.Localizacao.SiglaUF).WithAlias(() => registro53Dto.SiglaUf)
                        .SelectGroup(() => nfe.SerieEmissao).WithAlias(() => registro53Dto.Serie)
                        .SelectGroup(() => nfe.NumeroEmissao).WithAlias(() => registro53Dto.Numero)
                        .SelectGroup(() => cfop.Id).WithAlias(() => registro53Dto.Cfop)
                        .SelectGroup(() => impostoIcms.Cst.Id).WithAlias(() => registro53Dto.Csosn)
                        .SelectSum(() => itemNfe.TotalFiscal).WithAlias(() => registro53Dto.ValorTotal)
                        .SelectSum(() => impostoIcms.ValorBcSt).WithAlias(() => registro53Dto.BaseCalculoSt)
                        .SelectGroup(() => tbCancelamento.Status).WithAlias(() => registro53Dto.Cancelamento)
                        .SelectGroup(() => tbEmissao.IsDenegado).WithAlias(() => registro53Dto.IsDenegado)
                        );


            var or = Restrictions.Disjunction();
            
            or.Add(Restrictions.Eq(Projections.Property(() => impostoIcms.Cst.Id), "201"));
            or.Add(Restrictions.Eq(Projections.Property(() => impostoIcms.Cst.Id), "202"));
            or.Add(Restrictions.Eq(Projections.Property(() => impostoIcms.Cst.Id), "203"));
            or.Add(Restrictions.Eq(Projections.Property(() => impostoIcms.Cst.Id), "500"));
            or.Add(Restrictions.Eq(Projections.Property(() => impostoIcms.Cst.Id), "900"));

            var periodo = new FiltroPeriodo(filtroDataInicio, filtroDataFinal);
            var intervaloMensal = periodo.Restriction(Projections.Property(() => nfe.EmitidaEm));

            var empresaIgual = Restrictions.Eq(Projections.Property(() => nfe.Emitente.Empresa), empresa);

            var and = Restrictions.Conjunction();

            and.Add(intervaloMensal);
            and.Add(empresaIgual);

            query.Where(and);

            query.Where(or);

            query.TransformUsing(Transformers.AliasToBean<Registro53NfeDto>());

            var lista = query.OrderByAlias(() => nfe.Id).Asc.List<Registro53NfeDto>();

            return lista;
        }

        public EmissaoNfe BuscaUltimaEmissaoNfe(Nfeletronica nfe)
        {
            var query = Sessao.QueryOver<EmissaoNfe>()
                .Where(e => e.NfeId == nfe.Id)
                .OrderBy(e => e.Id).Desc
                .Take(1);

            return query.SingleOrDefault();
        }

        public bool PossuiEmissaoPendente(Nfeletronica nfe)
        {
            var query = Sessao.QueryOver<EmissaoNfe>()
                .Where(e => e.Finalizada == false && e.NfeId == nfe.Id);

            return query.RowCount() > 0;
        }

        public IList<NfePickerDTO> BuscarTodosParaPicker()
        {
            Nfeletronica nfeletronicaAlias = null;
            EmissaoFinalizadaNfe emissaoAlias = null;
            CancelamentoNfe canceladaAlias = null;
            EmitenteNfe emitenteAlias = null;
            EmpresaDTO empresaAlias = null;
            DestinatarioNfe destinatarioAlias = null;
            NfePickerDTO resposta = null;

            var query = Sessao.QueryOver(() => nfeletronicaAlias)
                .SelectList(list => list
                    .Select(() => nfeletronicaAlias.Id).WithAlias(() => resposta.Id)
                    .Select(() => emissaoAlias.Chave.Chave).WithAlias(() => resposta.Chave)
                    .Select(() => empresaAlias.RazaoSocial).WithAlias(() => resposta.NomeEmitente)
                    .Select(() => destinatarioAlias.Nome).WithAlias(() => resposta.NomeDestinatario)
                    .Select(() => nfeletronicaAlias.EmitidaEm).WithAlias(() => resposta.EmitidaEm)
                    .Select(() => emissaoAlias.XmlAutorizado).WithAlias(() => resposta.XmlAutorizacao)
                    .Select(() => destinatarioAlias.Endereco.Localizacao.CodigoMunicipio)
                    .WithAlias(() => resposta.MunicipioIbgeDestino))
                .JoinAlias(() => nfeletronicaAlias.Emitente, () => emitenteAlias)
                .JoinAlias(() => emissaoAlias.Empresa, () => empresaAlias)
                .JoinAlias(() => nfeletronicaAlias.Destinatario, () => destinatarioAlias)
                .JoinAlias(() => nfeletronicaAlias.Finalizacao, () => emissaoAlias, JoinType.InnerJoin)
                .JoinAlias(() => nfeletronicaAlias.Cancelamento, () => canceladaAlias, JoinType.LeftOuterJoin);

            query.TransformUsing(Transformers.AliasToBean<NfePickerDTO>());

            var lista = query.OrderByAlias(() => nfeletronicaAlias.Id).Desc.List<NfePickerDTO>();

            return lista.Where(a => a.Status?.EstaCancelado != true).ToList();
        }

        public void Deletar(FormaPagamentoNfe pagamento)
        {
            Sessao.Delete(pagamento);
            Sessao.Flush();
        }
    }
}