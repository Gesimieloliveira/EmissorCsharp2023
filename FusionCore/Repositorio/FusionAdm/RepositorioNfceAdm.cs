using System;
using System.Collections.Generic;
using FusionCore.Cupom.Nfce;
using FusionCore.Debug;
using FusionCore.ExportacaoPacote.Empacotadores;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.ConverterVendaParaNfe;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Nfce;
using FusionCore.FusionAdm.Nfce.SatFiscal;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionNfce.Fiscal.Flags;
using FusionCore.FusionNfce.Fiscal.Tributacoes;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Contratos;
using FusionCore.Repositorio.Filtros;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.SelecionarNfce;
using FusionCore.Sintegra.Dto;
using FusionCore.Sintegra.Registro61RNfce;
using FusionCore.Tributacoes.Estadual;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Util;

// ReSharper disable RedundantBoolCompare

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioNfceAdm : Repositorio<NfceAdm, int>, IRepositorioNfceAdm, IRepositorioExportacaoXml,
        IRepositorioConverterNfe
    {
        public RepositorioNfceAdm(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(NfceAdm nfce)
        {
            Sessao.SaveOrUpdate(nfce);
        }

        public void SalvarItem(NfceItemAdm itemAdm)
        {
            Sessao.SaveOrUpdate(itemAdm);
            Sessao.SaveOrUpdate(itemAdm.ImpostoIcms);
        }

        public void SalvarEmissao(NfceEmissaoAdm emissaoAdm)
        {
            Sessao.SaveOrUpdate(emissaoAdm);
        }

        public void SalvarDestinatario(NfceDestinatarioAdm nfceDestinatarioAdm)
        {
            Sessao.SaveOrUpdate(nfceDestinatarioAdm);
        }

        public void SalvarEmitente(NfceEmitenteAdm emitenteAdm)
        {
            Sessao.SaveOrUpdate(emitenteAdm);
        }

        public void SalvarCancelamento(NfceCancelamentoAdm cancelamentoAdm)
        {
            Sessao.SaveOrUpdate(cancelamentoAdm);
        }

        public void SalvarFormaPagamento(FormaPagamentoNfceAdm pagamentoAdm)
        {
            Sessao.SaveOrUpdate(pagamentoAdm);
        }

        public NfceAdm BuscarNfcePeloUuid(string uuid)
        {
            return Sessao.QueryOver<NfceAdm>().Where(x => x.Uuid == uuid).SingleOrDefault<NfceAdm>();
        }

        public IEnumerable<IEnvelope> BuscarXmlExportacao(DateTime inicio, DateTime fim, EmpresaDTO empresa)
        {
            XmlExportacao alias = null;
            NfceAdm tbNfce = null;
            NfceEmitenteAdm tbEmitente = null;
            NfceEmissaoAdm tbEmissao = null;
            NfceCancelamentoAdm tbCancelamento = null;

            var query = Sessao.QueryOver(() => tbEmitente)
                .SelectList(list => list
                    .Select(() => tbEmissao.Chave).WithAlias(() => alias.Chave)
                    .Select(() => tbEmissao.XmlAutorizado).WithAlias(() => alias.Xml)
                    .Select(() => tbCancelamento.StatusRetorno).WithAlias(() => alias.StatusRetorno))
                .JoinAlias(() => tbEmitente.Nfce, () => tbNfce, JoinType.InnerJoin)
                .JoinAlias(() => tbNfce.Emissao, () => tbEmissao, JoinType.InnerJoin)
                .JoinAlias(() => tbNfce.Cancelamento, () => tbCancelamento, JoinType.LeftOuterJoin);

            var and = Restrictions.Conjunction();

            var projectionAutorizado = Projections.Property(() => tbEmissao.Autorizado);
            var projectionRecebidoEm = Projections.Cast(NHibernateUtil.Date, Projections.Property(() => tbEmissao.RecebidoEm));
            var projectionEmpresa = Projections.Property(() => tbEmitente.Empresa);

            and.Add(Restrictions.Eq(projectionAutorizado, true));
            and.Add(Restrictions.Between(projectionRecebidoEm, inicio, fim));
            and.Add(Restrictions.Eq(projectionEmpresa, empresa));

            if (BuildMode.IsProducao)
            {
                and.Add(Restrictions.Eq(Projections.Property(() => tbEmissao.TipoAmbiente), TipoAmbiente.Producao));
            }

            query.Where(and);
            query.TransformUsing(Transformers.AliasToBean<XmlExportacao>());

            return query.List<XmlExportacao>();
        }

        public void SalvarNfceHistoricoAdm(NfceEmissaoHistoricoAdm historicoAdm)
        {
            Sessao.SaveOrUpdate(historicoAdm);
        }

        public NfceItemAdm BuscarItemPorId(int id)
        {
            return Sessao.Get<NfceItemAdm>(id);
        }

        public NfceEmissaoAdm BuscarEmissaoPeloId(int idRemotoEmissao)
        {
            return Sessao.Get<NfceEmissaoAdm>(idRemotoEmissao);
        }

        public NfceEmitenteAdm BuscaEmitentePeloId(int idRemotoEmitente)
        {
            return Sessao.Get<NfceEmitenteAdm>(idRemotoEmitente);
        }

        public NfceDestinatarioAdm BuscarDestinatarioPeloId(int destinatarioIdRemoto)
        {
            return Sessao.Get<NfceDestinatarioAdm>(destinatarioIdRemoto);
        }

        public NfceCancelamentoAdm BuscarCancelamentoPeloId(int cancelamentoIdRemoto)
        {
            return Sessao.Get<NfceCancelamentoAdm>(cancelamentoIdRemoto);
        }

        public CancelamentoSatAdm BuscaCancelamentoSatPeloId(int cancelamentoSatIdRemoto)
        {
            return Sessao.Get<CancelamentoSatAdm>(cancelamentoSatIdRemoto);
        }

        public IList<NfceDto> BuscaNfceParaConversao(FiltroConversorNfce filtroConversorNfce)
        {
            NfceAdm nfce = null;
            NfceEmitenteAdm emitente = null;
            NfceDestinatarioAdm destinatario = null;
            Cliente cliente = null;
            PessoaEntidade pessoaEntidade = null;
            NfceEmissaoAdm emissao = null;
            EmpresaDTO empresa = null;
            NfceDto nfceDto = null;

            var consulta = Sessao.QueryOver(() => nfce)
                .JoinAlias(() => nfce.Emitente, () => emitente)
                .JoinAlias(() => emitente.Empresa, () => empresa)
                .JoinAlias(() => nfce.Emissao, () => emissao)
                .JoinAlias(() => nfce.Destinatario, () => destinatario, JoinType.LeftOuterJoin)
                .JoinAlias(() => destinatario.Cliente, () => cliente, JoinType.LeftOuterJoin)
                .JoinAlias(() => cliente.Pessoa, () => pessoaEntidade, JoinType.LeftOuterJoin)
                .SelectList(list => 
                list.Select(() => nfce.Id).WithAlias(() => nfceDto.Id)
                    .Select(() => nfce.Serie).WithAlias(() => nfceDto.Serie)
                    .Select(() => nfce.NumeroFiscal).WithAlias(() => nfceDto.NumeroFiscal)
                    .Select(() => nfce.TotalNfce).WithAlias(() => nfceDto.TotalFiscal)
                    .Select(() => nfce.Status).WithAlias(() => nfceDto.Status)
                    .Select(() => empresa.RazaoSocial).WithAlias(() => nfceDto.RazaoSocialEmitente)
                    .Select(() => empresa.Id).WithAlias(() => nfceDto.IdEmitente)
                    .Select(() => nfce.RegimeTributario).WithAlias(() => nfceDto.RegimeTributario)
                    .Select(() => pessoaEntidade.Nome).WithAlias(() => nfceDto.NomeCliente)
                    .Select(() => pessoaEntidade.Id).WithAlias(() => nfceDto.IdCliente));

            consulta.Where(Restrictions.Eq(Projections.Property(() => emissao.Autorizado), true));
            consulta.Where(Restrictions.Eq(Projections.Property(() => nfce.ImportadaParaNfe), filtroConversorNfce.FiltroNfceJaConvertidas));

            if (filtroConversorNfce.TemCliente())
                consulta.Where(Restrictions.Like(Projections.Property(() => pessoaEntidade.Nome),
                    filtroConversorNfce.FiltroNomeDoClienteContenha, MatchMode.Anywhere));

            if (filtroConversorNfce.TemDataInicio() && filtroConversorNfce.NaoTemDataFinal())
                consulta.Where(Restrictions.Ge(Projections.Cast(NHibernateUtil.Date, Projections.Property(() => nfce.EmitidaEm)),
                    filtroConversorNfce.FiltroDataInicio.Value));

            if (filtroConversorNfce.TemDataFinal() && filtroConversorNfce.NaoTemDataInicio())
                consulta.Where(Restrictions.Le(Projections.Cast(NHibernateUtil.Date, Projections.Property(() => nfce.EmitidaEm)),
                    filtroConversorNfce.FiltroDataFinal.Value));

            if (filtroConversorNfce.TemDataInicio() && filtroConversorNfce.TemDataFinal())
                consulta.Where(
                    Restrictions.Between(Projections.Cast(NHibernateUtil.Date, Projections.Property(() => nfce.EmitidaEm))
                , filtroConversorNfce.FiltroDataInicio.Value, filtroConversorNfce.FiltroDataFinal.Value));

            consulta.TransformUsing(Transformers.AliasToBean<NfceDto>());

            var resultadoEmLista = consulta.OrderByAlias(() => nfce.Id).Desc.List<NfceDto>();

            resultadoEmLista.ForEach(item =>
            {
                item.VemDePontoDeVendaNfce();
            });

            return resultadoEmLista;
        }

        public void Deletar(NfceAdm nfceAdm)
        {
            Sessao.Delete(nfceAdm);
        }

        public NfceContingenciaAdm BuscarContingencia(int contingenciaId)
        {
            return Sessao.QueryOver<NfceContingenciaAdm>().Where(x => x.Id == contingenciaId).SingleOrDefault();
        }

        public IList<ItemRegistro61Nfce> BuscarRegistro61(DateTime inicioEm, DateTime finalEm, EmpresaDTO daEmpresa)
        {
            NfceAdm nfce = null;
            NfceEmissaoAdm emissao = null;
            NfceItemAdm item = null;
            NfceImpostoCsosnAdm imposto = null;
            TributacaoCst tributacaoCst = null;
            NfceEmitenteAdm emitente = null;
            EmpresaDTO empresa = null;

            ItemRegistro61Nfce itemRegistro61 = null;

            var consultaSobre = Sessao.QueryOver(() => nfce)
                .JoinAlias(() => nfce.Emissao, () => emissao, JoinType.InnerJoin)
                .JoinAlias(() => nfce.Emitente, () => emitente, JoinType.InnerJoin)
                .JoinAlias(() => emitente.Empresa, () => empresa, JoinType.InnerJoin)
                .JoinAlias(() => nfce.Itens, () => item, JoinType.InnerJoin)
                .JoinAlias(() => item.ImpostoIcms, () => imposto, JoinType.InnerJoin)
                .JoinAlias(() => imposto.CST, () => tributacaoCst, JoinType.InnerJoin)
                .SelectList(list => 
                    list
                        .Select(() => nfce.Id).WithAlias(() => itemRegistro61.Id)
                        .Select(() => nfce.EmitidaEm).WithAlias(() => itemRegistro61.EmissaoNoDia)
                        .Select(() => nfce.Serie).WithAlias(() => itemRegistro61.Serie)
                        .Select(() => nfce.NumeroFiscal).WithAlias(() => itemRegistro61.NumeroFiscal)
                        .Select(() => nfce.TotalNfce).WithAlias(() => itemRegistro61.ValorTotal)
                        .Select(() => tributacaoCst.Id).WithAlias(() => itemRegistro61.Cst));


            consultaSobre.TransformUsing(Transformers.AliasToBean<ItemRegistro61Nfce>());

            var periodo = new FiltroPeriodo(inicioEm, finalEm);
            var intervaloMensal = periodo.Restriction(Projections.Property(() => nfce.EmitidaEm));

            var naoEDenegada = Restrictions.Eq(Projections.Property(() => nfce.Denegada), false);
            var autorizadas = Restrictions.Eq(Projections.Property(() => nfce.Status), Status.Transmitida);

            var empresaIgual = Restrictions.Eq(Projections.Property(() => empresa.Id), daEmpresa.Id);

            var and = Restrictions.Conjunction();
            and.Add(intervaloMensal);
            and.Add(empresaIgual);
            and.Add(naoEDenegada);
            and.Add(autorizadas);

            consultaSobre.Where(and);
            var resultado = consultaSobre.List<ItemRegistro61Nfce>();

            return resultado;
        }

        public IList<Registro61RProdutoDTO> BuscarRegistro61R(DateTime inicioEm, DateTime finalEm, EmpresaDTO daEmpresa)
        {
            NfceAdm nfce = null;
            NfceEmissaoAdm emissao = null;
            NfceItemAdm item = null;
            ProdutoDTO produto = null;
            NfceImpostoCsosnAdm imposto = null;
            TributacaoCst tributacaoCst = null;
            NfceEmitenteAdm emitente = null;
            EmpresaDTO empresa = null;

            Registro61RProdutoDTO registro61RProdutoDTO = null;

            var consultaSobre = Sessao.QueryOver(() => nfce)
                .JoinAlias(() => nfce.Emissao, () => emissao, JoinType.InnerJoin)
                .JoinAlias(() => nfce.Emitente, () => emitente, JoinType.InnerJoin)
                .JoinAlias(() => emitente.Empresa, () => empresa, JoinType.InnerJoin)
                .JoinAlias(() => nfce.Itens, () => item, JoinType.InnerJoin)
                .JoinAlias(() => item.ImpostoIcms, () => imposto, JoinType.InnerJoin)
                .JoinAlias(() => item.Produto, () => produto, JoinType.InnerJoin)
                .JoinAlias(() => imposto.CST, () => tributacaoCst, JoinType.InnerJoin)
                .SelectList(list =>
                    list
                        .SelectGroup(() => produto.Id).WithAlias(() => registro61RProdutoDTO.Codigo)
                        .SelectGroup(() => imposto.AliquotaIcms).WithAlias(() => registro61RProdutoDTO.AliquotaMensal)
                        .SelectSum(() => imposto.BcIcms).WithAlias(() => registro61RProdutoDTO.BaseCalculoIcmsMensal)
                        .SelectSum(() => item.Quantidade).WithAlias(() => registro61RProdutoDTO.QuantidadeMovimentadaMensal)
                        .SelectSum(() => item.ValorTotal).WithAlias(() => registro61RProdutoDTO.ValorBrutoMensal)
                        .SelectSum(() => item.Desconto).WithAlias(() => registro61RProdutoDTO.ValorDescontoMensal)
                        .SelectSum(() => item.Acrescimo).WithAlias(() => registro61RProdutoDTO.ValorAcrescimoMensal));


            consultaSobre.TransformUsing(Transformers.AliasToBean<Registro61RProdutoDTO>());

            var periodo = new FiltroPeriodo(inicioEm, finalEm);
            var intervaloMensal = periodo.Restriction(Projections.Property(() => nfce.EmitidaEm));

            var naoEDenegada = Restrictions.Eq(Projections.Property(() => nfce.Denegada), false);
            var autorizadas = Restrictions.Eq(Projections.Property(() => nfce.Status), Status.Transmitida);

            var empresaIgual = Restrictions.Eq(Projections.Property(() => empresa.Id), daEmpresa.Id);

            var and = Restrictions.Conjunction();
            and.Add(intervaloMensal);
            and.Add(empresaIgual);
            and.Add(naoEDenegada);
            and.Add(autorizadas);

            consultaSobre.Where(and);
            var resultado = consultaSobre.List<Registro61RProdutoDTO>();

            return resultado;
        }

        public IList<Registro75Dto> BuscarRegistro75(DateTime inicioEm, DateTime finalEm, EmpresaDTO daEmpresa)
        {
            NfceAdm nfce = null;
            NfceEmissaoAdm emissao = null;
            NfceItemAdm item = null;
            ProdutoDTO produto = null;
            ProdutoUnidadeDTO unidade = null;
            NfceImpostoCsosnAdm imposto = null;
            TributacaoCst tributacaoCst = null;
            NfceEmitenteAdm emitente = null;
            EmpresaDTO empresa = null;

            Registro75Dto registro75 = null;

            var consultaSobre = Sessao.QueryOver(() => nfce)
                .JoinAlias(() => nfce.Emissao, () => emissao, JoinType.InnerJoin)
                .JoinAlias(() => nfce.Emitente, () => emitente, JoinType.InnerJoin)
                .JoinAlias(() => emitente.Empresa, () => empresa, JoinType.InnerJoin)
                .JoinAlias(() => nfce.Itens, () => item, JoinType.InnerJoin)
                .JoinAlias(() => item.ImpostoIcms, () => imposto, JoinType.InnerJoin)
                .JoinAlias(() => item.Produto, () => produto, JoinType.InnerJoin)
                .JoinAlias(() => produto.ProdutoUnidadeDTO, () => unidade, JoinType.InnerJoin)
                .JoinAlias(() => imposto.CST, () => tributacaoCst, JoinType.InnerJoin)
                .SelectList(list =>
                    list
                        .SelectGroup(() => produto.Id).WithAlias(() => registro75.CodigoProdutoServico)
                        .SelectGroup(() => produto.Ncm).WithAlias(() => registro75.CodigoNcm)
                        .SelectGroup(() => produto.Nome).WithAlias(() => registro75.Descricao)
                        .SelectGroup(() => unidade.Sigla).WithAlias(() => registro75.UnidadeMedida)
                        .SelectGroup(() => produto.AliquotaIpi).WithAlias(() => registro75.AliquotaIpi)
                        .SelectGroup(() => produto.AliquotaIcms).WithAlias(() => registro75.AliquotaIcms)
                        .SelectGroup(() => produto.ReducaoIcms).WithAlias(() => registro75.ReducaoBaseCalculoIcms));


            consultaSobre.TransformUsing(Transformers.AliasToBean<Registro75Dto>());

            var periodo = new FiltroPeriodo(inicioEm, finalEm);
            var intervaloMensal = periodo.Restriction(Projections.Property(() => nfce.EmitidaEm));

            var naoEDenegada = Restrictions.Eq(Projections.Property(() => nfce.Denegada), false);
            var autorizadas = Restrictions.Eq(Projections.Property(() => nfce.Status), Status.Transmitida);

            var empresaIgual = Restrictions.Eq(Projections.Property(() => empresa.Id), daEmpresa.Id);

            var and = Restrictions.Conjunction();
            and.Add(intervaloMensal);
            and.Add(empresaIgual);
            and.Add(naoEDenegada);
            and.Add(autorizadas);

            consultaSobre.Where(and);
            var resultado = consultaSobre.List<Registro75Dto>();

            return resultado;
        }

        public void Merge(NfceAdm nfce)
        {
            Sessao.Merge(nfce);
        }

        public IList<CupomFiscalDto> PesquisarNfces(IFiltroCupomFiscalDto filtroCupomFiscal)
        {
            NfceAdm nfceTabela = null;
            NfceEmitenteAdm emitenteTabela = null;
            EmpresaDTO empresaTabela = null;
            NfceEmissaoAdm emissaoTabela = null;
            NfceDestinatarioAdm destinatarioTabela = null;

            CupomFiscalDto cupomFiscalDto = null;

            var consulta = Sessao.QueryOver(() => nfceTabela)
                .JoinAlias(() => nfceTabela.Emitente, () => emitenteTabela, JoinType.InnerJoin)
                .JoinAlias(() => emitenteTabela.Empresa, () => empresaTabela, JoinType.InnerJoin)
                .JoinAlias(() => nfceTabela.Emissao, () => emissaoTabela, JoinType.LeftOuterJoin)
                .JoinAlias(() => nfceTabela.Destinatario, () => destinatarioTabela, JoinType.LeftOuterJoin)
                .SelectList(listaNfces => listaNfces
                    .Select(() => nfceTabela.Id).WithAlias(() => cupomFiscalDto.Id)
                    .Select(() => nfceTabela.Status).WithAlias(() => cupomFiscalDto.Status)
                    .Select(() => nfceTabela.NumeroFiscal).WithAlias(() => cupomFiscalDto.NumeroFiscal)
                    .Select(() => empresaTabela.RazaoSocial).WithAlias(() => cupomFiscalDto.NomeEmpresa)
                    .Select(() => emissaoTabela.Chave).WithAlias(() => cupomFiscalDto.Chave)
                    .Select(() => nfceTabela.Serie).WithAlias(() => cupomFiscalDto.SerieFiscal)
                    .Select(() => nfceTabela.EmitidaEm).WithAlias(() => cupomFiscalDto.EmitidaEm)
                    .Select(() => nfceTabela.TotalNfce).WithAlias(() => cupomFiscalDto.Total)
                    .Select(() => nfceTabela.Denegada).WithAlias(() => cupomFiscalDto.Denegada)
                    .Select(() => destinatarioTabela.Nome).WithAlias(() => cupomFiscalDto.NomeCliente)
                    .Select(() => emissaoTabela.Autorizado).WithAlias(() => cupomFiscalDto.EmissaoAutorizada)
                    .Select(() => nfceTabela.CriadoEm).WithAlias(() => cupomFiscalDto.CriadoEm)
                );

            if (filtroCupomFiscal.EmitidasIgualOuApos.IsNotNull())
            {
                consulta.Where(Restrictions
                    .Ge(Projections.Cast(NHibernateUtil.Date, Projections.Property(() => nfceTabela.EmitidaEm)),
                        filtroCupomFiscal.EmitidasIgualOuApos));
            }

            if (filtroCupomFiscal.CodigoIdIgualA.IsNotNull())
            {
                consulta.Where(() => nfceTabela.Id == filtroCupomFiscal.CodigoIdIgualA);
            }

            if (filtroCupomFiscal.NumeroIgual.IsNotNull())
            {
                consulta.Where(() => nfceTabela.NumeroFiscal == filtroCupomFiscal.NumeroIgual);
            }

            if (filtroCupomFiscal.NomeEmpresaContenha.IsNotNullOrEmpty())
            {
                consulta.Where(Restrictions.Like(Projections.Property(() => empresaTabela.RazaoSocial),
                    filtroCupomFiscal.NomeEmpresaContenha.TrimOrEmpty(),
                    MatchMode.Anywhere));
            }

            if (filtroCupomFiscal.NomeClienteContenha.IsNotNullOrEmpty())
            {
                consulta.Where(Restrictions.Like(Projections.Property(() => destinatarioTabela.Nome),
                    filtroCupomFiscal.NomeClienteContenha.TrimOrEmpty(), MatchMode.Anywhere));
            }

            if (filtroCupomFiscal.TipoEmissao.IsNotNull())
            {
                consulta.Where(Restrictions.Eq(Projections.Property(() => nfceTabela.TipoEmissao),
                    filtroCupomFiscal.TipoEmissao.Value.ToEmissaoFiscal()));
            }

            if (filtroCupomFiscal.SituacaoFiscal.IsNotNull())
            {
                consulta.Where(Restrictions.Eq(Projections.Property(() => nfceTabela.Status),
                    filtroCupomFiscal.SituacaoFiscal.Value.ToStatus()));
            }

            consulta.TransformUsing(Transformers.AliasToBean<CupomFiscalDto>());

            var todosCupomDtos = consulta.OrderByAlias(() => nfceTabela.CriadoEm).Desc.List<CupomFiscalDto>();

            return todosCupomDtos;
        }

        public string BaixarXmlAutorizado(int nfceId)
        {
            NfceAdm nfceTabela = null;
            NfceEmissaoAdm emissaoTabela = null;

            var consulta = Sessao.QueryOver(() => nfceTabela)
                .JoinAlias(() => nfceTabela.Emissao, () => emissaoTabela)
                .Select(Projections.Property(() => emissaoTabela.XmlAutorizado));

            consulta.Where(() => nfceTabela.Id == nfceId);

            var xmlAutorizado = consulta.SingleOrDefault<string>();

            return xmlAutorizado;
        }

        public string UltimoXmlAssinado(int nfceId)
        {
            NfceEmissaoHistoricoAdm tbHistorico = null;

            var consulta = Sessao.QueryOver(() => tbHistorico)
                .Select(i => i.XmlEnvio)
                .And(i => i.Nfce.Id == nfceId)
                .OrderBy(i => i.Id).Desc
                .Take(1);

            return consulta.FutureValue<string>().Value;
        }
    }
}