using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.Cidade;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Fiscal.Flags;
using FusionCore.FusionNfce.Fiscal.SatFiscal;
using FusionCore.FusionNfce.Pagamento;
using FusionCore.FusionNfce.Vo;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

// ReSharper disable RedundantBoolCompare

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioNfce : Repositorio<Nfce, int>, IRepositorioNfce
    {
        public RepositorioNfce(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(Nfce nfce)
        {
            Sessao.SaveOrUpdate(nfce);

            if (nfce.Emissao != null)
            {
                SalvarEmissaoSemTransacao(nfce.Emissao);
            }

            if (nfce.Emitente != null)
            {
                SalvarEmitente(nfce.Emitente);
            }

            if (nfce.Destinatario != null)
            {
                SalvarDestinatario(nfce.Destinatario);
            }

            if (nfce.Cancelamento != null)
            {
                SalvarCancelamento(nfce.Cancelamento);
            }

            if (nfce.Contingencia != null)
            {
                SalvarContingencia(nfce.Contingencia);
            }
        }

        public IList<NfceOpcoesVo> BuscaElegiveisParaRecuperacao()
        {
            Nfce nfce = null;

            var query = CriaQueryNfceOpcoesVo(JoinType.LeftOuterJoin, JoinType.LeftOuterJoin);
            
            var conjunctionOffline = Restrictions.Conjunction();

            conjunctionOffline.Add(Restrictions.Eq(Projections.Property(() => nfce.TipoEmissao), TipoEmissao.ContigenciaOfflineNFCe));
            conjunctionOffline.Add(Restrictions.Or(Restrictions.Eq(Projections.Property(() => nfce.Status), Status.Aberta), Restrictions.Eq(Projections.Property(() => nfce.Status), Status.PendenteOffline)));

            var conjutionOutras = Restrictions.Conjunction();

            conjutionOutras.Add(Restrictions.Eq(Projections.Property(() => nfce.Status), Status.Aberta));
            
            query.Where(conjunctionOffline || conjutionOutras);
            query.TransformUsing(Transformers.AliasToBean<NfceOpcoesVo>());

            return query.List<NfceOpcoesVo>();
        }

        public int BuscaQuantidadeElegiveisParaRecuperacao(Nfce nfceExcluida, DateTime? data = null)
        {
            Nfce nfce = null;

            var query = Sessao.QueryOver(() => nfce);

            var conjunctionNormal = Restrictions.Conjunction();

            conjunctionNormal.Add(Restrictions.Eq(Projections.Property(() => nfce.TipoEmissao), TipoEmissao.Normal));
            conjunctionNormal.Add(Restrictions.Eq(Projections.Property(() => nfce.Status), Status.Aberta));

            if (nfceExcluida != null)
                conjunctionNormal.Add(Restrictions.Not(Restrictions.Eq(Projections.Property(() => nfce.Id), nfceExcluida.Id)));


            if (data != null)
            {
                var conjunctionPorDataInicialEFinal = Restrictions.Conjunction();

                conjunctionPorDataInicialEFinal.Add(conjunctionNormal);
                conjunctionPorDataInicialEFinal.Add(Restrictions.Le(Projections.Property(() => nfce.CriadoEm), data));

                query.Where(conjunctionPorDataInicialEFinal);

                return query.RowCount();
            }

            query.Where(conjunctionNormal);

            return query.RowCount();
        }

        private IQueryOver<Nfce, Nfce> CriaQueryNfceOpcoesVo(JoinType joinEmissao, JoinType joinCancelamento)
        {
            Nfce nfce = null;
            NfceCancelamento cancelamento = null;
            NfceEmissao emissao = null;
            NfceOpcoesVo alias = null;

            var query = Sessao.QueryOver(() => nfce)
                .SelectList(list => list
                    .Select(() => nfce.Id).WithAlias(() => alias.Id)
                    .Select(() => nfce.NumeroFiscal).WithAlias(() => alias.NumeroDocumento)
                    .Select(() => nfce.Serie).WithAlias(() => alias.Serie)
                    .Select(() => emissao.Chave).WithAlias(() => alias.Chave)
                    .Select(() => emissao.Autorizado).WithAlias(() => alias.Autorizado)
                    .Select(() => nfce.TotalNfce).WithAlias(() => alias.Total)
                    .Select(() => nfce.TipoEmissao).WithAlias(() => alias.TipoEmissao)
                    .Select(() => nfce.EmitidaEm).WithAlias(() => alias.EmitidaEm)
                    .Select(() => emissao.RecebidoEm).WithAlias(() => alias.AutorizadaEm)
                    .Select(() => nfce.Status).WithAlias(() => alias.Status))
                .JoinAlias(() => nfce.Emissao, () => emissao, joinEmissao)
                .JoinAlias(() => nfce.Cancelamento, () => cancelamento, joinCancelamento);

            return query;
        }

        public IList<Nfce> BuscaNfceOfflineComErrosNaEmissao()
        {
            Nfce nfce = null;

            var lista = Sessao.QueryOver(() => nfce)
                .Select(n => n.Id, n => n.EmitidaEm)
                .Where(() => nfce.TipoEmissao == TipoEmissao.ContigenciaOfflineNFCe
                             && nfce.Status == Status.PendenteOffline)
                .List<object[]>()
                .Select(p => new Nfce
                {
                    Id = (int) p[0],
                    EmitidaEm = (DateTime) p[1]
                }).ToList();

            return lista;
        }

        public void SalvarItemESincronizar(NfceItem item)
        {
            Sessao.SaveOrUpdate(item);

            SalvarESincronizar(item.Nfce);
        }

        public void SalvarItem(NfceItem item)
        {
            Sessao.SaveOrUpdate(item);

            item.Nfce.Sincronizado = true;
            Salvar(item.Nfce);
        }

        public void SalvarEmissao(NfceEmissao emissao)
        {
            Sessao.SaveOrUpdate(emissao);
        }

        public void SalvarDestinatario(NfceDestinatario destinatario)
        {
            Sessao.SaveOrUpdate(destinatario);
        }

        public void SalvarCancelamentoSat(CancelamentoSat cancelamentoSat)
        {
            Sessao.SaveOrUpdate(cancelamentoSat);
        }

        public IList<NfceOpcoesVo> BuscarParaOpcoes(string inputFiltro = "", DateTime? dataEmissaoInicial = null, DateTime? dataEmissaoFinal = null)
        {
            Nfce nfce = null;
            NfceCancelamento cancelamento = null;
            NfceEmissao emissao = null;

            var query = CriaQueryNfceOpcoesVo(JoinType.InnerJoin, JoinType.LeftOuterJoin)
                .OrderBy(() => nfce.Id).Desc;

            var disjunction = Restrictions.Disjunction();

            disjunction.Add(Restrictions.Eq(Projections.Property(() => emissao.Autorizado), true));
            disjunction.Add(Restrictions.And(Restrictions.And(Restrictions.Eq(Projections.Property(() => emissao.TipoEmissao), TipoEmissao.ContigenciaOfflineNFCe), Restrictions.Eq(Projections.Property(() => nfce.Status), Status.Aberta)), Restrictions.IsNull(Projections.Property(() => cancelamento.Nfce))));
            disjunction.Add(Restrictions.And(Restrictions.And(Restrictions.Eq(Projections.Property(() => emissao.TipoEmissao), TipoEmissao.ContigenciaOfflineNFCe), Restrictions.Eq(Projections.Property(() => nfce.Status), Status.Cancelada)), Restrictions.IsNotNull(Projections.Property(() => cancelamento.Nfce))));
            
            query.Where(disjunction);

            if (!string.IsNullOrWhiteSpace(inputFiltro))
            {
                var typeString = NHibernateUtil.String;
                query.Where(Restrictions.Or(Restrictions.Eq(Projections.Cast(typeString, Projections.Property(() => nfce.Id)), inputFiltro), Restrictions.Eq(Projections.Cast(typeString, Projections.Property(() => emissao.NumeroDocumento)), inputFiltro)));
            }

            if (dataEmissaoInicial != null && dataEmissaoFinal != null)
            {
                query.Where(Restrictions.Between(Projections.Cast(NHibernateUtil.Date, Projections.Property(() => nfce.EmitidaEm)), dataEmissaoInicial, dataEmissaoFinal));
            }

            if (dataEmissaoInicial != null && dataEmissaoFinal == null)
            {
                query.Where(Restrictions.Ge(Projections.Cast(NHibernateUtil.Date, Projections.Property(() => nfce.EmitidaEm)), dataEmissaoInicial));
            }

            if (dataEmissaoInicial == null && dataEmissaoFinal != null)
            {
                query.Where(Restrictions.Le(Projections.Cast(NHibernateUtil.Date, Projections.Property(() => nfce.EmitidaEm)), dataEmissaoFinal));
            }

            query.TransformUsing(Transformers.AliasToBean<NfceOpcoesVo>());

            query.Take(500);

            return query.List<NfceOpcoesVo>();
        }

        public IList<NfceOpcoesVo> BuscaParaOpcoesSat(string filtro = "", DateTime? dataEmissaoInicial = null, DateTime? dataEmissaoFinal = null)
        {
            Nfce nfceAlias = null;
            FinalizaEmissaoSat emissaoSat = null;
            CancelamentoSat cancelamentoSat = null;
            NfceOpcoesVo alias = null;

            var query = Sessao.QueryOver(() => nfceAlias)
                .SelectList(list => list
                    .Select(() => nfceAlias.Id).WithAlias(() => alias.Id)
                    .Select(() => emissaoSat.Chave).WithAlias(() => alias.Chave)
                    .Select(() => nfceAlias.TotalNfce).WithAlias(() => alias.Total)
                    .Select(() => emissaoSat.NumeroDocumento).WithAlias(() => alias.NumeroDocumento)
                    .Select(() => nfceAlias.EmitidaEm).WithAlias(() => alias.EmitidaEm)
                    .Select(() => nfceAlias.EmitidaEm).WithAlias(() => alias.AutorizadaEm)
                    .Select(() => nfceAlias.Status).WithAlias(() => alias.Status))
                .JoinAlias(() => nfceAlias.FinalizaEmissaoSat, () => emissaoSat, JoinType.InnerJoin)
                .JoinAlias(() => nfceAlias.CancelamentoSat, () => cancelamentoSat, JoinType.LeftOuterJoin)
                .OrderBy(() => nfceAlias.Id).Desc;


            var status = Restrictions.Disjunction();

            status.Add(Restrictions.Eq(Projections.Property(() => nfceAlias.Status), Status.Transmitida));
            status.Add(Restrictions.Eq(Projections.Property(() => nfceAlias.Status), Status.Cancelada));

            query.Where(status);

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var typeString = NHibernateUtil.String;

                query.Where(Restrictions.Or(
                    Restrictions.Eq(Projections.Cast(typeString, Projections.Property(() => nfceAlias.Id)), filtro),
                    Restrictions.Eq(Projections.Cast(typeString, Projections.Property(() => emissaoSat.NumeroCaixa)),
                        filtro)));
            }

            if (dataEmissaoInicial != null && dataEmissaoFinal != null)
            {
                query.Where(Restrictions.Between(Projections.Cast(NHibernateUtil.Date, Projections.Property(() => nfceAlias.EmitidaEm)), dataEmissaoInicial, dataEmissaoFinal));
            }

            if (dataEmissaoInicial != null && dataEmissaoFinal == null)
            {
                query.Where(Restrictions.Ge(Projections.Cast(NHibernateUtil.Date, Projections.Property(() => nfceAlias.EmitidaEm)), dataEmissaoInicial));
            }

            if (dataEmissaoInicial == null && dataEmissaoFinal != null)
            {
                query.Where(Restrictions.Le(Projections.Cast(NHibernateUtil.Date, Projections.Property(() => nfceAlias.EmitidaEm)), dataEmissaoFinal));
            }

            query.TransformUsing(Transformers.AliasToBean<NfceOpcoesVo>());

            query.Take(500);

            return query.List<NfceOpcoesVo>();
        }

        public void SalvarNfceEnvioLote(NfceEnvioLote envioLote)
        {
            Sessao.SaveOrUpdate(envioLote);
        }

        public void SalvarEmitente(NfceEmitente emitente)
        {
            Sessao.SaveOrUpdate(emitente);
        }

        public void SalvarCancelamento(NfceCancelamento cancelamento)
        {
            Sessao.SaveOrUpdate(cancelamento);
        }

        public void SalvarPagamento(FormaPagamentoNfce formaPagamentoNfce)
        {
            Sessao.SaveOrUpdate(formaPagamentoNfce);
        }

        public void SalvarEmissaoSemTransacao(NfceEmissao emissao)
        {
            Sessao.SaveOrUpdate(emissao);
        }

        public IList<Nfce> NfceEmitidaOffline()
        {
            Nfce nfce = null;

            var lista = Sessao.QueryOver(() => nfce)
                .Where(() => nfce.TipoEmissao == TipoEmissao.ContigenciaOfflineNFCe
                             && nfce.Status == Status.Aberta).List<Nfce>();

            return lista;
        }

        public void SalvarFormaPagamento(FormaPagamentoNfce pagamento)
        {
            Sessao.SaveOrUpdate(pagamento);
        }

        public void SalvarHistorico(NfceEmissaoHistorico emissaoHistorico)
        {
            Sessao.SaveOrUpdate(emissaoHistorico);
        }

        public XmlAutorizadoDto BuscarXmlAutorizado(int nfceId)
        {
            NfceEmissao emissaoAlias = null;
            NfceEmissaoHistorico emissaoHistoricoAlias = null;
            Nfce nfceAlias = null;
            XmlAutorizadoDto retornoAlias = null;

            var queryOver = Sessao.QueryOver(() => emissaoHistoricoAlias)
                .JoinAlias(() => emissaoHistoricoAlias.Nfce, () => nfceAlias)
                .JoinAlias(() => nfceAlias.Emissao, () => emissaoAlias)
                .SelectList(
                    list => list.Select(() => emissaoAlias.XmlAutorizado).WithAlias(() => retornoAlias.Xml))
                .Where(a => a.Nfce.Id == nfceId);

            queryOver.TransformUsing(Transformers.AliasToBean(typeof(XmlAutorizadoDto)));

            var xmlImpressaoDto = queryOver.List<XmlAutorizadoDto>().FirstOrDefault();

            return xmlImpressaoDto;
        }

        public XmlAutorizadoDto BuscarXmlSatAutorizado(int nfceId)
        {
            FinalizaEmissaoSat emissaoAlias = null;
            Nfce nfceAlias = null;
            XmlAutorizadoDto retornoAlias = null;

            var queryOver = Sessao.QueryOver(() => nfceAlias)
                .JoinAlias(() => nfceAlias.FinalizaEmissaoSat, () => emissaoAlias)
                .SelectList(
                    list => list.Select(() => emissaoAlias.XmlRetorno).WithAlias(() => retornoAlias.Xml))
                .Where(a => a.Id == nfceId);

            queryOver.TransformUsing(Transformers.AliasToBean(typeof(XmlAutorizadoDto)));

            var xmlImpressaoDto = queryOver.List<XmlAutorizadoDto>().FirstOrDefault();

            return xmlImpressaoDto;
        }

        public NfceHistoricoUltimaChave BuscarChaveDeHistoricoMaisAtual(Nfce nfce)
        {
            NfceEmissaoHistorico emissaoHistoricoAlias = null;
            NfceHistoricoUltimaChave retornoAlias = null;

            var queryOver = Sessao.QueryOver(() => emissaoHistoricoAlias)
                .SelectList(
                    list => list
                        .Select(h => h.Id).WithAlias(() => retornoAlias.Id)
                        .Select(() => emissaoHistoricoAlias.ChaveTexto.Valor).WithAlias(() => retornoAlias.Chave)
                        .Select(() => emissaoHistoricoAlias.FalhaReceberLote).WithAlias(() => retornoAlias.FalharReceberLote)
                        .Select(() => emissaoHistoricoAlias.XmlLote).WithAlias(() => retornoAlias.XmlLote))
                .Where(h => h.Nfce == nfce && h.Finalizou.Valor == false).OrderByAlias(() => emissaoHistoricoAlias.Id).Desc;

            queryOver.TransformUsing(Transformers.AliasToBean<NfceHistoricoUltimaChave>());

            var ultimaChave = queryOver.Take(1).List<NfceHistoricoUltimaChave>().FirstOrDefault();

            return ultimaChave;
        }

        public bool ExisteHistoricoEmAbertoNfce(Nfce nfce)
        {
            var qtdHistoricoEmAberto = ContaHistoricos(nfce);

            return qtdHistoricoEmAberto > 0;
        }

        public bool ExisteHistoricoEmAbertoSat(Nfce nfce)
        {
            var qtdHistoricoEmAberto = ContaHistoricosSat(nfce);

            return qtdHistoricoEmAberto > 0;
        }

        public HistoricoEnvioSat BuscarHistoricoMaisAtualSat(Nfce nfce)
        {
            HistoricoEnvioSat historicoEnvioSatAlias = null;

            var historico = Sessao.QueryOver(() => historicoEnvioSatAlias)
                .Where(h => h.Nfce == nfce && h.Finalizou == false).SingleOrDefault();

            return historico;
        }

        public NfceEmissaoHistorico BuscarHistoricoPorId(int id)
        {
            return Sessao.Load<NfceEmissaoHistorico>(id);
        }

        public string BuscarUltimoXmlAssinado(int nfceId)
        {
            NfceEmissaoHistorico tbHistorico = null;

            var query = Sessao.QueryOver(() => tbHistorico)
                .Select(i => i.XmlEnvio.Valor)
                .And(i => i.Nfce.Id == nfceId)
                .OrderBy(i => i.Id).Desc
                .Take(1);

            return query.FutureValue<string>().Value;
        }

        public int QuantidadeDeNFCeOffiline()
        {
            Nfce nfce = null;

            var quantidade = Sessao.QueryOver(() => nfce)
                .Where(() => nfce.TipoEmissao == TipoEmissao.ContigenciaOfflineNFCe
                             && nfce.Status == Status.Aberta)
                .RowCount();

            return quantidade;
        }

        public NfceEmissaoHistorico BuscarHistoricoPelaChaveDeAcesso(string chaveAcesso)
        {
            NfceEmissaoHistorico emissaoHistorico = null;

            var historico = Sessao.QueryOver(() => emissaoHistorico)
                .Where(() => emissaoHistorico.ChaveTexto.Valor == chaveAcesso).OrderByAlias(() => emissaoHistorico.Id)
                .Desc.List().FirstOrDefault();

            return historico;
        }

        public IEnumerable<NfceEmissaoHistorico> BuscarHistoricoEmissao(Nfce nfce)
        {
            NfceEmissaoHistorico emissaoHistorico = null;
            var historicos = Sessao.QueryOver(() => emissaoHistorico)
                .Where(() => emissaoHistorico.Nfce == nfce).List<NfceEmissaoHistorico>();

            return historicos;
        }

        public IEnumerable<HistoricoEnvioSat> BuscarHistoricoEnvioSats(Nfce nfce)
        {
            HistoricoEnvioSat emissaoHistorico = null;
            var historicos = Sessao.QueryOver(() => emissaoHistorico)
                .Where(() => emissaoHistorico.Nfce == nfce).List<HistoricoEnvioSat>();

            return historicos;
        }

        public void SalvarHistoricoSat(HistoricoEnvioSat historico)
        {
            Sessao.SaveOrUpdate(historico);
        }

        public void SalvarFinalizacaoSat(FinalizaEmissaoSat finalizacao)
        {
            Sessao.SaveOrUpdate(finalizacao);
        }

        public int ContaHistoricos(Nfce nfce)
        {
            NfceEmissaoHistorico emissaoHistorico = null;
            var quantidade = Sessao.QueryOver(() => emissaoHistorico)
                .Where(() => emissaoHistorico.Nfce == nfce
                && emissaoHistorico.Finalizou.Valor == false).RowCount();

            return quantidade;
        }

        public bool ExisteHistorico(int nfceId)
        {
            NfceEmissaoHistorico emissaoHistorico = null;
            var quantidade = Sessao.QueryOver(() => emissaoHistorico)
                .Where(() => emissaoHistorico.Nfce.Id == nfceId).RowCount();

            return quantidade != 0;
        }

        public int ContaHistoricosSat(Nfce nfce)
        {
            HistoricoEnvioSat emissaoHistorico = null;
            var quantidade = Sessao.QueryOver(() => emissaoHistorico)
                .Where(() => emissaoHistorico.Nfce == nfce
                    && emissaoHistorico.Finalizou == false).RowCount();

            return quantidade;
        }

        public NfceContingencia BuscarContingenciaAtiva()
        {
            NfceContingencia contingenciaAlias = null;

            var contingencia = Sessao.QueryOver(() => contingenciaAlias)
                .Where(() => contingenciaAlias.Ativa == true)
                .SingleOrDefault<NfceContingencia>();

            return contingencia;
        }

        public void SalvarContingencia(NfceContingencia contingencia)
        {
            Sessao.SaveOrUpdate(contingencia);
        }

        public void SalvarESincronizar(Nfce nfce)
        {
            nfce.Sincronizado = false;

            Salvar(nfce);
        }

        public IEnumerable<Nfce> BuscarTodasNfceSincronivaveis()
        {
            var queryOver = Sessao.QueryOver<Nfce>().Where(n => n.Sincronizado == false && n.Status != Status.Aberta);

            var lista = queryOver.List<Nfce>();

            return lista;
        }

        public void RetirarDaSessao(Nfce nfce)
        {
            Sessao.Evict(nfce);
        }

        public ClienteNfceDto BuscarDestinatarioPorDocumento(string documentoUnicoCliente)
        {
            NfceDestinatario destinatario = null;
            CidadeNfce cidade = null;
            ClienteNfceDto alias = null;
                
            var queryOver = Sessao.QueryOver(() => destinatario)
                .JoinAlias(() => destinatario.Cidade, () => cidade)
                .SelectList(list => list
                .Select(() => destinatario.Nome).WithAlias(() => alias.Nome)
                .Select(() => destinatario.DocumentoUnico).WithAlias(() => alias.DocumentoUnico)
                .Select(() => destinatario.Email).WithAlias(() => alias.Email)
                .Select(() => destinatario.Logradouro).WithAlias(() => alias.Logradouro)
                .Select(() => destinatario.Numero).WithAlias(() => alias.Numero)
                .Select(() => destinatario.Bairro).WithAlias(() => alias.Bairro)
                .Select(() => destinatario.Cep).WithAlias(() => alias.Cep)
                .Select(() => destinatario.Complemento).WithAlias(() => alias.Complemento)
                .Select(() => destinatario.InscricaoEstadual).WithAlias(() => alias.InscricaoEstadual)
                .Select(() => destinatario.Cidade).WithAlias(() => alias.Cidade)
                )
                .Where(c => c.DocumentoUnico == documentoUnicoCliente);

            queryOver.TransformUsing(Transformers.AliasToBean<ClienteNfceDto>());

            var cliente =  queryOver.OrderBy(() => destinatario.NfceId).Desc.Take(1).SingleOrDefault<ClienteNfceDto>();

            return cliente;
        }

        public int QuantidadeRespostaFiscalPentendete()
        {
            Nfce nfce = null;
            FormaPagamentoNfce formaPagamentoNfce = null;

            var queryOver = Sessao.QueryOver(() => formaPagamentoNfce)
                .JoinAlias(() => formaPagamentoNfce.Nfce, () => nfce);

            var conjunction = Restrictions.Conjunction();

            conjunction.Add(Restrictions.Eq(Projections.Property(() => nfce.Status), Status.Transmitida));

            conjunction.Add(Restrictions.Eq(Projections.Property(() => formaPagamentoNfce.IdFormaPagamento), FormaPagamentoNfce.CartaoPos));

            conjunction.Add(Restrictions.Eq(Projections.Property(() => formaPagamentoNfce.IsRespostaFiscalSucesso), false));

            conjunction.Add(Restrictions.Eq(Projections.Property(() => formaPagamentoNfce.IsMfe), true));


            queryOver.Where(conjunction);

            var qtdRespostaFiscalPentendete = queryOver.RowCount();

            return qtdRespostaFiscalPentendete;
        }

        public int BuscaQuantidadeElegiveisPendenteOffline(Nfce nfceExcluida, DateTime? data = null)
        {
            Nfce nfce = null;

            var query = Sessao.QueryOver(() => nfce);

            var conjunctionOfflineAberta = Restrictions.Conjunction();

            conjunctionOfflineAberta.Add(Restrictions.Eq(Projections.Property(() => nfce.TipoEmissao), TipoEmissao.ContigenciaOfflineNFCe));
            conjunctionOfflineAberta.Add(Restrictions.Eq(Projections.Property(() => nfce.Status), Status.PendenteOffline));

            if (nfceExcluida != null)
                conjunctionOfflineAberta.Add(Restrictions.Not(Restrictions.Eq(Projections.Property(() => nfce.Id), nfceExcluida.Id)));

            if (data != null)
            {
                conjunctionOfflineAberta.Add(Restrictions.Le(Projections.Property(() => nfce.CriadoEm), data));
            }
            
            query.Where(conjunctionOfflineAberta);

            return query.RowCount();
        }

        public IList<FormaPagamentoNfce> ObterTodasFormasPagamentoRespostaFiscalPendente()
        {
            Nfce nfce = null;
            FormaPagamentoNfce formaPagamentoNfce = null;

            var queryOver = Sessao.QueryOver(() => formaPagamentoNfce)
                .JoinAlias(() => formaPagamentoNfce.Nfce, () => nfce);

            var conjunction = Restrictions.Conjunction();


            conjunction.Add(Restrictions.Eq(Projections.Property(() => nfce.Status), Status.Transmitida));

            conjunction.Add(Restrictions.Eq(Projections.Property(() => formaPagamentoNfce.IdFormaPagamento), FormaPagamentoNfce.CartaoPos));

            conjunction.Add(Restrictions.Eq(Projections.Property(() => formaPagamentoNfce.IsRespostaFiscalSucesso), false));

            conjunction.Add(Restrictions.Eq(Projections.Property(() => formaPagamentoNfce.IsMfe), true));


            queryOver.Where(conjunction);

            var lista = queryOver.List<FormaPagamentoNfce>();

            return lista;
        }

        public FormaPagamentoNfce BuscarFormaPagamentoPorId(int formaPagamentoId)
        {
            var formaPagamento = Sessao.Get<FormaPagamentoNfce>(formaPagamentoId);

            return formaPagamento;
        }

        public void SalvarESincronizar(NfceInutilizacao inutilizacao)
        {
            inutilizacao.Sincronizado = false;
            Sessao.Save(inutilizacao);
        }

        public IList<NfceInutilizacao> BuscarTodasInutilizacoesPendentes()
        {
            return Sessao.QueryOver<NfceInutilizacao>().Where(x => x.Sincronizado == false).List<NfceInutilizacao>();
        }

        public void Salvar(NfceInutilizacao inut)
        {
            Sessao.SaveOrUpdate(inut);
        }

        public void Refresh(NfceInutilizacao inut)
        {
            Sessao.Refresh(inut);
        }

        public new void Refresh(Nfce nfce)
        {
            Sessao.Refresh(nfce);
        }

        public void Deletar(FormaPagamentoNfce formaPagamentoNfce)
        {
            Sessao.Delete(formaPagamentoNfce);
        }

        public void Merge(Nfce nfce)
        {
            Sessao.Merge(nfce);
        }

        public void Deletar(NfceDestinatario destinatario)
        {
            Sessao.Delete(destinatario);
        }

        public void Salvar(NfceCobranca cobranca)
        {
            Sessao.SaveOrUpdate(cobranca);
        }

        public void Salvar(NfceCobrancaDuplicata duplicata)
        {
            Sessao.SaveOrUpdate(duplicata);
        }

        public void Deletar(NfceCobrancaDuplicata duplicata)
        {
            Sessao.Delete(duplicata);
        }

        public void Deletar(NfceCobranca cobranca)
        {
            Sessao.Delete(cobranca);
        }

        public void Deletar(NfceItem item)
        {
            Sessao.Delete(item);
        }

        public string ObterXmlAutorizado(int nfceId)
        {
            Nfce nfceTabela = null;
            NfceEmissao emissaoTabela = null;

            var consulta = Sessao.QueryOver(() => nfceTabela)
                .JoinAlias(() => nfceTabela.Emissao, () => emissaoTabela)
                .Select(Projections.Property(() => emissaoTabela.XmlAutorizado));

            consulta.Where(() => nfceTabela.Id == nfceId);

            var xmlAutorizado = consulta.SingleOrDefault<string>();

            return xmlAutorizado;
        }
    }
}
 