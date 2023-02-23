using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Cupom.Nfce;
using FusionCore.Debug;
using FusionCore.ExportacaoPacote.Empacotadores;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.ConverterVendaParaNfe;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.SelecionarNfce;
using FusionCore.Vendas.Autorizadores.Nfce;
using FusionCore.Vendas.Faturamentos;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Util;
using SituacaoFiscal = FusionCore.Vendas.Autorizadores.Nfce.SituacaoFiscal;

namespace FusionCore.Vendas.Autorizadores.Infra
{
    public class RepositorioCupomFiscal : Repositorio<CupomFiscal, int>, IRepositorioExportacaoXml, IRepositorioConverterNfe
    {
        public RepositorioCupomFiscal(ISession sessao) : base(sessao)
        {
        }

        public void SalvarOuAlterar(CupomFiscal cupomFiscal)
        {
            Sessao.SaveOrUpdate(cupomFiscal);
        }

        public bool ExisteCupomParaEssaVenda(FaturamentoVenda venda)
        {
            var consulta = Sessao.QueryOver<CupomFiscal>()
                .Where(x => x.Venda == venda);

            return consulta.RowCount() != 0;
        }

        public CupomFiscal ObterApenasCupomFiscal(FaturamentoVenda venda)
        {
            CupomFiscal tabelaCupomFiscal = null;
            CupomFiscal cupomFiscal = null;

            var consulta = Sessao.QueryOver(() => tabelaCupomFiscal)
                .SelectList(lista => lista
                    .Select(() => tabelaCupomFiscal.Id).WithAlias(() => cupomFiscal.Id)
                    .Select(() => tabelaCupomFiscal.EmissorFiscalId).WithAlias(() => cupomFiscal.EmissorFiscalId)
                    .Select(() => tabelaCupomFiscal.NumeroFiscal).WithAlias(() => cupomFiscal.NumeroFiscal)
                    .Select(() => tabelaCupomFiscal.Serie).WithAlias(() => cupomFiscal.Serie)
                    .Select(() => tabelaCupomFiscal.CodigoNumerico).WithAlias(() => cupomFiscal.CodigoNumerico)
                    .Select(() => tabelaCupomFiscal.TipoEmissao).WithAlias(() => cupomFiscal.TipoEmissao)
                    .Select(() => tabelaCupomFiscal.AmbienteSefaz).WithAlias(() => cupomFiscal.AmbienteSefaz)
                    .Select(() => tabelaCupomFiscal.ContingenciaId).WithAlias(() => cupomFiscal.ContingenciaId)
                    .Select(() => tabelaCupomFiscal.EmitirEm).WithAlias(() => cupomFiscal.EmitirEm)
                ).TransformUsing(Transformers.AliasToBeanConstructor(typeof(CupomFiscal).GetConstructors()[1]))
                .Where(c => c.Venda == venda);


            return consulta.SingleOrDefault();
        }

        public CupomFiscal ObterCupomFiscal(FaturamentoVenda venda)
        {
            return Sessao.QueryOver<CupomFiscal>().Where(c => c.Venda == venda).SingleOrDefault();
        }

        public void SalvarOuAlterarHistorico(CupomFiscalHistorico cupomFiscalHistorico)
        {
            Sessao.SaveOrUpdate(cupomFiscalHistorico);
        }

        public byte ObterEmissorFiscalId(FaturamentoVenda venda)
        {
            CupomFiscal tabelaCupomFiscal = null;

            var consulta = Sessao.QueryOver(() => tabelaCupomFiscal)
                .Select(c => c.EmissorFiscalId);

            consulta.Where(c => c.Venda == venda);

            var emissorFiscalId = consulta.SingleOrDefault<byte>();

            return emissorFiscalId;
        }

        public CupomFiscalHistorico BuscarHistoricoEmAberto(FaturamentoVenda venda)
        {
            FaturamentoVenda vendaAlias = null;
            CupomFiscal cupomFiscalAlias = null;
            CupomFiscalHistorico cupomFiscalHistoricoAlias = null;

            var cupomFiscalHistorico = Sessao.QueryOver(() => cupomFiscalHistoricoAlias)
                .JoinAlias(() => cupomFiscalHistoricoAlias.CupomFiscal, () => cupomFiscalAlias, JoinType.InnerJoin)
                .JoinAlias(() => cupomFiscalAlias.Venda, () => vendaAlias, JoinType.InnerJoin)
                .Where(() => vendaAlias.Id == venda.Id)
                .Where(() => cupomFiscalHistoricoAlias.Finalizado == false).SingleOrDefault();

            return cupomFiscalHistorico;
        }

        public void SalvarFinalizacao(CupomFiscalFinalizado cupomFiscalFinalizado)
        {
            Sessao.Save(cupomFiscalFinalizado);
        }

        public bool AlocarNumeroFiscal(FaturamentoVenda venda)
        {
            CupomFiscal tabelaCupomFiscal = null;

            var quantidade = Sessao.QueryOver(() => tabelaCupomFiscal)
                .Where(c => c.Venda == venda)
                .Where(() => tabelaCupomFiscal.NumeroFiscal != 0)
                .RowCount();

            return quantidade == 0;
        }

        public bool ExisteHistoricoEmAberto(FaturamentoVenda venda)
        {
            FaturamentoVenda vendaAlias = null;
            CupomFiscal cupomFiscalAlias = null;
            CupomFiscalHistorico cupomFiscalHistoricoAlias = null;

            var quantidade = Sessao.QueryOver(() => cupomFiscalHistoricoAlias)
                .JoinAlias(() => cupomFiscalHistoricoAlias.CupomFiscal, () => cupomFiscalAlias, JoinType.InnerJoin)
                .JoinAlias(() => cupomFiscalAlias.Venda, () => vendaAlias, JoinType.InnerJoin)
                .Where(() => vendaAlias.Id == venda.Id)
                .Where(() => cupomFiscalHistoricoAlias.Finalizado == false).RowCount();

            if (quantidade > 1)
                throw new InvalidOperationException("Existe mais de um cupom fiscal histórico em aberto");

            return quantidade == 1;
        }

        public bool ExisteHistoricoEmAbertoComRejeicao(FaturamentoVenda venda)
        {
            FaturamentoVenda vendaAlias = null;
            CupomFiscal cupomFiscalAlias = null;
            CupomFiscalHistorico cupomFiscalHistoricoAlias = null;

            var rejeicao = Restrictions.Disjunction();
            rejeicao.Add(Restrictions.Eq(Projections.Property(() => cupomFiscalAlias.SituacaoFiscal), SituacaoFiscal.Rejeicao));
            rejeicao.Add(Restrictions.Eq(Projections.Property(() => cupomFiscalAlias.SituacaoFiscal), SituacaoFiscal.RejeicaoOffline));

            var quantidade = Sessao.QueryOver(() => cupomFiscalHistoricoAlias)
                .JoinAlias(() => cupomFiscalHistoricoAlias.CupomFiscal, () => cupomFiscalAlias, JoinType.InnerJoin)
                .JoinAlias(() => cupomFiscalAlias.Venda, () => vendaAlias, JoinType.InnerJoin)
                .Where(() => vendaAlias.Id == venda.Id)
                .Where(() => cupomFiscalHistoricoAlias.Finalizado == false)
                .Where(rejeicao).RowCount();

            if (quantidade > 1)
                throw new InvalidOperationException("Existe mais de um cupom fiscal histórico em aberto");

            return quantidade == 1;
        }

        public bool ExisteHistoricoEmAberto(int idVendaFaturamento)
        {
            FaturamentoVenda vendaAlias = null;
            CupomFiscal cupomFiscalAlias = null;
            CupomFiscalHistorico cupomFiscalHistoricoAlias = null;

            var quantidade = Sessao.QueryOver(() => cupomFiscalHistoricoAlias)
                .JoinAlias(() => cupomFiscalHistoricoAlias.CupomFiscal, () => cupomFiscalAlias, JoinType.InnerJoin)
                .JoinAlias(() => cupomFiscalAlias.Venda, () => vendaAlias, JoinType.InnerJoin)
                .Where(() => vendaAlias.Id == idVendaFaturamento)
                .Where(() => cupomFiscalHistoricoAlias.Finalizado == false).RowCount();

            if (quantidade > 1)
                throw new InvalidOperationException("Existe mais de um cupom fiscal histórico em aberto");

            return quantidade == 1;
        }

        public bool CupomAutorizado(FaturamentoVenda venda)
        {
            CupomFiscal tabelaCupomFiscal = null;

            var autorizadoOr = Restrictions.Disjunction();

            autorizadoOr.Add(Restrictions.Eq(Projections.Property(() => tabelaCupomFiscal.SituacaoFiscal),
                SituacaoFiscal.Autorizada));

            autorizadoOr.Add(Restrictions.Eq(Projections.Property(() => tabelaCupomFiscal.SituacaoFiscal),
                SituacaoFiscal.AutorizadaDenegada));

            autorizadoOr.Add(Restrictions.Eq(Projections.Property(() => tabelaCupomFiscal.SituacaoFiscal),
                SituacaoFiscal.AutorizadaSemInternet));

            var quantidade = Sessao.QueryOver(() => tabelaCupomFiscal)
                .Where(c => c.Venda == venda)
                .Where(autorizadoOr)
                .RowCount();

            return quantidade == 1;
        }

        public string BaixarObterXmlAutorizado(int cupomId)
        {
            CupomFiscal cupomFiscalTabela = null;
            CupomFiscalFinalizado cupomFiscalFinalizadoTabela = null;

            var consulta = Sessao.QueryOver(() => cupomFiscalTabela)
                .JoinAlias(() => cupomFiscalTabela.CupomFiscalFinalizado,
                    () => cupomFiscalFinalizadoTabela,
                    JoinType.InnerJoin)
                .Select(Projections.Property(() => cupomFiscalFinalizadoTabela.XmlAutorizado));

            consulta.Where(() => cupomFiscalTabela.Id == cupomId);

            var xmlAutorizado = consulta.SingleOrDefault<string>();

            return xmlAutorizado;
        }

        public string BaixarUltimoXmlAssinado(int cupomId)
        {
            CupomFiscalHistorico cupomFiscalHistoricoTabela = null;

            var consulta = Sessao.QueryOver(() => cupomFiscalHistoricoTabela)
                .Select(h => h.Envio)
                .And(h => h.CupomFiscal.Id == cupomId)
                .OrderBy(h => h.Id).Desc
                .Take(1);

            return consulta.FutureValue<string>().Value;
        }

        public IList<NfceDto> BuscaNfceParaConversao(FiltroConversorNfce filtroConversorNfce)
        {
            CupomFiscal cupomFiscalTabela = null;
            CupomFiscalFinalizado cupomFiscalFinalizadoTabela = null;
            FaturamentoVenda faturamentoVendaTabela = null;
            EmpresaDTO empresaTabela = null;
            Destinatario destinatarioTabela = null;
            Cliente clienteTabela = null;
            PessoaEntidade pessoaEntidadeTabela = null;

            NfceDto alias = null;


            var consulta = Sessao.QueryOver(() => cupomFiscalTabela)
                .JoinAlias(() => cupomFiscalTabela.CupomFiscalFinalizado, () => cupomFiscalFinalizadoTabela)
                .JoinAlias(() => cupomFiscalTabela.Venda, () => faturamentoVendaTabela)
                .JoinAlias(() => faturamentoVendaTabela.Empresa, () => empresaTabela)
                .JoinAlias(() => faturamentoVendaTabela.Destinatario, () => destinatarioTabela, JoinType.LeftOuterJoin)
                .JoinAlias(() => destinatarioTabela.Cliente, () => clienteTabela, JoinType.LeftOuterJoin)
                .JoinAlias(() => clienteTabela.Pessoa, () => pessoaEntidadeTabela, JoinType.LeftOuterJoin)
                .SelectList(list => 
                    list.Select(() => cupomFiscalTabela.Id).WithAlias(() => alias.Id)
                        .Select(() => cupomFiscalTabela.Serie).WithAlias(() => alias.Serie)
                        .Select(() => cupomFiscalTabela.NumeroFiscal).WithAlias(() => alias.NumeroFiscal)
                        .Select(() => faturamentoVendaTabela.Total).WithAlias(() => alias.TotalFiscal)
                        .Select(() => cupomFiscalTabela.SituacaoFiscal).WithAlias(() => alias.SituacaoFiscal)
                        .Select(() => empresaTabela.RazaoSocial).WithAlias(() => alias.RazaoSocialEmitente)
                        .Select(() => empresaTabela.Id).WithAlias(() => alias.IdEmitente)
                        .Select(() => empresaTabela.RegimeTributario).WithAlias(() => alias.RegimeTributario)
                        .Select(() => pessoaEntidadeTabela.Nome).WithAlias(() => alias.NomeCliente)
                        .Select(() => pessoaEntidadeTabela.Id).WithAlias(() => alias.IdCliente)
                );


            consulta.Where(Restrictions.Eq(Projections.Property(() => cupomFiscalTabela.SituacaoFiscal),
                SituacaoFiscal.Autorizada));
            consulta.Where(Restrictions.Eq(Projections.Property(() => cupomFiscalTabela.ImportadaParaNfe), filtroConversorNfce.FiltroNfceJaConvertidas));

            if (filtroConversorNfce.TemCliente())
                consulta.Where(Restrictions.Like(Projections.Property(() => pessoaEntidadeTabela.Nome),
                    filtroConversorNfce.FiltroNomeDoClienteContenha,
                    MatchMode.Anywhere));

            if (filtroConversorNfce.TemDataInicio() && filtroConversorNfce.NaoTemDataFinal())
                consulta.Where(Restrictions.Ge(Projections.Cast(NHibernateUtil.Date, Projections.Property(() => cupomFiscalFinalizadoTabela.AutorizadaEm)),
                    filtroConversorNfce.FiltroDataInicio.Value));

            if (filtroConversorNfce.TemDataFinal() && filtroConversorNfce.NaoTemDataInicio())
                consulta.Where(Restrictions.Le(Projections.Cast(NHibernateUtil.Date, Projections.Property(() => cupomFiscalFinalizadoTabela.AutorizadaEm)),
                    filtroConversorNfce.FiltroDataFinal.Value));

            if (filtroConversorNfce.TemDataInicio() && filtroConversorNfce.TemDataFinal())
                consulta.Where(
                    Restrictions.Between(Projections.Cast(NHibernateUtil.Date, Projections.Property(() => cupomFiscalFinalizadoTabela.AutorizadaEm))
                        , filtroConversorNfce.FiltroDataInicio.Value, filtroConversorNfce.FiltroDataFinal.Value));

            consulta.TransformUsing(Transformers.AliasToBean<NfceDto>());

            var resultadoEmLista = consulta.OrderByAlias(() => cupomFiscalTabela.Id).Desc.List<NfceDto>();

            resultadoEmLista.ForEach(item =>
            {
                item.ResolveStatus();
                item.VemDeFaturamento();
            });

            return resultadoEmLista;
        }

        public IList<CupomFiscalDto> PesquisarCupons(IFiltroCupomFiscalDto filtroCupomFiscal)
        {
            CupomFiscal cupomFiscalTabela = null;
            FaturamentoVenda faturamentoVendaTabela = null;
            Destinatario destinatarioTabela = null;
            Cliente clienteTabela = null;
            PessoaEntidade pessoaEntidadeTabela = null;
            EmpresaDTO empresaTabela = null;
            CupomFiscalFinalizado cupomFiscalFinalizadoTabela = null;
            CupomFiscalDto cupomFiscalDto = null;

            var consulta = Sessao.QueryOver(() => cupomFiscalTabela)
                .JoinAlias(() => cupomFiscalTabela.Venda, () => faturamentoVendaTabela, JoinType.InnerJoin)
                .JoinAlias(() => faturamentoVendaTabela.Empresa, () => empresaTabela, JoinType.InnerJoin)
                .JoinAlias(() => faturamentoVendaTabela.Destinatario, () => destinatarioTabela, JoinType.LeftOuterJoin)
                .JoinAlias(() => destinatarioTabela.Cliente, () => clienteTabela, JoinType.LeftOuterJoin)
                .JoinAlias(() => clienteTabela.Pessoa, () => pessoaEntidadeTabela, JoinType.LeftOuterJoin)
                .JoinAlias(() => cupomFiscalTabela.CupomFiscalFinalizado,
                    () => cupomFiscalFinalizadoTabela,
                    JoinType.LeftOuterJoin)
                .SelectList(listaNfces => listaNfces
                    .Select(() => cupomFiscalTabela.Id).WithAlias(() => cupomFiscalDto.Id)
                    .Select(() => cupomFiscalTabela.SituacaoFiscal).WithAlias(() => cupomFiscalDto.SituacaoFiscal)
                    .Select(() => cupomFiscalTabela.NumeroFiscal).WithAlias(() => cupomFiscalDto.NumeroFiscal)
                    .Select(() => faturamentoVendaTabela.Id).WithAlias(() => cupomFiscalDto.VendaId)
                    .Select(() => empresaTabela.RazaoSocial).WithAlias(() => cupomFiscalDto.NomeEmpresa)
                    .Select(() => cupomFiscalFinalizadoTabela.Chave).WithAlias(() => cupomFiscalDto.Chave)
                    .Select(() => cupomFiscalTabela.Serie).WithAlias(() => cupomFiscalDto.SerieFiscal)
                    .Select(() => cupomFiscalFinalizadoTabela.AutorizadaEm).WithAlias(() => cupomFiscalDto.EmitidaEm)
                    .Select(() => faturamentoVendaTabela.Total).WithAlias(() => cupomFiscalDto.Total)
                    .Select(() => pessoaEntidadeTabela.Nome).WithAlias(() => cupomFiscalDto.NomeCliente)
                    .Select(() => cupomFiscalTabela.CriadaEm).WithAlias(() => cupomFiscalDto.CriadoEm)
                );


            if (filtroCupomFiscal.EmitidasIgualOuApos.IsNotNull())
            {
                consulta.Where(Restrictions
                    .Ge(Projections.Cast(NHibernateUtil.Date, Projections.Property(() => cupomFiscalFinalizadoTabela.AutorizadaEm)),
                        filtroCupomFiscal.EmitidasIgualOuApos));
            }

            if (filtroCupomFiscal.CodigoIdIgualA.IsNotNull())
            {
                consulta.Where(() => cupomFiscalTabela.Id == filtroCupomFiscal.CodigoIdIgualA);
            }

            if (filtroCupomFiscal.NumeroIgual.IsNotNull())
            {
                consulta.Where(() => cupomFiscalTabela.NumeroFiscal == filtroCupomFiscal.NumeroIgual);
            }

            if (filtroCupomFiscal.NomeEmpresaContenha.IsNotNullOrEmpty())
            {
                consulta.Where(Restrictions.Like(Projections.Property(() => empresaTabela.RazaoSocial),
                    filtroCupomFiscal.NomeEmpresaContenha.TrimOrEmpty(),
                    MatchMode.Anywhere));
            }

            if (filtroCupomFiscal.NomeClienteContenha.IsNotNullOrEmpty())
            {
                consulta.Where(Restrictions.Like(Projections.Property(() => pessoaEntidadeTabela.Nome),
                    filtroCupomFiscal.NomeClienteContenha.TrimOrEmpty(), MatchMode.Anywhere));
            }

            if (filtroCupomFiscal.TipoEmissao.IsNotNull())
            {
                consulta.Where(Restrictions.Eq(Projections.Property(() => cupomFiscalTabela.TipoEmissao),
                    filtroCupomFiscal.TipoEmissao.Value.ToEmissaoFiscal()));
            }

            if (filtroCupomFiscal.SituacaoFiscal.IsNotNull())
            {
                consulta.Where(Restrictions.Eq(Projections.Property(() => cupomFiscalTabela.SituacaoFiscal),
                    filtroCupomFiscal.SituacaoFiscal.Value));
            }

            consulta.TransformUsing(Transformers.AliasToBean<CupomFiscalDto>());
            var cupons = consulta.OrderByAlias(() => cupomFiscalTabela.CriadaEm).Desc.List<CupomFiscalDto>();

            return cupons;
        }

        public IList<CupomFiscal> BuscarCuponsOffline()
        {
            CupomFiscal cupomFiscalTabela = null;

            return Sessao.QueryOver(() => cupomFiscalTabela)
                .Where(Restrictions.Eq(Projections.Property(() => cupomFiscalTabela.TipoEmissao),
                    TipoEmissao.ContigenciaOfflineNFCe))
                .Where(Restrictions.Eq(Projections.Property(() => cupomFiscalTabela.SituacaoFiscal),
                    SituacaoFiscal.AutorizadaSemInternet)).List<CupomFiscal>();
        }

        public CupomFiscalHistorico BuscarCupomFiscalHistoricoUnico(int cuponsFiscalId)
        {
            CupomFiscalHistorico cupomFiscalHistoricoTabela = null;
            CupomFiscal cupomFiscalTabela = null;

            return Sessao.QueryOver(() => cupomFiscalHistoricoTabela)
                .JoinAlias(() => cupomFiscalHistoricoTabela.CupomFiscal, () => cupomFiscalTabela, JoinType.InnerJoin)
                .Where(Restrictions.Eq(Projections.Property(() => cupomFiscalTabela.Id),
                    cuponsFiscalId)).SingleOrDefault();
        }

        public CupomFiscalHistorico BuscarCupomFiscalHistoricoAberto(int cuponsFiscalId)
        {
            CupomFiscalHistorico cupomFiscalHistoricoTabela = null;
            CupomFiscal cupomFiscalTabela = null;

            return Sessao.QueryOver(() => cupomFiscalHistoricoTabela)
                .JoinAlias(() => cupomFiscalHistoricoTabela.CupomFiscal, () => cupomFiscalTabela, JoinType.InnerJoin)
                .Where(Restrictions.Eq(Projections.Property(() => cupomFiscalTabela.Id),
                    cuponsFiscalId))
                .Where(Restrictions.Eq(Projections.Property(() => cupomFiscalHistoricoTabela.Finalizado),
                    false)).SingleOrDefault();
        }

        public CupomFiscalHistorico BuscarPelaChave(string chave)
        {
            CupomFiscalHistorico cupomFiscalHistoricoTabela = null;
            CupomFiscal cupomFiscalTabela = null;

            return Sessao.QueryOver(() => cupomFiscalHistoricoTabela)
                .JoinAlias(() => cupomFiscalHistoricoTabela.CupomFiscal, () => cupomFiscalTabela, JoinType.InnerJoin)
                .Where(Restrictions.Eq(Projections.Property(() => cupomFiscalHistoricoTabela.Chave),
                    chave)).SingleOrDefault();
        }

        public CupomFiscalHistorico BuscarPelaChaveNaoFinalizado(string chave)
        {
            CupomFiscalHistorico cupomFiscalHistoricoTabela = null;
            CupomFiscal cupomFiscalTabela = null;

            return Sessao.QueryOver(() => cupomFiscalHistoricoTabela)
                .JoinAlias(() => cupomFiscalHistoricoTabela.CupomFiscal, () => cupomFiscalTabela, JoinType.InnerJoin)
                .Where(Restrictions.Eq(Projections.Property(() => cupomFiscalHistoricoTabela.Chave), chave))
                .Where(Restrictions.Eq(Projections.Property(() => cupomFiscalHistoricoTabela.Finalizado),
                    false)).SingleOrDefault();
        }

        public IEnumerable<IEnvelope> BuscarXmlExportacao(DateTime inicio, DateTime fim, EmpresaDTO empresa)
        {
            CupomFiscal cupomFiscalTabela = null;
            CupomFiscalFinalizado cupomFiscalFinalizadoTabela = null;
            FaturamentoVenda vendaTabela = null;
            EmpresaDTO empresaTabela = null;

            XmlExportacao resposta = null;

            var consulta = Sessao.QueryOver(() => cupomFiscalTabela)
                .JoinAlias(() => cupomFiscalTabela.CupomFiscalFinalizado, () => cupomFiscalFinalizadoTabela, JoinType.InnerJoin)
                .JoinAlias(() => cupomFiscalTabela.Venda, () => vendaTabela, JoinType.InnerJoin)
                .JoinAlias(() => vendaTabela.Empresa, () => empresaTabela, JoinType.InnerJoin)
                .SelectList(lista => lista
                    .Select(() => cupomFiscalFinalizadoTabela.Chave).WithAlias(() => resposta.Chave)
                    .Select(() => cupomFiscalFinalizadoTabela.XmlAutorizado).WithAlias(() => resposta.Xml)
                    .Select(() => cupomFiscalTabela.SituacaoFiscal).WithAlias(() => resposta.CupomSituacaoFiscal)
                );

            var and = Restrictions.Conjunction();
            var autorizacaoOuCancelado = Restrictions.Disjunction();

            autorizacaoOuCancelado.Add(Restrictions.Eq(Projections.Property(() => cupomFiscalTabela.SituacaoFiscal),
                SituacaoFiscal.Autorizada));

            autorizacaoOuCancelado.Add(Restrictions.Eq(Projections.Property(() => cupomFiscalTabela.SituacaoFiscal),
                SituacaoFiscal.Cancelado));

            var projectionRecebidoEm = Projections.Cast(NHibernateUtil.Date, Projections.Property(() => cupomFiscalFinalizadoTabela.AutorizadaEm));

            var projectionEmpresa = Projections.Property(() => empresaTabela.Id);

            if (BuildMode.IsProducao)
            {
                and.Add(Restrictions.Eq(Projections.Property(() => cupomFiscalTabela.AmbienteSefaz), TipoAmbiente.Producao));
            }

            and.Add(autorizacaoOuCancelado);
            and.Add(Restrictions.Between(projectionRecebidoEm, inicio, fim));
            and.Add(Restrictions.Eq(projectionEmpresa, empresa.Id));

            consulta.Where(and);
            consulta.TransformUsing(Transformers.AliasToBean<XmlExportacao>());

            return consulta.List<XmlExportacao>();
        }

        public bool PossuiCupomEmitidoEmContingencia()
        {
            CupomFiscal cupomFiscalAlias = null;

            var consulta = Sessao.QueryOver(() => cupomFiscalAlias)
                .Where(
                    Restrictions.Or(
                            Restrictions.Eq(Projections.Property(() => cupomFiscalAlias.SituacaoFiscal), SituacaoFiscal.RejeicaoOffline),
                            Restrictions.Eq(Projections.Property(() => cupomFiscalAlias.SituacaoFiscal), SituacaoFiscal.AutorizadaSemInternet)
                        )
                );

            return consulta.RowCount() != 0;
        }

        public bool PossuiCupomNaoAutorizados()
        {
            CupomFiscal cupomFiscalAlias = null;

            var diferente = Restrictions.Conjunction();

            diferente.Add(Restrictions.Eq(Projections.Property(() => cupomFiscalAlias.SituacaoFiscal),
                SituacaoFiscal.Autorizada));

            diferente.Add(Restrictions.Eq(Projections.Property(() => cupomFiscalAlias.SituacaoFiscal),
                SituacaoFiscal.Cancelado));

            diferente.Add(Restrictions.Eq(Projections.Property(() => cupomFiscalAlias.SituacaoFiscal),
                SituacaoFiscal.AutorizadaDenegada));

            diferente.Add(Restrictions.Eq(Projections.Property(() => cupomFiscalAlias.SituacaoFiscal),
                SituacaoFiscal.RejeicaoOffline));

            diferente.Add(Restrictions.Eq(Projections.Property(() => cupomFiscalAlias.SituacaoFiscal),
                SituacaoFiscal.AutorizadaSemInternet));

            var consulta = Sessao.QueryOver(() => cupomFiscalAlias)
                .Where(diferente);

            return consulta.RowCount() != 0;
        }

        public string BuscarChaveFiscalNosHistoricos(int cupomId)
        {
            CupomFiscal cupomFiscalTabela = null;
            CupomFiscalHistorico cupomFiscalHistoricoTabela = null;

            var consulta = Sessao.QueryOver(() => cupomFiscalHistoricoTabela)
                .JoinAlias(() => cupomFiscalHistoricoTabela.CupomFiscal,
                    () => cupomFiscalTabela,
                    JoinType.InnerJoin)
                .Select(Projections.Property(() => cupomFiscalHistoricoTabela.Chave));

            consulta = consulta.Where(() => cupomFiscalTabela.Id == cupomId).OrderBy(() => cupomFiscalHistoricoTabela.Id).Desc;

            var chave = consulta.Future<string>().First();

            return chave;
        }

        public CupomFiscal GetPeloId(int id)
        {
            var cupom = base.GetPeloId(id);

            cupom.Venda.Produtos.ForEach(item =>
            {
                NHibernateUtil.Initialize(item.Produto.ProdutosAlias);
            });

            return cupom;
        }

        public void Merge(CupomFiscal cupom)
        {
            Sessao.Merge(cupom);
        }
    }
}