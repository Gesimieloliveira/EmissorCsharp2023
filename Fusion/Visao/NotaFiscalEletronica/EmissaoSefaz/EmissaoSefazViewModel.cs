using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DFe.Utils.Assinatura;
using Fusion.Sessao;
using FusionCore.ControleCaixa.Facades;
using FusionCore.FusionAdm.ContingenciaSefaz;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Fiscal.NF.Autorizacao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.NotaFiscalEletronica.EmissaoSefaz
{
    public class EmissaoSefazViewModel : ViewModel
    {
        private readonly Nfeletronica _nfe;
        private readonly ISessaoManager _sessaoManager;
        private readonly UsuarioDTO _usuario = SessaoSistema.Instancia.UsuarioLogado;
        private EmissaoNfe _ultimaEmissao;
        private int _tentarReenviarAutorizacao = 0;

        public EmissaoSefazViewModel(Nfeletronica nfe, ISessaoManager sessaoManager)
        {
            _nfe = nfe;
            _sessaoManager = sessaoManager;
            EmProcessamento = true;
        }

        public bool EmProcessamento
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool EnvioOk
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool EmissaoOk
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool RetornoOk
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool EmissaoEmAndamento
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public string TextoInformativo
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string ChaveEmAutorizacao
        {
            get
            {
                var chave = GetValue<string>();
                return !string.IsNullOrWhiteSpace(chave) ? chave : "Ainda não gerei!";
            }
            set => SetValue(value);
        }

        public event EventHandler CloseRequest;
        public event EventHandler<EmissaoFinalizadaNfe> EmissaoAutorizada;
        public event EventHandler<EmissaoNfe> EmissaoPendente;

        public void Inicializar()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorioNfe = new RepositorioNfe(sessao);

                _ultimaEmissao = repositorioNfe.BuscaUltimaEmissaoNfe(_nfe);
            }

            TextoInformativo = "Nenhuma informação disponível";
            EmissaoEmAndamento = _ultimaEmissao != null && _ultimaEmissao.Finalizada == false;

            if (_ultimaEmissao != null && _ultimaEmissao.Finalizada == false)
            {
                TextoInformativo = "Existe uma emissão pendente. Clique em no botão para para conclui-la";
                ChaveEmAutorizacao = _ultimaEmissao.Chave.ToString();
            }

            EmProcessamento = false;
        }

        public void IniciarEmissao()
        {
            ControleCaixaGestorFacade.ThrowExcetpionSeNaoExistirCaixaAberto(_usuario);

            EmProcessamento = true;
            EmissaoEmAndamento = true;
            EmissaoOk = false;
            EnvioOk = false;
            RetornoOk = false;
            TextoInformativo = "Estou iniciando a emissão";

            Task.Run(() =>
            {
                EmissaoNfe emissao = null;

                try
                {
                    emissao = CriaOuRecuperaEmissao();

                    TextoInformativo = "Emissão ok. Vou autorizar a NF-e agora";
                    ChaveEmAutorizacao = emissao.Chave.ToString();
                    EmissaoOk = true;

                    if (!emissao.HouveTentativaAutorizacao())
                    {
                        var autorizador = new AutorizadorNfe(_sessaoManager);
                        autorizador.AutorizaNaSefaz(emissao);
                    }

                    EnvioOk = true;
                    TextoInformativo = "Estou aguardando o retorno da SEFAZ";
                    Thread.Sleep(5000);

                    FinalizarEmissao(emissao);
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
                catch (InvalidOperationException e)
                {
                    Application.Current.Dispatcher.Invoke(() => DialogBox.MostraAviso(e.Message));
                }
                catch (Exception e)
                {
                    Application.Current.Dispatcher.Invoke(() => DialogBox.MostraErro("Erro ao Emitir: " + e.Message, e));
                }
                finally
                {
                    RespondeSolicitacaoUsuario(emissao);
                }
            });
        }

        private void RespondeSolicitacaoUsuario(EmissaoNfe emissao = null)
        {
            if (emissao == null)
            {
                Inicializar();
                return;
            }

            if (_nfe.Finalizacao != null)
            {
                EmProcessamento = false;
                EmissaoEmAndamento = false;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    EmissaoAutorizada?.Invoke(this, _nfe.Finalizacao);
                    CloseRequest?.Invoke(this, EventArgs.Empty);
                });

                return;
            }

            _ultimaEmissao = emissao;

            const int naoConstaSefaz = 217;

            if (_ultimaEmissao.GetStatusRejeicao() == naoConstaSefaz && _tentarReenviarAutorizacao < 2 && _ultimaEmissao.Finalizada)
            {
                Thread.Sleep(5000);
                _tentarReenviarAutorizacao += 1;
                _ultimaEmissao = null;
                IniciarEmissao();
                return;
            }

            TextoInformativo = _ultimaEmissao.GetTextoRejeicao();
            EmissaoEmAndamento = _ultimaEmissao.Finalizada == false;
            EmProcessamento = false;
            _tentarReenviarAutorizacao = 0;
        }

        private EmissaoNfe CriaOuRecuperaEmissao()
        {
            if (_ultimaEmissao?.Finalizada == false)
                return _ultimaEmissao;

            var contingencia = GetContingenciaSeExistir(_nfe);
            var emissornfe = new EmissorNfe(_sessaoManager);

            if (contingencia?.EstaAberta == true)
            {
                emissornfe.AtivarContingencia(contingencia);
            }

            return emissornfe.Emitir(_nfe);
        }

        private ContingenciaNfe GetContingenciaSeExistir(Nfeletronica nfe)
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioContingenciaNfe(sessao);
                return repositorio.ContingenciaAberta(nfe.Emitente.CarregarDadosEmissor(_sessaoManager));
            }
        }

        private void FinalizarEmissao(EmissaoNfe emissao)
        {
            ControleCaixaGestorFacade.ThrowExcetpionSeNaoExistirCaixaAberto(_usuario);

            var finalizadorNfe = new FinalizadorNfe(_sessaoManager, SessaoSistema.Instancia.UsuarioLogado);

            for (var tentativa = 0; tentativa <= 3; tentativa++)
            {
                finalizadorNfe.FinalizarEmissao(_nfe, emissao);

                if (emissao.Finalizada)
                {
                    break;
                }

                TextoInformativo = "Servidor da SEFAZ está sonolento, espere mais um pouco";
                Thread.Sleep(5000);
            }
        }

        public void CloseHandler()
        {
            if (_ultimaEmissao?.Finalizada == false)
            {
                EmissaoPendente?.Invoke(this, _ultimaEmissao);
            }
        }
    }
}