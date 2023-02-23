using System;
using System.Net;
using System.Threading.Tasks;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF.Autorizacao;
using FusionCore.FusionAdm.Fiscal.NF.Consulta;
using FusionCore.FusionNfce;
using FusionCore.FusionNfce.Autorizacao;
using FusionCore.FusionNfce.Extencoes;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Fiscal.Flags;
using FusionCore.FusionNfce.Fiscal.Servicos;
using FusionCore.FusionNfce.Servicos;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionNfce;
using FusionLibrary.VisaoModel;
using NFe.Classes.Protocolo;
using NFe.Classes.Servicos.Consulta;
using NHibernate;

namespace FusionNfce.Visao.Autorizacao.Emissao
{
    public class AutorizacaoModelResposta
    {
        public bool Sucesso { get; private set; }
        public string Mensagem { get; private set; }

        public AutorizacaoModelResposta(bool sucesso, string mensagem)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
        }
    }

    public sealed class AutorizacaoNfceModel : ModelBase
    {
        private readonly Nfce _nfce;
        private bool _emProcessamento;
        private TipoNotificacao _tipoNotificacao;
        private string _textoNotificacao;
        private ServicoEmissaoHistoricoNfce _servicoHistorico;

        public bool EmProcessamento
        {
            get => _emProcessamento;
            set
            {
                if (value == _emProcessamento) return;
                _emProcessamento = value;
                PropriedadeAlterada();
            }
        }

        public TipoNotificacao TipoNotificacao
        {
            get => _tipoNotificacao;
            set
            {
                if (value == _tipoNotificacao) return;
                _tipoNotificacao = value;
                PropriedadeAlterada();
            }
        }

        public string TextoNotificacao
        {
            get => _textoNotificacao;
            set
            {
                if (value == _textoNotificacao) return;
                _textoNotificacao = value;
                PropriedadeAlterada();
            }
        }

        public AutorizacaoNfceModel(Nfce nfce)
        {
            _nfce = nfce;
            _servicoHistorico = new ServicoEmissaoHistoricoNfce();
        }

        private void MostraNotificacao(string texto, TipoNotificacao tipo = TipoNotificacao.Informativo)
        {
            TextoNotificacao = texto;
            TipoNotificacao = tipo;
        }

        public async Task<AutorizacaoModelResposta> EmiteNotaFiscalAsync()
        {
            return await Task.Run(() => EmiteNotaFiscal());
        }

        private AutorizacaoModelResposta EmiteNotaFiscal()
        {
            // ReSharper disable once RedundantAssignment
            AutorizacaoModelResposta resposta = null;

            try
            {
                EmProcessamento = true;
                MostraNotificacao("Obtendo informações para emissão...");
                resposta = AutorizaNaSefaz();
            }
            catch (WebException ex)
            {
                MostraNotificacao("Ouve um erro de conexão com a sefaz ou internet\n" + ex.Message, TipoNotificacao.Erro);
                resposta = new AutorizacaoModelResposta(false, "Ouve um erro de conexão com a sefaz ou internet");
            }
            catch (Exception e)
            {
                MostraNotificacao(e.Message, TipoNotificacao.Erro);
                resposta = new AutorizacaoModelResposta(false, e.Message);
            }
            finally
            {
                EmProcessamento = false;
            }

            return resposta;
        }

        private AutorizacaoModelResposta AutorizaNaSefaz()
        {
            MostraNotificacao("Autorizando Nota Fiscal Eletrônica de Consumidor na SEFAZ...");

            new CalculaImpostosNfce(_nfce).Calcular();

            var certificado = CertificadoDigitalNfce.CriaNfceCertificate2(true);
            var historicoUltimaChave = BuscarChaveDeUltimaTentativa(_nfce);
            var resultadoConsulta = ConsultaNfce(historicoUltimaChave);

            if (resultadoConsulta != null && resultadoConsulta.IsAutorizado())
            {
                return NfceAutorizada(historicoUltimaChave, resultadoConsulta);
            }

            if (resultadoConsulta != null && !resultadoConsulta.NaoConstaSefaz())
            {
                return TrataRejeicaoNoHistorico(historicoUltimaChave, resultadoConsulta);
            }

            var emitirNfce = new EmitirNfce(certificado);
            var emissaoHistorico = emitirNfce.Emite(_nfce);

            if (emissaoHistorico.Chave.FormaEmissao == TipoEmissao.ContigenciaOfflineNFCe
                && _nfce.Status != Status.PendenteOffline)
            {
                try
                {
                    using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
                    using (var transacao = sessao.BeginTransaction())
                    using (var financeiro = new ServicoControleFinanceiroNfce(SessaoSistemaNfce.Usuario))
                    {
                        //FLuxo de emissão em contigência offline
                        var repositorioNfce = new RepositorioNfce(sessao);
                        _nfce.TipoEmissao = TipoEmissao.ContigenciaOfflineNFCe;
                        repositorioNfce.Salvar(_nfce);


                        financeiro.GerarFinanceiroParaNfce(_nfce);
                        GeraRegistroCaixa(sessao);

                        financeiro.ComitarAlteracoes();
                        transacao.Commit();
                    }

                    return ProcessaResposta(new RespostaAutorizacao(1000, "NFC-e Emitida em contingência offline"));
                }
                catch (FinanceiroServidorException e)
                {
                    _servicoHistorico.FinalizaHistoricoSemResultado(emissaoHistorico, e.Message);

                    throw;
                }
            }

            var autorizaNaSefazNfce = new AutorizaNaSefazNfce(certificado);
            var resposta = autorizaNaSefazNfce.AutorizaNaSefaz(emissaoHistorico);

            return ProcessaResposta(resposta);
        }

        private AutorizacaoModelResposta TrataRejeicaoNoHistorico(NfceHistoricoUltimaChave historicoUltimaChave,
            retConsSitNFe resultadoConsulta)
        {
            if (resultadoConsulta.cStat == 999 
                || (resultadoConsulta.protNFe?.infProt != null && resultadoConsulta.protNFe.infProt.cStat == 999) )
            {
                return new AutorizacaoModelResposta(false, resultadoConsulta.xMotivo);
            }

            var finalizarHistorico = BuscarHistoricoPorId(historicoUltimaChave.Id);

            var historicoNovo = _servicoHistorico.FinalizaHistorico(finalizarHistorico, resultadoConsulta);

            MostraNotificacao(historicoNovo.Motivo.Valor, TipoNotificacao.Erro);

            return new AutorizacaoModelResposta(false, historicoNovo.Motivo.Valor);
        }

        private AutorizacaoModelResposta NfceAutorizada(NfceHistoricoUltimaChave historicoUltimaChave,
            retConsSitNFe resultadoConsulta)
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            using (var financeiro = new ServicoControleFinanceiroNfce(SessaoSistemaNfce.Usuario))
            {
                var finalizarHistorico = BuscarHistoricoPorId(historicoUltimaChave.Id);

                financeiro.GerarFinanceiroParaNfce(_nfce);
                GeraRegistroCaixa(sessao);

                var configuracao = new ServicoProcessaFinalizacaoNFCe.Configuracao(
                    new RepositorioNfce(sessao),
                    finalizarHistorico,
                    _nfce,
                    resultadoConsulta);

                var servicoFinalizacao = new ServicoProcessaFinalizacaoNFCe(configuracao);

                servicoFinalizacao.Finaliza();
                financeiro.ComitarAlteracoes();

                transacao.Commit();

                return new AutorizacaoModelResposta(true, configuracao.NfceEmissaoHistorico.Motivo.Valor);
            }
        }

        private NfceEmissaoHistorico BuscarHistoricoPorId(int id)
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            {
                return new RepositorioNfce(sessao).BuscarHistoricoPorId(id);
            }
        }

        private retConsSitNFe ConsultaNfce(NfceHistoricoUltimaChave historicoUltimaChave)
        {
            if (historicoUltimaChave == null) return null;
            if (historicoUltimaChave.Finalizado) return null;

            var consultaZeus = new ConsultaNfce(SessaoSistemaNfce.GetDadosSefaz());

            var recibo = historicoUltimaChave.ObterRecibo();

            if (recibo.IsNotNullOrEmpty())
            {
                var retCosReciNFe = consultaZeus.ConsultaPeloRecibo(recibo);

                if (retCosReciNFe != null && retCosReciNFe.cStat == 106)
                {
                    return consultaZeus.ConsultaPelaChave(historicoUltimaChave.Chave);
                }

                if (retCosReciNFe == null || retCosReciNFe.cStat != 104)
                    throw new InvalidOperationException("Não foi possível obter resposta de autorização da SEFAZ");

                var infProt = retCosReciNFe.protNFe[0].infProt;

                return new retConsSitNFe
                {
                    cStat = infProt.cStat,
                    cUF = retCosReciNFe.cUF,
                    chNFe = infProt.chNFe,
                    protNFe = new protNFe
                    {
                        infProt = infProt,
                        versao = retCosReciNFe.versao
                    },
                    versao = retCosReciNFe.versao,
                    tpAmb = infProt.tpAmb,
                    verAplic = infProt.verAplic,
                    xMotivo = infProt.xMotivo
                };
            }

            
            var retConsSitNFe = consultaZeus.ConsultaPelaChave(historicoUltimaChave.Chave);
            
            return retConsSitNFe;
        }

        private NfceHistoricoUltimaChave BuscarChaveDeUltimaTentativa(Nfce nfce)
        {
            if (SessaoSistemaNfce.TipoEmissao != TipoEmissao.Normal) return null;

            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                var repositorio = new RepositorioNfce(sessao);
                var ultimaChave = repositorio.BuscarChaveDeHistoricoMaisAtual(nfce);

                return ultimaChave;
            }
        }

        private void GeraRegistroCaixa(ISession sessao)
        {
            new GeraRegistroCaixa(_nfce, sessao, SessaoSistemaNfce.Usuario).RegistrarCaixa();
        }

        private AutorizacaoModelResposta ProcessaResposta(RespostaAutorizacao resposta)
        {
            if (resposta.CodigoSolicitacao == 999)
            {
                MostraNotificacao(resposta.TextoSolicitacao, TipoNotificacao.Erro);
                return new AutorizacaoModelResposta(false, resposta.TextoSolicitacao);
            }

            if (resposta.CodigoSolicitacao == 1000)
            {
                MostraNotificacao(resposta.TextoSolicitacao);
                return new AutorizacaoModelResposta(true, resposta.TextoSolicitacao);
            }


            if (resposta.Autorizado)
            {
                MostraNotificacao(resposta.TextoAutorizacao, TipoNotificacao.Sucesso);
                return new AutorizacaoModelResposta(true, resposta.TextoAutorizacao);
            }

            MostraNotificacao(resposta.TextoAutorizacao, TipoNotificacao.Erro);
            return new AutorizacaoModelResposta(false, resposta.TextoAutorizacao);
        }
    }
}