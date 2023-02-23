using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DFe.Utils.Assinatura;
using FusionCore.FusionAdm.CteEletronico.Autorizador;
using FusionCore.FusionAdm.CteEletronico.Autorizador.ConfiguracoesAutorizacoes;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.Fiscal.Fabricas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronico.Emitir.Emissao
{
    public class EmissaoSefazCteViewModel : ViewModel
    {
        private readonly Cte _cte;
        private readonly ISessaoManager _sessaoGerenciada;
        private bool _emProcessamento;
        private bool _emissaoEmAndamento;
        private bool _emissaoOk;
        private bool _envioOk;
        private bool _retornoOk;
        private string _textoInformativo;
        private CteEmissaoHistorico _ultimaEmissao;
        private string _chaveEmAutorizacao;

        public event EventHandler<Cte> Autorizado;
        public event EventHandler<Cte> AtualizarNumeracao; 

        public EmissaoSefazCteViewModel(Cte cte, ISessaoManager sessaoGerenciada)
        {
            _cte = cte;
            _sessaoGerenciada = sessaoGerenciada;
            EmProcessamento = true;
        }

        public bool EmProcessamento
        {
            get => _emProcessamento;
            set
            {
                _emProcessamento = value;
                PropriedadeAlterada();
            }
        }

        public void IniciarEmissao()
        {
            EmProcessamento = true;
            EmissaoEmAndamento = true;
            EmissaoOk = false;
            EnvioOK = false;
            RetornoOk = false;
            TextoInformativo = "Estou iniciando a emissão";


            Task.Run(() =>
            {
                CteEmissaoHistorico emissaoHistorico = null;

                try
                {
                    if (_cte.NumeroFiscalEmissao == 0)
                    {
                        new AlocarNumeracaoCTe(_cte).AlocarNumeroFiscal();
                    }
                    OnAtualizarNumeracao();

                    emissaoHistorico = CriaOuRecuperaEmissao();

                    TextoInformativo = "Emissão ok. Vou autorizar a CT-e agora";
                    ChaveEmAutorizacao = emissaoHistorico.Chave;
                    EmissaoOk = true;

                    if (!emissaoHistorico.HouveTentativaAutorizacao())
                    {
                        var autorizador = new AutorizaNaSefazCte(CertificadoDigitalFactory.Cria(_cte.PerfilCte.EmissorFiscal, true));
                        autorizador.AutorizaNaSefaz(emissaoHistorico);
                    }

                    EnvioOK = true;
                    TextoInformativo = "Estou aguardando o retorno da SEFAZ";


                    var tempoConsultaRecibo = BuscarTempoConsultaRecibo();

                    Thread.Sleep(tempoConsultaRecibo);

                    FinalizarEmissao(emissaoHistorico, tempoConsultaRecibo);
                    RetornoOk = true;
                }
                catch (WebException e)
                {
                    CertificadoDigital.ClearCache();
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        DialogBox.MostraErro("Verifique sua Internet - Sem resposta da SEFAZ", e);
                    });
                }
                catch (Exception e)
                {
                    Application.Current.Dispatcher.Invoke(
                        () => { DialogBox.MostraErro("Não consegui emitir: " + e.Message, e); });
                }
                finally
                {
                    RespondeSolicitacaoUsuario(emissaoHistorico);
                }
            });
        }

        private static int BuscarTempoConsultaRecibo()
        {
            int tempoConsultaRecibo;
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                tempoConsultaRecibo = new RepositorioCteConfiguracaoAutorizacao(sessao)
                    .TempoConsultaReciboMilesegundos();
            }

            return tempoConsultaRecibo;
        }

        private void FinalizarEmissao(CteEmissaoHistorico emissaoHistorico, int tempoConsultaRecibo)
        {
            var finalizadorCte = new FinalizadorCte();

            for (int tentativa = 0; tentativa <= 3; tentativa++)
            {
                finalizadorCte.FinalizarEmsisao(_cte, emissaoHistorico);

                if (emissaoHistorico.Finalizada)
                {
                    break;
                }

                TextoInformativo = "Servidor da SEFAZ está sonolento, espere mais um pouco";
                Thread.Sleep(tempoConsultaRecibo);
            }
        }

        private void RespondeSolicitacaoUsuario(CteEmissaoHistorico emissaoHistorico)
        {
            if (emissaoHistorico == null)
            {
                Inicializar();
                return;
            }

            if (_cte.CteEmissao != null)
            {
                EmProcessamento = false;
                EmissaoEmAndamento = false;

                Application.Current.Dispatcher.Invoke(() =>
                {
                   OnAutorizado();
                   OnFechar();
                });

                return;
            }

            _ultimaEmissao = emissaoHistorico;

            TextoInformativo = _ultimaEmissao.GetTextoRejeicao();
            EmissaoEmAndamento = _ultimaEmissao.Finalizada == false;
            EmProcessamento = false;
        }

        public string TextoInformativo
        {
            get => _textoInformativo;
            set
            {
                _textoInformativo = value;
                PropriedadeAlterada();
            }
        }

        public bool RetornoOk
        {
            get => _retornoOk;
            set
            {
                _retornoOk = value;
                PropriedadeAlterada();
            }
        }

        public bool EnvioOK
        {
            get => _envioOk;
            set
            {
                _envioOk = value;
                PropriedadeAlterada();
            }
        }

        public bool EmissaoOk
        {
            get => _emissaoOk;
            set
            {
                _emissaoOk = value;
                PropriedadeAlterada();
            }
        }

        public bool EmissaoEmAndamento
        {
            get => _emissaoEmAndamento;
            set
            {
                _emissaoEmAndamento = value;
                PropriedadeAlterada();
            }
        }

        public string ChaveEmAutorizacao
        {
            get => _chaveEmAutorizacao;
            set
            {
                _chaveEmAutorizacao = value;
                PropriedadeAlterada();
            }
        }

        private CteEmissaoHistorico CriaOuRecuperaEmissao()
        {
            if (_ultimaEmissao?.Finalizada == false)
                return _ultimaEmissao;

            var emitirCte = new EmitirCte(_cte.EmissorFiscal);

            return emitirCte.Emite(_cte); 
        }

        public void Inicializar()
        {
            using (var sessao = _sessaoGerenciada.CriaSessao())
            {
                var repositorioCte = new RepositorioCte(sessao);

                _ultimaEmissao = repositorioCte.BuscaUltimaEmissaoHistorico(_cte);
            }

            TextoInformativo = "Nenhuma informação disponível";
            EmissaoEmAndamento = _ultimaEmissao != null && _ultimaEmissao.Finalizada == false;

            if (_ultimaEmissao != null && _ultimaEmissao.Finalizada == false)
            {
                TextoInformativo = "Existe uma emissão pendente. Clique em no botão para para conclui-la";
                ChaveEmAutorizacao = _ultimaEmissao.Chave;
            }

            EmProcessamento = false;
        }

        protected virtual void OnAutorizado()
        {
            Autorizado?.Invoke(this, _cte);
        }

        protected virtual void OnAtualizarNumeracao()
        {
            AtualizarNumeracao?.Invoke(this, _cte);
        }
    }
}