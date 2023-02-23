using System;
using System.Threading;
using System.Xml;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.ChaveEletronica;
using FusionCore.FusionAdm.Fiscal.Fabricas;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF.Autorizacao;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionNfce.Fiscal.TyneTypes;
using FusionCore.Helpers.DocumentoXml;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Vendas.Autorizadores.Infra;
using FusionCore.Vendas.Autorizadores.Nfce;
using FusionCore.Vendas.Faturamentos;
using FusionCore.Vendas.Repositorio;
using ChaveSefazHelper = FusionCore.Helpers.ChaveFiscal.ChaveSefazHelper;
using SituacaoFiscal = FusionCore.Vendas.Autorizadores.Nfce.SituacaoFiscal;

namespace Fusion.Visao.Vendas
{
    public class VerificaHistoricoPendente
    {
        private readonly FaturamentoVenda _venda;
        private CupomFiscal _cupomFiscal;
        private CupomFiscalHistorico _historico;
        private EmissorFiscal _emissorFiscal;

        public VerificaHistoricoPendente(FaturamentoVenda venda)
        {
            _venda = venda;
            CarregarInformacoes();
        }

        public void Verificar()
        {
            ValidaChave(_historico.Chave);

            ConsultaPelaChave();

            if (_historico.IsRejeicao999())
            {
                throw new InvalidOperationException(_historico.GetTextoRejeicao());
            }

            SalvarCupomFiscalHistorico();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);

                try
                {
                    repositorioCupomFiscal.SalvarOuAlterarHistorico(_historico);
                    var cupomFiscal = _historico.CupomFiscal;


                    if (_historico.Autorizado())
                    {
                        cupomFiscal.Autorizada();
                        FinalizaCupomFiscal(cupomFiscal, _historico, repositorioCupomFiscal, new RepositorioFaturamento(sessao));
                    }

                    if (_historico.Denegado())
                    {
                        cupomFiscal.Denegada();
                        FinalizaCupomFiscal(cupomFiscal, _historico, repositorioCupomFiscal, new RepositorioFaturamento(sessao));
                    }

                    transacao.Commit();
                }
                catch
                {
                    transacao.Rollback();
                    sessao.Clear();

                    sessao.Load(_historico, _historico.Id);
                    sessao.Load(_venda, _venda.Id);
                    throw;
                }
            }

            VerificaSeTemRejeicao();
        }

        private void VerificaSeTemRejeicao()
        {
            try
            {
                _historico.ThrowInvalidOperationOutrasRejeicoes();
            }
            catch
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                using (var transacao = sessao.BeginTransaction())
                {
                    var repositorioFaturamento = new RepositorioFaturamento(sessao);
                    var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);

                    var cupomFiscal = _historico.CupomFiscal;

                    cupomFiscal.ComRejeicao();

                    repositorioCupomFiscal.SalvarOuAlterar(cupomFiscal);
                    repositorioFaturamento.Salvar(_venda);
                    transacao.Commit();
                }

                throw;
            }
        }

        private void FinalizaCupomFiscal(CupomFiscal cupomFiscal, CupomFiscalHistorico historico, RepositorioCupomFiscal repositorioCupomFiscal, RepositorioFaturamento repositorioFaturamento)
        {
            cupomFiscal.ComCupomFinalizado(new CupomFiscalFinalizado(
                cupomFiscal,
                historico.Chave,
                historico.ObterProtocolo(),
                historico.ObterDataReciboEm(),
                historico.MontarXmlProcessado()));

            repositorioCupomFiscal.SalvarFinalizacao(cupomFiscal.CupomFiscalFinalizado);
            repositorioCupomFiscal.SalvarOuAlterar(cupomFiscal);

            ConverteSituacaoFiscalParaVenda(cupomFiscal.SituacaoFiscal);
            repositorioFaturamento.Salvar(_venda);
        }

        private void ConverteSituacaoFiscalParaVenda(SituacaoFiscal situacaoFiscal)
        {
            switch (situacaoFiscal)
            {
                case SituacaoFiscal.Aberta:
                    _venda.SituacaoFiscalNaoEnviado();
                    break;
                case SituacaoFiscal.Autorizada:
                    _venda.SituacaoFiscalAutorizado();
                    break;
                case SituacaoFiscal.AutorizadaSemInternet:
                    _venda.SituacaoFiscalAutorizadoSemInternet();
                    break;
                case SituacaoFiscal.Cancelado:
                    _venda.SituacaoFiscalCancelado();
                    break;
                case SituacaoFiscal.AutorizadaDenegada:
                    _venda.SituacaoFiscalAutorizadoDenegada();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(situacaoFiscal), situacaoFiscal, null);
            }
        }

        private void CarregarInformacoes()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);
                _cupomFiscal = repositorioCupomFiscal.ObterCupomFiscal(_venda);
                _historico = repositorioCupomFiscal.BuscarCupomFiscalHistoricoAberto(_cupomFiscal.Id);
            }
        }

        private static void ValidaChave(string chave)
        {
            if (string.IsNullOrEmpty(chave))
                throw new InvalidOperationException(
                    "Porfavor é obrigatorio o uso de uma chave\nA chave deve ter 44 digitos, obrigado");

            if (chave.Length != 44)
                throw new InvalidOperationException("A chave deve ter 44 digitos, obrigado");
        }

        private void ConsultaPelaChave()
        {
            var configuracaoServico = CarregarConfiguracaoZeusFaturamento.CarregarConfiguracaoServicoZeus(_venda);

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var emissorFiscalId = new RepositorioCupomFiscal(sessao).ObterEmissorFiscalId(_venda);
                _emissorFiscal = new RepositorioEmissorFiscal(sessao).GetPeloId(emissorFiscalId);
            }

            var chave = ChaveSefazHelper.LoadChave(_historico.Chave);

            var certificado = CertificadoDigitalFactory.Cria(_emissorFiscal, true);
            var situacaoNotaSefaz = new SituacaoNotaSefaz(configuracaoServico, certificado);
            XmlNode respostaConsultaProtocolo;

            if (chave.FormaEmissao == TipoEmissao.ContigenciaOfflineNFCe)
            {
                var builder = new Chave.Builder()
                    .ComAnoMes(chave.AnoMes)
                    .ComCnpjEmitente(chave.Cnpj)
                    .ComCodigoIbgeUf(chave.CodigoIbgeUf)
                    .ComCodigoNumerico(chave.CodigoNumerico)
                    .ComFormaEmissao(TipoEmissao.Normal)
                    .ComModeloDocumento(ModeloDocumento.NFCe)
                    .ComNumeroFiscal(chave.NumeroFiscal)
                    .ComSerie(chave.Serie)
                    .ComDigitoVerificador(new DigitoVerificador(0));
                var chaveSefazOffline = FusionCore.FusionAdm.Fiscal.ChaveEletronica.ChaveSefazHelper.GerarChave(new ComponentesChaveNfce(builder));

                respostaConsultaProtocolo = situacaoNotaSefaz.GetSituacaoPelaChave(new ChaveSefaz(chaveSefazOffline.Chave));

                var xmlHelper = new XmlHelper(respostaConsultaProtocolo.OuterXml);
                var status = xmlHelper.GetValueFromElement("cStat", "infProt").GetValueOrDefault<int>();

                const int naoConstaNaBaseSefaz = 217;
                if (status != naoConstaNaBaseSefaz && status != 0)
                {
                    _historico.ProcessarRespostaPelaChave(respostaConsultaProtocolo);
                    _historico.AlteraChave(chaveSefazOffline.Chave);
                    return;
                }
            }

            Thread.Sleep(15000);

            respostaConsultaProtocolo = situacaoNotaSefaz.GetSituacaoPelaChave(new ChaveSefaz(_historico.Chave));
            _historico.ProcessarRespostaPelaChave(respostaConsultaProtocolo);
        }

        private void SalvarCupomFiscalHistorico()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);
                repositorioCupomFiscal.SalvarOuAlterarHistorico(_historico);
                transacao.Commit();
            }
        }
    }
}