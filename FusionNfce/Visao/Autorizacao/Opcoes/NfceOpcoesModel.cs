using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Fusion.FastReport.Facades;
using Fusion.FastReport.Facades.Infra;
using FusionCore.AutorizacaoOperacao;
using FusionCore.AutorizacaoOperacao.Autorizacao;
using FusionCore.AutorizacaoOperacao.PayloadTypes;
using FusionCore.ControleCaixa.Facades;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionNfce.Autorizacao;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Fiscal.SatFiscal;
using FusionCore.FusionNfce.SatFiscal;
using FusionCore.FusionNfce.Servico;
using FusionCore.FusionNfce.Servicos;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Repositorio.Legacy.Flags;
using FusionLibrary.VisaoModel;
using FusionNfce.AutorizacaoSatFiscal;
using FusionNfce.AutorizacaoSatFiscal.Criadores;
using FusionNfce.AutorizacaoSatFiscal.Helper;
using FusionNfce.Impressao;
using FusionNfce.Visao.Cancelamento;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.SendMail;
using FusionWPF.SharedViews.AutorizarOperacao;
using OpenAC.Net.Sat;
using ComponenteEmail = FusionCore.FusionAdm.Componentes.Email;

namespace FusionNfce.Visao.Autorizacao.Opcoes
{
    public sealed class NfceOpcoesModel : ModelBase
    {
        private readonly DanfeNfceFacade _danfeFacade;
        private readonly Nfce _nfce;
        private bool _imprimindo;
        private readonly SessaoNfce _sessaoNfce;

        public NfceOpcoesModel(Nfce nfce)
        {
            _danfeFacade = new DanfeNfceFacade();
            _nfce = nfce;
        }

        public ICommand CommandEmail => GetSimpleCommand(EnviaEmailAction);

        public ICommand CommandFechar => GetSimpleCommand(FecharTelaAction);
        public ICommand CommandCancelar => GetSimpleCommand(CancelarAction);
        public ICommand CommandImprimir => GetSimpleCommand(ImprimirAction);

        public event EventHandler FecharTela;

        public void CancelarAction(object obj)
        {
            var payload = new NfceCancelada(_nfce.Id, _nfce.TotalNfce);
            var autorizarUsuario = new AutorizarUsuarioNfce(SessaoSistemaNfce.SessaoManager);
            var autorizarCancelamento = new AutorizarOperacaoView(SessaoSistemaNfce.SessaoManager,autorizarUsuario ,SessaoSistemaNfce.Usuario, _nfce.Id.ToString(), Permissao.CANCELAR_NFCE, payload, () =>
             {
                 CancelarNfce();
             });

            autorizarCancelamento.ExecutarAcao();
        }

        public void CancelarNfce()
        {
            try
            {
                ControleCaixaNfceFacade.ThrowExcetpionSeNaoExistirCaixaAberto(SessaoSistemaNfce.Usuario);
                SessaoSistemaNfce.Usuario.VerificaPermissao.IsTemPermissaoThrow(Permissao
                    .PDV_CANCELAR_VENDA_FINALIZADA);
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraInformacao(e.Message);
                return;
            }

            if (SessaoSistemaNfce.IsEmissorNFce())
            {
                if (CancelarNfceComContingenciaAtiva()) return;

                var nfce = ObterNfceParaSerCancelada();

                var cancelador = new CanceladorSefaz(SessaoSistemaNfce.GetDadosSefaz());
                var model = new CancelamentoNfceModel(nfce, cancelador);
                var dialog = new CancelamentoNfceView(model);

                dialog.ShowDialog();
            }

            if (SessaoSistemaNfce.IsEmissorSat())
            {
                if (!DialogBox.MostraConfirmacao("Realmente deseja cancelar o Cupom Fiscal Eletrônico?",
                    MessageBoxImage.Question))
                {
                    return;
                }

                try
                {
                    using (var financeiro = new ServicoControleFinanceiroNfce(SessaoSistemaNfce.Usuario))
                    {
                        if (_nfce.ExisteCobrancaQueGeraFinancerio())
                        {
                            financeiro.CancelarFinanceiroNfce(_nfce);
                        }

                        var acbrSat = CriaAcbrSat.Criar();
                        new AtivarSat(acbrSat).Ativar();

                        acbrSat.OnCancelarUltimaVenda += (sender, args) => args.Dados = TrataIE.TrataXmlIE(args.Dados);

                        var cancelamento = new CFeCanc(CFe.Load(_nfce.FinalizaEmissaoSat.XmlRetorno));
                        var retorno = acbrSat.CancelarUltimaVenda(cancelamento);

                        if (retorno.MensagemSEFAZ.IsNotNullOrEmpty() || retorno.RetornoLst.Count >= 5 &&
                            retorno.RetornoLst[5].IsNotNullOrEmpty())
                        {
                            var mensagemSefaz = retorno.MensagemSEFAZ.IsNotNullOrEmpty()
                                ? retorno.MensagemSEFAZ
                                : retorno.RetornoLst[5];

                            DialogBox.MostraInformacao("Mensagem Sefaz: " + mensagemSefaz);
                        }

                        if (retorno.CodigoDeRetorno != 7000)
                        {
                            throw new InvalidOperationException($"Retorno: {retorno.MensagemRetorno}");
                        }

                        financeiro.ComitarAlteracoes();
                        SalvarCancelamentoSat(_nfce, retorno, cancelamento);

                        if (retorno.CodigoDeRetorno == 7000)
                        {
                            DialogBox.MostraInformacao($"Retorno: {retorno.MensagemRetorno}");
                        }

                        DanfeSatHelper.ImprimirCancelamento(new XmlAutorizadoDto
                        {
                            Xml = _nfce.FinalizaEmissaoSat.XmlRetorno
                        },
                            new XmlCancelamentoDto
                            {
                                Xml = retorno.Cancelamento.GetXml(encoding: Encoding.UTF8)
                            },
                            SessaoSistemaNfce.Preferencia.NomeImpressora,
                            SessaoSistemaNfce.Preferencia.NomeFantasiaCustomizado);
                    }
                }
                catch (InvalidOperationException ex)
                {
                    DialogBox.MostraAviso(ex.Message);
                }
            }

            OnFecharTela();

        }

        private bool CancelarNfceComContingenciaAtiva()
        {

            if (SessaoSistemaNfce.EstaEmContingencia())
            {
                if (!DialogBox.MostraConfirmacao(
                    "Deseja realmente cancelar uma nfc-e feita em contingência mas não transmitida ainda?",
                    MessageBoxImage.Question)) return true;

                if (_nfce.Cancelada) return true;


                using (var financeiro = new ServicoControleFinanceiroNfce(SessaoSistemaNfce.Usuario))
                using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
                using (var transacao = sessao.BeginTransaction())
                {
                    var repositorio = new RepositorioNfce(sessao);

                    //Fluxo de cancelamento para nfc-e emitida em contigencia e ainda não enviada
                    financeiro.CancelarFinanceiroNfce(_nfce);
                    new GeraRegistroCaixa(_nfce, sessao, SessaoSistemaNfce.Usuario).EstornarCaixa();

                    var itensNaoCancelados = _nfce.ObterOsItens().Where(i => i.Cancelado == false);

                    foreach (var i in itensNaoCancelados)
                    {
                        var estoqueServico = EstoqueServicoNfce.Cria(sessao,
                            i.Produto,
                            OrigemEventoEstoque.CancelamentoNfce,
                            TipoEventoEstoque.Entrada,
                            i.Quantidade);

                        estoqueServico.Acrescentar();

                        sessao.Flush();
                        sessao.Clear();
                    }

                    _nfce.FoiCancelada();

                    repositorio.SalvarESincronizar(_nfce);

                    financeiro.ComitarAlteracoes();
                    transacao.Commit();
                }

                OnFecharTela();

                return true;
            }

            return false;
        }

        private void SalvarCancelamentoSat(Nfce nfce, CancelamentoSatResposta retorno, CFeCanc cancelamento)
        {
            var cancelamentoSat = new CancelamentoSat
            {
                Nfce = nfce,
                XmlRetorno = retorno.Cancelamento.GetXml(encoding: Encoding.Unicode),
                AmbienteSefaz = SessaoSistemaNfce.GetAmbienteSefaz(),
                EnviadoEm = DateTime.Now,
                XmlEnvio = cancelamento.GetXml(encoding: Encoding.Unicode),
                CodigoRetorno = retorno.CodigoDeRetorno
            };

            nfce.FoiCancelada();

            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioNfce(sessao);

                repositorio.SalvarCancelamentoSat(cancelamentoSat);
                repositorio.SalvarESincronizar(nfce);


                var itensNaoCancelados = nfce.ObterOsItens().Where(i => i.Cancelado == false);

                foreach (var i in itensNaoCancelados)
                {
                    var estoqueServico = EstoqueServicoNfce.Cria(
                        sessao,
                        i.Produto,
                        OrigemEventoEstoque.CancelamentoNfce,
                        TipoEventoEstoque.Entrada,
                        i.Quantidade
                    );

                    estoqueServico.Acrescentar();
                }

                //TODO: Cancelar financeiro no fluxo de cancelamento com SAT
                new GeraRegistroCaixa(_nfce, sessao, SessaoSistemaNfce.Usuario).EstornarCaixa();

                transacao.Commit();
            }
        }

        private void FecharTelaAction(object obj)
        {
            OnFecharTela();
        }

        public void EnviaEmailAction(object obj)
        {
            if (SessaoSistemaNfce.EstaEmContingencia())
            {
                DialogBox.MostraInformacao("Você deve sair da contingência para enviar email");
                return;
            }


            var behavior = new EnvioEmailBehavior();

            behavior.DespacharEmails += DespacharEmailsHandler;

            if (SessaoSistemaNfce.IsEmissorNFce())
                behavior.Assunto = "NOTA FISCAL CONSUMIDOR ELETRONICA";

            if (SessaoSistemaNfce.IsEmissorSat())
                behavior.Assunto = "CUPOM FISCAL ELETRONICO";

            behavior.CorpoMensagem = "Segue em anexo o DANFE e o XML";

            if (!string.IsNullOrWhiteSpace(_nfce.Destinatario?.Email))
            {
                var email = new ComponenteEmail(_nfce.Destinatario.Email);
                behavior.Emails = new ObservableCollection<ComponenteEmail>(new[] { email });
            }

            new EnvioEmailView(behavior).ShowDialog();
        }

        private void DespacharEmailsHandler(object sender, IEnumerable<ComponenteEmail> emails)
        {
            var behavior = sender as EnvioEmailBehavior;

            if (behavior == null)
            {
                return;
            }

            if (SessaoSistemaNfce.IsEmissorNFce())
            {
                using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
                {
                    var configuracaoEmail = new RepositorioConfiguracaoEmailNfce(sessao).BuscarUnicaConfiguracao();

                    var escreverMensagem = new EscreverMensagem(
                        behavior.Assunto,
                        behavior.CorpoMensagem,
                        SessaoSistemaNfce.Preferencia.NomeFantasiaCustomizado
                    );

                    _danfeFacade.EnviarEmail(_nfce, emails, escreverMensagem
                        , SessaoSistemaNfce.Preferencia.LayoutImpressao.DanfeNfce()
                        , new ServicoObterXml(new RepositorioDanfeNfce(sessao))
                        , configuracaoEmail);
                }
            }

            if (SessaoSistemaNfce.IsEmissorSat())
            {
                DanfeSatHelper.EnviaEmail(_nfce, emails, behavior.Assunto, behavior.CorpoMensagem, SessaoSistemaNfce.Preferencia.NomeFantasiaCustomizado);
            }
        }

        private void OnFecharTela()
        {
            FecharTela?.Invoke(this, EventArgs.Empty);
        }

        private Nfce ObterNfceParaSerCancelada()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            {
                var repositorioNfce = new RepositorioNfce(sessao);
                var nfce = repositorioNfce.GetPeloId(_nfce.Id);

                return nfce;
            }
        }

        public void ImprimirAction(object sender)
        {
            if (_imprimindo)
            {
                return;
            }

            _imprimindo = true;

            try
            {
                if (SessaoSistemaNfce.IsEmissorNFce())
                {
                    new ServicoImpressaoNfce(_nfce.Id, _danfeFacade).Imprimir();
                    return;
                }

                if (SessaoSistemaNfce.IsEmissorSat())
                {
                    var xmlDto = new XmlAutorizadoDto { Xml = _nfce.FinalizaEmissaoSat.XmlRetorno };

                    DanfeSatHelper.Imprimir(
                        xmlDto,
                        SessaoSistemaNfce.Preferencia.NomeImpressora,
                        SessaoSistemaNfce.Preferencia.NomeFantasiaCustomizado);
                }
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            finally
            {
                _imprimindo = false;
            }
        }
    }
}