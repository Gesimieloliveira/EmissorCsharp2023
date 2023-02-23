using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using FontAwesome.WPF;
using Fusion.FastReport.Facades;
using FusionCore.ControleCaixa.Facades;
using FusionCore.Excecoes;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF.ConstularStatus;
using FusionCore.FusionAdm.Fiscal.NF.EnviaLote;
using FusionCore.FusionAdm.Fiscal.NF.Exception;
using FusionCore.FusionAdm.Fiscal.NF.InutilizacaoNumero;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.FusionAdm.TabelasDePrecos.Dtos;
using FusionCore.FusionAdm.TabelasDePrecos.NfceSync;
using FusionCore.FusionNfce.Autorizacao;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Pagamento;
using FusionCore.FusionNfce.Preferencias;
using FusionCore.FusionNfce.Produto;
using FusionCore.FusionNfce.Servico;
using FusionCore.FusionNfce.Servico.Historicos;
using FusionCore.FusionNfce.Servicos;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.FusionNfce.Venda;
using FusionCore.FusionNfce.VendasPendentesMensais;
using FusionCore.FusionNfce.Vendedores;
using FusionCore.Helpers.Ambiente;
using FusionCore.Helpers.EmpresaDesenvolvedora;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Helpers.Sincronizador;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Repositorio.Legacy.Flags;
using FusionCore.Tributacoes.Flags;
using FusionLibrary.Helper.Conversores;
using FusionLibrary.Helper.Criptografia;
using FusionLibrary.Helper.Diversos;
using FusionLibrary.VisaoModel;
using FusionNfce.AutorizacaoSatFiscal;
using FusionNfce.AutorizacaoSatFiscal.Criadores;
using FusionNfce.AutorizacaoSatFiscal.Ext;
using FusionNfce.AutorizacaoSatFiscal.Helper;
using FusionNfce.Core;
using FusionNfce.Impressao;
using FusionNfce.Servicos;
using FusionNfce.Visao.Autorizacao.Emissao;
using FusionNfce.Visao.Autorizacao.SatFiscal;
using FusionNfce.Visao.Avisos;
using FusionNfce.Visao.BarraDeProgresso;
using FusionNfce.Visao.ConsultaProduto;
using FusionNfce.Visao.Principal.Contratos;
using FusionNfce.Visao.Principal.FinalizarVenda.FormasPagamento;
using FusionNfce.Visao.Principal.Implementacoes.Caixa;
using FusionNfce.Visao.Principal.Model;
using FusionWPF.Base.Utils.Dialogs;
using NFe.Classes.Servicos.Status;
using NHibernate;
using NHibernate.Util;
using OpenAC.Net.Sat;
using Tef.Dominio;
using AutorizacaoModelResposta = FusionNfce.Visao.Autorizacao.Emissao.AutorizacaoModelResposta;
using Brushes = System.Windows.Media.Brushes;

namespace FusionNfce.Visao.Principal
{
    public enum StatusCaixa
    {
        CancelamentoItem,
        AlteracaoDeQuantidade,
        Normal,
        AlterarItem
    }

    public static class StatusCaixaExt
    {
        public static IComandoCaixa GetComando(this StatusCaixa statusCaixa)
        {
            switch (statusCaixa)
            {
                case StatusCaixa.CancelamentoItem:
                    return new ComandoCaixaCancelarItem();
                case StatusCaixa.AlteracaoDeQuantidade:
                    return new ComandoCaixaAdicionaQuantidade();
                case StatusCaixa.Normal:
                    return new ComandoCaixaNormal();
                case StatusCaixa.AlterarItem:
                    return new ComandoCaixaAlterarItem();
            }

            return null;
        }
    }

    public sealed class VendaModel : ViewModel
    {
        public event EventHandler AtualizarListaItens;
        private readonly RestricaoClienteObrigatorio _restricaoClienteObrigatorio;
        private readonly DanfeNfceFacade _danfeFacade = new DanfeNfceFacade();
        private DispatcherTimer _timerVenderProduto;
        private DispatcherTimer _timerVerificaSeTemErros;
        private DispatcherTimer _timerMensagemContingencia;
        private ObservableCollection<NfceItem> _itens;
        private readonly IList<ItemEspera> _produtosEmEspera;
        private string _codigoBarras;
        private decimal _total;
        private decimal _quantidade = 1;
        private bool _habilitarCodigoBarras = true;
        private string _informacaoCaixa = "Caixa Livre" + Homologacao();
        private string _informacaoAcaoBarras = "Venda";
        private bool _ativaAvisos;
        private AvisosFormModel _modelAvisos;
        private BitmapImage _logo;
        private DispatcherTimer _timerStatusServidor;
        private FontAwesomeIcon _statusServidor;
        private SolidColorBrush _corStatus;
        private bool _estaSincronizandoNfceOffline;
        private StatusVenda _statusVenda;
        private bool _isNfce;
        private bool _isSat;
        private string _loginUsuario;
        private bool _isContingencia;
        private string _mensagemContingencia;
        private bool _isBotaoResolverProblema;
        private ICommand _commandResolverProblema;
        private bool _isImpressaoDiretaAtiva;
        private bool _isVendasPendente;
        private bool _isVendasPendenteOffline;
        private ITef _tef;
        private bool _isTefAtivo;
        private string _versaoSistema;
        private string _nomeUsuarioLogado;
        private bool _isNfceSemMFe;
        private bool _isAdicionarVendedor;
        private string _nomeVendedor;
        private NfceItem _nfceItemSelecionado;
        private VendedorNfce _vendedor;
        private TabelaPrecoDto _tabelaPrecoSelecionada;
        private int? _idCodigoVenda;
        private Nfce _nfce;
        private DateTime? _dataCriacaoVenda;

        public Nfce Nfce
        {
            get => _nfce;
            set
            {
                _nfce = value;
                IdCodigoVenda = _nfce?.Id ?? null;
                DataCriacaoVenda = _nfce?.CriadoEm ?? null;
            } 
        }

        public DateTime? DataCriacaoVenda
        {
            get => _dataCriacaoVenda;
            set
            {
                _dataCriacaoVenda = value;
                PropriedadeAlterada();
            }
        }

        public event EventHandler FinalizacaoRapidaHandler;

        public string LabelCst { get; set; } = SessaoSistemaNfce.Empresa().RegimeTributario == RegimeTributario.SimplesNacional
            ? "CSOSN"
            : "CST";

        public StatusVenda StatusVenda
        {
            get => _statusVenda;
            set
            {
                _statusVenda = value;
                SessaoSistemaNfce.StatusVenda = value;
            }
        }

        public StatusCaixa StatusCaixa { get; set; }
        public ProdutoNfce ProdutoManual { get; set; }

        public bool AtivaAvisos
        {
            get => _ativaAvisos;
            set
            {
                if (value == _ativaAvisos) return;
                _ativaAvisos = value;
                PropriedadeAlterada();
            }
        }

        public string InformacaoAcaoBarras
        {
            get => _informacaoAcaoBarras;
            set
            {
                if (value == _informacaoAcaoBarras) return;
                _informacaoAcaoBarras = value;
                PropriedadeAlterada();
            }
        }

        public string InformacaoCaixa
        {
            get => _informacaoCaixa;
            set
            {
                if (value == _informacaoCaixa) return;
                _informacaoCaixa = value;
                PropriedadeAlterada();
            }
        }

        public int? IdCodigoVenda
        {
            get => _idCodigoVenda;
            set
            {
                _idCodigoVenda = value;
                PropriedadeAlterada();
            }
        }

        public bool HabilitarCodigoBarras
        {
            get => _habilitarCodigoBarras;
            set
            {
                if (value == _habilitarCodigoBarras) return;
                _habilitarCodigoBarras = value;
                PropriedadeAlterada();
            }
        }

        public decimal Quantidade
        {
            get => _quantidade;
            set
            {
                if (value == _quantidade) return;
                _quantidade = value;
                PropriedadeAlterada();
            }
        }

        public decimal Total
        {
            get => _total;
            set
            {
                if (value == _total) return;
                _total = value;
                PropriedadeAlterada();
            }
        }

        public bool IsContemHistoricoNaoFinalizado => ObterHistoricoNaoFinalizado();

        private bool ObterHistoricoNaoFinalizado()
        {
            return ExisteHistoricoAbertoServico.Existe(Nfce);
        }

        public string CodigoBarras
        {
            get => _codigoBarras;
            set
            {
                if (value == _codigoBarras) return;
                _codigoBarras = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<NfceItem> Itens
        {
            get => _itens;
            set
            {
                if (Equals(value, _itens)) return;
                _itens = value;
                PropriedadeAlterada();
            }
        }

        public NfceItem NfceItemSelecionado
        {
            get => _nfceItemSelecionado;
            set
            {
                if (_nfceItemSelecionado == null) return;
                _nfceItemSelecionado = value;
                PropriedadeAlterada();
            }
        }

        private bool TemErroFinanceiro { get; set; }
        private bool TemErroNfce { get; set; }

        public BitmapImage Logo
        {
            get => _logo;
            set
            {
                if (Equals(value, _logo)) return;
                _logo = value;
                PropriedadeAlterada();
            }
        }

        public SolidColorBrush CorStatus
        {
            get => _corStatus;
            set
            {
                if (Equals(value, _corStatus)) return;
                _corStatus = value;
                PropriedadeAlterada();
            }
        }

        public FontAwesomeIcon StatusServidor
        {
            get => _statusServidor;
            set
            {
                if (value == _statusServidor) return;
                _statusServidor = value;
                PropriedadeAlterada();
            }
        }

        public VendaModel()
        {
            Itens = new ObservableCollection<NfceItem>();
            _produtosEmEspera = new List<ItemEspera>();
            _restricaoClienteObrigatorio = new RestricaoClienteObrigatorio();

            VerificaStatusServidor();
            IniciarTimerVender();
            IniciarTimerVerificaSeTemErros();
            IniciarTimerMensagemContingencia();
            StatusVenda = StatusVenda.Livre;
            StatusCaixa = StatusCaixa.Normal;
            CarregarLogo();
            IniciaTimerStatusServico();
            IsNfce = SessaoSistemaNfce.IsEmissorNFce();
            IsSat = SessaoSistemaNfce.IsEmissorSat();
            LoginUsuario = SessaoSistemaNfce.Usuario.Login;
            IsContingencia = SessaoSistemaNfce.EstaEmContingencia();
            IsImpressaoDiretaAtiva = SessaoSistemaNfce.ImpressaoDireta.Ativa;
            AtualizarStatusVendaPendente();

            InicializaTef();

            VersaoSistema = ResponsavelLegal.VersaoSistema;
            NomeUsuarioLogado = SessaoSistemaNfce.Usuario.Login;
            NomeEmpresaEmissora = SessaoSistemaNfce.Configuracao.EmissorFiscal.Empresa.RazaoSocial;
            NomeTerminal = SessaoSistemaNfce.Configuracao.TerminalOfflineId.ToString();
        }

        private void IniciarTimerMensagemContingencia()
        {
            _timerMensagemContingencia = new DispatcherTimer(DispatcherPriority.Background, Application.Current.Dispatcher);
            _timerMensagemContingencia.Dispatcher.Thread.IsBackground = true;
            _timerMensagemContingencia.Tick += TimerMensagemContingencia_Tick;
            _timerMensagemContingencia.Interval = new TimeSpan(0, 0, 0, 0, 100);
            _timerMensagemContingencia.Start();
        }

        private void TimerMensagemContingencia_Tick(object sender, EventArgs e)
        {
            var timer = sender as DispatcherTimer;
            timer?.Stop();

            if (!SessaoSistemaNfce.EstaEmContingencia())
            {
                timer?.Start();
                return;
            }

            var quantidadeHora = (DateTime.Now - SessaoSistemaNfce.Contingencia.EntrouEm).Hours;

            if (quantidadeHora == 0 || quantidadeHora == 1)
            {
                MensagemContingencia = "Emitindo em Contingência a mais de " + quantidadeHora + " hora";
                timer?.Start();
                return;
            }

            MensagemContingencia = "Emitindo em Contingência a mais de " + quantidadeHora + " horas";
            timer?.Start();
        }

        public bool IsSat
        {
            get => _isSat;
            set
            {
                if (value == _isSat) return;
                _isSat = value;
                PropriedadeAlterada();
            }
        }

        public bool IsNfce
        {
            get => _isNfce;
            set
            {
                if (value == _isNfce) return;
                _isNfce = value;
                PropriedadeAlterada();
            }
        }

        public string LoginUsuario
        {
            get => _loginUsuario;
            set
            {
                if (value == _loginUsuario) return;
                _loginUsuario = value;
                PropriedadeAlterada();
            }
        }

        public bool IsContingencia
        {
            get => _isContingencia;
            set
            {
                if (value == _isContingencia) return;
                _isContingencia = value;
                PropriedadeAlterada();
            }
        }

        public string MensagemContingencia
        {
            get => _mensagemContingencia;
            set
            {
                if (value == _mensagemContingencia) return;
                _mensagemContingencia = value;
                PropriedadeAlterada();
            }
        }

        public bool IsBotaoResolverProblema
        {
            get => _isBotaoResolverProblema;
            set
            {
                _isBotaoResolverProblema = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandResolverProblema
        {
            get => _commandResolverProblema;
            set
            {
                if (Equals(value, _commandResolverProblema)) return;
                _commandResolverProblema = value;
                PropriedadeAlterada();
            }
        }

        public bool IsImpressaoDiretaAtiva
        {
            get => _isImpressaoDiretaAtiva;
            set
            {
                _isImpressaoDiretaAtiva = value;
                PropriedadeAlterada();
            }
        }

        public bool IsVendasPendente
        {
            get { return _isVendasPendente; }
            set
            {
                if (value == _isVendasPendente) return;
                _isVendasPendente = value;
                PropriedadeAlterada();
            }
        }

        public bool IsVendasPendenteOffline
        {
            get { return _isVendasPendenteOffline; }
            set
            {
                if (value == _isVendasPendenteOffline) return;
                _isVendasPendenteOffline = value;
                PropriedadeAlterada();
            }
        }

        public bool IsTefAtivo
        {
            get => _isTefAtivo;
            set
            {

                _isTefAtivo = value && IsNfce;
                PropriedadeAlterada();
            }
        }

        public string VersaoSistema
        {
            get => _versaoSistema;
            set
            {
                if (value == _versaoSistema) return;
                _versaoSistema = value;
                PropriedadeAlterada();
            }
        }

        public string NomeUsuarioLogado
        {
            get => _nomeUsuarioLogado;
            set
            {
                if (value == _nomeUsuarioLogado) return;
                _nomeUsuarioLogado = value;
                PropriedadeAlterada();
            }
        }

        public string NomeTerminal
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string NomeEmpresaEmissora
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public UltimaBuscaEfetuadaDoDia ObterUltimaBuscaEfetuada()
        {
            return UltimaBuscaEfetuadaDoDia;
        }

        public UltimaBuscaEfetuadaDoDia UltimaBuscaEfetuadaDoDia { get; private set; }

        public bool IsAdicionarVendedor
        {
            get => _isAdicionarVendedor;
            set
            {
                _isAdicionarVendedor = value;
                PropriedadeAlterada();
            }
        }

        public void SalvarUltimaBuscaEfetuada(UltimaBuscaEfetuadaDoDia ultimaBuscaEfetuadaDoDia)
        {
            UltimaBuscaEfetuadaDoDia = ultimaBuscaEfetuadaDoDia;
        }

        public event EventHandler SelecionaConteudoTextBoxKg;

        private void IniciarTimerVender()
        {
            _timerVenderProduto = new DispatcherTimer(DispatcherPriority.Background, Application.Current.Dispatcher);
            _timerVenderProduto.Dispatcher.Thread.IsBackground = true;
            _timerVenderProduto.Tick += TimerVenderProduto_Tick;
            _timerVenderProduto.Interval = new TimeSpan(0, 0, 0, 0, 100);
            _timerVenderProduto.Start();
        }

        private void IniciarTimerVerificaSeTemErros()
        {
            _timerVerificaSeTemErros = new DispatcherTimer(DispatcherPriority.Background, Application.Current.Dispatcher);
            _timerVerificaSeTemErros.Dispatcher.Thread.IsBackground = true;
            _timerVerificaSeTemErros.Tick += TimerVerificaSeTemErros_Tick;
#if DEBUG
            _timerVerificaSeTemErros.Interval = new TimeSpan(0, 0, 1, 0, 0);
#else
            _timerVerificaSeTemErros.Interval = new TimeSpan(0, 0, 5, 0, 0);
#endif
            _timerVerificaSeTemErros.Start();
        }

        private async void TimerVerificaSeTemErros_Tick(object sender, EventArgs e)
        {
            _modelAvisos = new AvisosFormModel();

            var timer = sender as DispatcherTimer;
            timer?.Stop();

            if (SessaoSistemaNfce.TipoEmissao != TipoEmissao.Normal)
            {
                _modelAvisos.AddAvisos(new List<Aviso>
                {
                    new Aviso
                    {
                        Action = null,
                        Mensagem = "Você está em modo contingência.",
                        Icone = FontAwesomeIcon.Warning
                    }
                });
                AtivaAvisos = true;
                timer?.Start();
                return;
            }

            if (_estaSincronizandoNfceOffline) return;

            await Task.Run(() => TentaEnviarNfceOffline(timer));

            AtivaAvisoErros();
        }

        private async Task TentaEnviarNfceOffline(DispatcherTimer timer)
        {
            try
            {
                if (SessaoSistemaNfce.TipoEmissao != TipoEmissao.Normal) return;

                _estaSincronizandoNfceOffline = true;
                var verificacao = new VerificaSeTemErros();

                var fazSincronizacao = verificacao.ExecutaVerificacao();

                verificacao.VerificaSeTemNFCeOfflineComErrosDeTransmissao();

                var avisos = verificacao.ObterAvisos();

                if (timer != null)
                {
                    TemErroNfce = avisos.Count > 0;
                    _modelAvisos.AddAvisos(avisos);
                }

                if (timer == null)
                    avisos.ForEach(a => { DialogBox.MostraInformacao(a.Mensagem); });

                if (!fazSincronizacao) return;

                var enviarNfceLote = new EnviarNfceEmLotesZeus(verificacao.ListaDeNfces, SessaoSistemaNfce.GetDadosSefaz());

                await enviarNfceLote.EnviaAsync();

                var ultimaException = enviarNfceLote.UltimaException;

                if (TrataErroDaSincronizacaoEmSegundoPlano(ultimaException)) return;

                Application.Current.Dispatcher.Invoke(
                    () =>
                    {
                        VerificaSeTemAviso(enviarNfceLote.NfceNaoEmitida,
                            enviarNfceLote.GrupoLotes,
                            timer == null);
                    });
            }
            catch (Exception ex)
            {
                _estaSincronizandoNfceOffline = false;
                try
                {
                    if (timer == null)
                        DialogBox.MostraAviso(ex.Message);

                    if (timer != null)
                    {
                        _modelAvisos.AddAviso(FontAwesomeIcon.Warning,
                            "Ocorreu algum erro inesperado, ligar para o suporte");
                        TemErroNfce = true;
                    }
                }
                catch (Exception)
                {
                    // ignore
                }
            }
            finally
            {
                _estaSincronizandoNfceOffline = false;
                OnConcluiuSincronizacaoNfceOffline();
                timer?.Start();
            }
        }

        private bool TrataErroDaSincronizacaoEmSegundoPlano(Exception ultimaException)
        {
            if (ultimaException == null) return false;

            if (ultimaException.GetType() == typeof(WebException))
            {
                _modelAvisos.AddAviso(FontAwesomeIcon.Warning,
                    "Ocorreu algum erro de conexão com a internet. Porfavor verificar o mesmo ou entrar em modo contingência offline");
                return true;
            }

            if (ultimaException.GetType() == typeof(Exception))
            {
                _modelAvisos.AddAviso(FontAwesomeIcon.Warning, ultimaException.Message);
                return true;
            }

            _modelAvisos.AddAviso(FontAwesomeIcon.Warning, "Ocorreu algum erro inesperado, ligar para o suporte");
            return true;
        }

        public event EventHandler ConcluiuSincronizacaoNfceOffline;

        private void VerificaSeTemAviso(List<Nfce> nfceNaoEmitida, List<GrupoLote> grupoLotes, bool semTimer = false)
        {
            if (!semTimer)
                _modelAvisos.Itens.Clear();

            if (nfceNaoEmitida.Count == 0)
            {
                if (semTimer)
                {
                    DialogBox.MostraInformacao("Todas as NFC-es TipoEmissao foram transmitidas com sucesso. Obrigado.");
                    OnConcluiuSincronizacaoNfceOffline();
                    return;
                }

                _modelAvisos.AddAviso(FontAwesomeIcon.ThumbsOutlineUp,
                    "Todas as NFC-es TipoEmissao foram transmitidas com sucesso. Obrigado.");
                return;
            }

            if (semTimer)
            {
                var mensagem = new StringBuilder();
                mensagem.AppendLine("Existem " + nfceNaoEmitida.Count +
                                    " NFC-es TipoEmissao que não foram transmitidas porfavor\n");

                grupoLotes.ForEach(
                    g =>
                    {
                        mensagem.AppendLine("Houve " + g.Quantidade + " erros. Motivo " + g.CodigoAutorizacao + " " +
                                            g.Mensagem);
                    });

                DialogBox.MostraInformacao(mensagem.ToString());
                OnConcluiuSincronizacaoNfceOffline();
                return;
            }

            _modelAvisos.AddAviso(FontAwesomeIcon.ThumbsOutlineDown,
                "Existem " + nfceNaoEmitida.Count + " NFC-es TipoEmissao que não foram transmitidas porfavor");

            grupoLotes.ForEach(
                g =>
                {
                    _modelAvisos.AddAviso(FontAwesomeIcon.ThumbsOutlineDown,
                        "Houve " + g.Quantidade + " erros. Motivo " + g.CodigoAutorizacao + " " + g.Mensagem);
                });
        }

        private async void TimerVenderProduto_Tick(object sender, EventArgs e)
        {
            _timerVenderProduto.Stop();

            try
            {
                if (_produtosEmEspera.Count == 0) return;

                var comando = StatusVenda.GetCommando();

                InformacaoCaixa = "Venda em andamento" + Homologacao();
                await Task.Run(() => comando.ExecutaAcao(this, _produtosEmEspera[0]));
            }
            finally
            {
                _timerVenderProduto.Start();
            }
        }

        public void IniciaVenda(ProdutoBaseDTO item)
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                var repositorio = new RepositorioProdutoNfce(sessao);
                var produto = repositorio.GetPeloId(item.Id);

                IniciaVenda(produto);
            }
        }

        public void IniciaVenda(ProdutoNfce produto = null)
        {
            if (string.IsNullOrWhiteSpace(CodigoBarras) && produto == null) return;
            if (produto != null && CodigoBarras == null) CodigoBarras = string.Empty;
            if (VerificaSeEstaAdicionandoQuantidade()) return;

            BloquearCaixaCorrigirVendasPendente();

            FecharSistemaSeAcessoInvalido();
            StatusCaixa.GetComando().ExecutaAcao(this, CodigoBarras.Trim(), produto);

            CodigoBarras = string.Empty;
            InformacaoAcaoBarras = "Venda";
        }

        private bool VerificaSeEstaAdicionandoQuantidade()
        {
            var regex = new Regex(@"^\*[0-9,]?.+");
            var temUmAsteristico = regex.IsMatch(CodigoBarras);

            if (temUmAsteristico)
            {
                var codigoBarras = CodigoBarras;
                codigoBarras = codigoBarras.Replace("*", string.Empty);

                try
                {
                    var quantidade = decimal.Parse(codigoBarras, NumberStyles.AllowDecimalPoint);

                    if (quantidade == 0)
                    {
                        throw new ArgumentException("Preciso que informe uma quantidade maior que 0 (zero)!");
                    }

                    Quantidade = quantidade;
                }
                catch (FormatException)
                {
                    throw new ArgumentException("Quantidade digitada após o modificador * é inválida.");
                }

                CodigoBarras = string.Empty;
                return true;
            }
            return false;
        }

        public void FecharSistemaSeAcessoInvalido()
        {
            if (SessaoSistemaNfce.AcessoConcedido != null)
                return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                DialogBox.MostraAviso("Licenciamento", SessaoSistemaNfce.MensagemErroAcesso);
                Application.Current.Shutdown();
            });
        }

        public void AtualizaTotais()
        {
            var total = Itens.Where(i => i.Cancelado == false).ToList().Sum(i => i.TotalUnitario);

            Total = total;
        }

        public void AtualizaListas(NfceItem nfceItem, ItemEspera item)
        {
            Itens.Insert(0, nfceItem);
            OnAtualizarListaItens();
            _produtosEmEspera.Remove(item);
        }

        public void AdicionaProdutoNaListaDeEspera(ItemEspera itemEspera)
        {
            ValidaProduto(itemEspera.Produto);

            itemEspera.Produto.Quantidade = Quantidade;
            _produtosEmEspera.Add(itemEspera);
            Quantidade = 1;
        }

        private void ValidaProduto(ProdutoNfce produto)
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                try
                {
                    BoqueioEstoqueHelper.ChecaEstoqueNegativoNfce(produto, Quantidade, sessao);
                }
                catch (EstoqueException ex)
                {
                    throw new InvalidOperationException(ex.Message, ex);
                }
            }

            if (string.IsNullOrEmpty(produto.Ncm))
                throw new InvalidOperationException("O produto não tem ncm, porfavor inserir ncm no produto");

            if (produto.Ncm.Length != 8)
                throw new InvalidOperationException(
                    "O ncm do produto está menor do que o aceitavel, o tamanho do ncm e fixado em 8 digitos");

            if (produto.Nome.IsNullOrEmpty())
                throw new InvalidOperationException($"Produto de #ID: {produto.Id} o nome está vazio, adicionar nome no mesmo");
        }

        public void OnSelecionaConteudoTextBox()
        {
            SelecionaConteudoTextBoxKg?.Invoke(this, EventArgs.Empty);
        }

        public void RemoverItem()
        {
            StatusCaixa = StatusCaixa.CancelamentoItem;
            InformacaoAcaoBarras = "Cancelar Item";
        }

        public void AdicionaQuantidade()
        {
            StatusCaixa = StatusCaixa.AlteracaoDeQuantidade;
            InformacaoAcaoBarras = "Adiciona Quantidade";
        }

        public void AlterarItem()
        {
            StatusCaixa = StatusCaixa.AlterarItem;
            InformacaoAcaoBarras = "Alterar Item";
        }

        public void LimpaDados()
        {
            Nfce = null;
            Itens.Clear();
            CodigoBarras = string.Empty;
            Quantidade = 1;
            Total = 0;
            ProdutoManual = null;
            StatusCaixa = StatusCaixa.Normal;
            StatusVenda = StatusVenda.Livre;
            InformacaoCaixa = "Caixa Livre" + Homologacao();
            NomeVendedor = _vendedor?.Nome ?? string.Empty;
            DefinirTabelaPadrao();
        }

        public void RecuperarVenda(object sender, NfceEvent e)
        {
            LimpaDados();
            Nfce = e.Nfce;

            RecuperaVenda();

            var isRecuperar = IsContemHistoricoNaoFinalizado;

            if (Nfce.Emissao != null && Nfce.NumeroFiscal == 0)
            {
                isRecuperar = false;
            }

            if (isRecuperar)
            {
                OnFinalizacaoRapidaHandler();
            }

        }

        public void RecuperaVenda()
        {
            NomeVendedor = Nfce.Vendedor != null ? Nfce.Vendedor.Nome : string.Empty;
            Nfce.ObterOsItens().OrderBy(i => i.NumeroItem).ForEach(i => { Itens.Insert(0, i); });

            StatusVenda = StatusVenda.Venda;
            InformacaoCaixa = "Venda em andamento" + Homologacao();


            _tabelaPrecoSelecionada = Nfce.TabelaPreco == null ? null : new TabelaPrecoDto
            {
                Descricao = Nfce.TabelaPreco.Descricao,
                Id = Nfce.TabelaPreco.Id
            };

            PropriedadeAlterada(nameof(TabelaPrecoSelecionada));

            AtualizaTotais();
        }

        public void FinalizarVenda()
        {
            if (Nfce == null)
                throw new InvalidOperationException("Porfavor iniciar uma venda antes de tentar subtotalizar a mesma");

            if (Itens.Count(i => i.Cancelado == false) == 0)
                throw new InvalidOperationException(
                    "Porfavor passar pelo menos um item antes de tentar subtotalizar a mesma");

            if (Nfce.ObterOsItens() == null)
                Nfce.AddItens(Itens);

            InformacaoCaixa = "Finalizando Venda" + Homologacao();
        }

        public void FinalizouPagamento(object sender, EventArgs e)
        {
            LimpaDados();
        }

        public void ConsultaStatus()
        {
            Application.Current.Dispatcher.Invoke(ProgressBarAgil4.ShowProgressBar);

            RetornoConsultaStatus consultaStatus = null;
            SatResposta satResposta = null;
            retConsStatServ consStatServ = null;

            if (SessaoSistemaNfce.IsEmissorNFce())
            {
                try
                {
                    var consultador = new ConsultarStatusZeus(SessaoSistemaNfce.GetDadosSefaz());

                    consultaStatus = consultador.ConsultarStatus();
                }
                catch (Exception e)
                {
                    Application.Current.Dispatcher.Invoke(() => DialogBox.MostraAviso(e.Message));
                }
            }

            if (SessaoSistemaNfce.IsEmissorSat())
            {
                var acbrSat = CriaAcbrSat.Criar();
                new AtivarSat(acbrSat).Ativar();

                using (acbrSat)
                {
                    satResposta = acbrSat.ConsultarSAT();
                }
            }

            Application.Current.Dispatcher.Invoke(ProgressBarAgil4.CloseProgressBar);


            if (satResposta != null && satResposta.MensagemSEFAZ.IsNotNullOrEmpty())
            {
                DialogBox.MostraInformacao("Mensagem Sefaz: " + satResposta.MensagemSEFAZ);
            }

            if (SessaoSistemaNfce.IsEmissorNFce())
            {
                DialogBox.MostraInformacao(consultaStatus?.XMotivo);
            }

            if (SessaoSistemaNfce.IsEmissorSat())
            {
                DialogBox.MostraInformacao(satResposta?.MensagemDoCodigoDeRetorno().Mensagem + "\nObservação: "
                        + satResposta?.MensagemDoCodigoDeRetorno().Observacao
                        + "\n" + satResposta?.MensagemRetorno);
            }
        }

        public async Task CancelarVendaEmAndamento()
        {
            await Task.Run(() => CancelarAction());
        }

        private void CancelarAction()
        {
            Application.Current.Dispatcher.Invoke(ProgressBarAgil4.ShowProgressBar);

            try
            {
                if (Nfce == null || Nfce.Id == 0)
                {
                    return;
                }

                ServicoControleFinanceiroNfce financeiro = null;

                using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
                using (var transacao = sessao.BeginTransaction())
                {
                    var repositorio = new RepositorioNfce(sessao);

                    Itens.Where(item => item.Cancelado == false).ForEach(i =>
                    {
                        var estoqueServico = EstoqueServicoNfce.Cria(
                            sessao,
                            i.Produto,
                            OrigemEventoEstoque.CancelamentoNfce,
                            TipoEventoEstoque.Entrada,
                            i.Quantidade
                        );

                        estoqueServico.Acrescentar();

                        sessao.Flush();
                        sessao.Clear();
                    });

                    new GeraRegistroCaixa(Nfce, sessao, SessaoSistemaNfce.Usuario).EstornarCaixa();

                    if (Nfce.Cobranca != null)
                    {
                        //Fluxo de cancelamento de venda com inutilização de número
                        financeiro = new ServicoControleFinanceiroNfce(SessaoSistemaNfce.Usuario);
                        financeiro.CancelarFinanceiroNfce(Nfce);
                    }

                    Nfce.FoiCancelada();

                    repositorio.SalvarESincronizar(Nfce);

                    NfceInutilizacao inutilizacao = null;

                    if (SessaoSistemaNfce.IsEmissorNFce() && Nfce.NumeroDocumento != 0)
                    {
                        try
                        {
                            inutilizacao = EnviaInutilizacaoSefaz();
                        }
                        catch (JaInutilizadoException)
                        {
                            //ignore
                        }
                    }

                    if (inutilizacao.IsNotNull())
                    {
                        repositorio.SalvarESincronizar(inutilizacao);
                    }

                    financeiro?.ComitarAlteracoes();
                    transacao.Commit();
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    LimpaDados();
                    AtualizarStatusVendaPendente();
                });
            }
            catch (NFeException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            finally
            {
                Application.Current.Dispatcher.Invoke(ProgressBarAgil4.CloseProgressBar);
            }
        }

        private NfceInutilizacao EnviaInutilizacaoSefaz()
        {
            const string justificativaInutilizacao = "NUMERO INUTILIZADO AO CANCELAR UMA NFC-E NAO TRANSMITIDA";
            var serie = Nfce.Serie;

            var ano = byte.Parse(Nfce.EntradaSaidaEm.ToString("yy"));
            var cnpj = Nfce?.Emitente?.Empresa?.Cnpj ?? Nfce?.Emissao?.EmissorFiscal?.Empresa?.Cnpj;
            var codigoIbge = Nfce?.Emitente?.Empresa?.Estado?.CodigoIbge ?? Nfce?.Emissao?.EmissorFiscal?.Empresa?.Estado?.CodigoIbge;

            var inutilizacaoZeus = new NfeInutilizacaoZeus(
                ano,
                serie,
                Nfce.NumeroDocumento,
                Nfce.NumeroDocumento,
                justificativaInutilizacao,
                cnpj)
            {
                DadosServicoSefaz = SessaoSistemaNfce.GetDadosSefaz()
            };

            var sucesso = inutilizacaoZeus.EnviarParaSefaz();

            return new NfceInutilizacao
            {
                NumeroFinal = Nfce.NumeroDocumento,
                Ano = ano,
                CnpjEmitente = cnpj,
                CodigoUfSolicitante = int.Parse(codigoIbge.ToString()),
                Justificativa = justificativaInutilizacao,
                ModeloDocumento = ModeloDocumento.NFCe,
                NumeroInicial = Nfce.NumeroDocumento,
                Protocolo = sucesso.Protocolo,
                Serie = serie,
                Uuid = GuuidHelper.Computar(DateTime.Now.ToString("G") + SessaoSistemaNfce.Configuracao.BindTerminal)
            };
        }

        public void AbrirTelaAvisos()
        {
            var dialog = new AvisosForm(_modelAvisos);
            dialog.ShowDialog();
        }

        public static string Homologacao()
        {
            var tipoAmbiente = TipoAmbiente.Producao;

            if (SessaoSistemaNfce.Configuracao.EmissorFiscal.FlagSat)
                tipoAmbiente = SessaoSistemaNfce.Configuracao.EmissorFiscal.EmissorFiscalSat.Ambiente;

            if (SessaoSistemaNfce.Configuracao.EmissorFiscal.FlagNfce)
                tipoAmbiente = SessaoSistemaNfce.Configuracao.EmissorFiscal.EmissorFiscalNfce.Ambiente;

            return tipoAmbiente == TipoAmbiente.Homologacao
                ? " => SERVIDOR DE HOMOLOGAÇÃO"
                : string.Empty;
        }

        public Nfce AtualizaObjetoNfce()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            {
                var repositorio = new RepositorioNfce(sessao);
                var nfce = repositorio.GetPeloId(Nfce.Id);
                return nfce;
            }
        }

        private void AtivaAvisoErros()
        {
            if (TemErroFinanceiro)
            {
                AtivaAvisos = TemErroFinanceiro;
                return;
            }

            if (TemErroNfce)
            {
                AtivaAvisos = TemErroNfce;
                return;
            }

            AtivaAvisos = false;
        }

        private void CarregarLogo()
        {
            var logo = ConverteImage.ByteEmImagem(SessaoSistemaNfce.ConfiguracaoFrenteCaixa?.Logo);


            Logo = logo;
        }

        private void IniciaTimerStatusServico()
        {
            _timerStatusServidor = new DispatcherTimer(DispatcherPriority.Background, Application.Current.Dispatcher);
            _timerStatusServidor.Dispatcher.Thread.IsBackground = true;
            _timerStatusServidor.Tick += DispatcherTimer_StatusServidor;
            _timerStatusServidor.Interval = new TimeSpan(0, 0, 0, 10, 0);
            _timerStatusServidor.Start();
        }

        private void DispatcherTimer_StatusServidor(object sender, EventArgs e)
        {
            VerificaStatusServidor();
        }

        private void VerificaStatusServidor()
        {
            new Thread(() =>
            {
                try
                {
                    new ConsultarStatusDoServico().Executar("FusionNfceSincronizador");
                    StatusServidor = FontAwesomeIcon.Database;
                    CorStatus = Brushes.LawnGreen;
                }
                catch (Exception)
                {
                    StatusServidor = FontAwesomeIcon.Database;
                    CorStatus = Brushes.DarkRed;
                }
            })
            {
                IsBackground = true
            }.Start();
        }

        public async void SincronziarNfceOffline()
        {
            if (_estaSincronizandoNfceOffline)
            {
                DialogBox.MostraInformacao(
                    "Existe uma sincronização em andamento, porfavor aguardar a mesma para depois tentar");
                return;
            }
            await Task.Run(() => TentaEnviarNfceOffline(null));
        }

        private void OnConcluiuSincronizacaoNfceOffline()
        {
            ConcluiuSincronizacaoNfceOffline?.Invoke(this, EventArgs.Empty);
        }

        public async void FinalizacaoRapida()
        {
            try
            {
                ControleCaixaNfceFacade.ThrowExcetpionSeNaoExistirCaixaAberto(SessaoSistemaNfce.Usuario);

                if (Nfce == null)
                {
                    throw new InvalidOperationException("Preciso de um documento aberto para finalizar!");
                }

                if (_restricaoClienteObrigatorio.NecessarioCliente(Nfce))
                {
                    throw new InvalidOperationException("Não é possível utilizar a finalização rápida. Necessário informar cliente na Nota!");
                }

                await ProcessaFinalizacaoRapida();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (WebSocketException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro(ex.Message, ex);
            }
        }

        private async Task ProcessaFinalizacaoRapida()
        {
            ProgressBarAgil4.ShowProgressBar();

            await Task.Run(async () =>
            {
                try
                {
                    if (Nfce == null)
                        throw new InvalidOperationException(
                            "Iniciar uma venda antes de tentar subtotalizar a mesma");

                    if (Itens.Count(i => i.Cancelado == false) == 0)
                        throw new InvalidOperationException(
                            "Preciso de pelo menos um item antes de tentar subtotalizar a mesma");

                    InformacaoCaixa = "Finalização de venda rapida processando" + Homologacao();

                    var nfce = BuscarNfce();

                    var totalPago = nfce.ObterTotalPago();

                    if (totalPago != nfce.TotalNfce)
                    {
                        SalvarInformacoesNFCe();
                    }

                    if (SessaoSistemaNfce.IsEmissorNFce() && SessaoSistemaNfce.EstaEmContingencia())
                    {
                        nfce.Contingencia = SessaoSistemaNfce.Contingencia;
                    }

                    SalvarNfce(nfce);

                    AutorizacaoModelResposta resposta = null;
                    Autorizacao.SatFiscal.AutorizacaoModelResposta respostaSat = null;

                    await Application.Current.Dispatcher.Invoke(async () =>
                    {
                        nfce = BuscarNfce();

                        if (SessaoSistemaNfce.IsEmissorSat())
                        {
                            var autorizaSatView = new AutorizacaoSatView(nfce);

                            respostaSat = await autorizaSatView.AutorizaAsync();

                            if (respostaSat.Sucesso)
                            {
                                autorizaSatView.Close();
                            }
                        }

                        if (SessaoSistemaNfce.IsEmissorNFce())
                        {
                            var autorizaView = new AutorizacaoNfceView(nfce);
                            resposta = await autorizaView.AutorizaAsync();

                            if (resposta.Sucesso)
                            {
                                autorizaView.Close();
                            }
                         }

                    });


                    var nfceBuscada = BuscarNfce();

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (resposta?.Sucesso == false)
                        {
                            LimpaDados();
                            Nfce = nfceBuscada;
                            RecuperaVenda();
                            return;
                        }

                        if (respostaSat?.Sucesso == false)
                            return;

                        try
                        {
                            FusionCore.FusionAdm.Acbr.Servicos.AbrirGaveta.Executar();

                            if (SessaoSistemaNfce.IsEmissorNFce())
                            {
                                new ServicoImpressaoNfce(nfceBuscada.Id, _danfeFacade).Imprimir();
                                return;
                            }

                            if (SessaoSistemaNfce.IsEmissorSat())
                            {
                                var xmlDto = new XmlAutorizadoDto { Xml = nfceBuscada.FinalizaEmissaoSat.XmlRetorno };
                                DanfeSatHelper.Imprimir(xmlDto, SessaoSistemaNfce.Preferencia.NomeImpressora, SessaoSistemaNfce.Preferencia.NomeFantasiaCustomizado);
                            }

                        }
                        finally
                        {
                            LimpaDados();
                        }
                    });

                }
                finally
                {
                    Application.Current.Dispatcher.Invoke(ProgressBarAgil4.CloseProgressBar);
                    AtualizarStatusVendaPendente();
                }
            });
        }

        private void SalvarNfce(Nfce nfce)
        {
            Nfce.Vendedor = _vendedor;

            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioNfce(sessao);

                repositorio.SalvarESincronizar(nfce);
                
                transacao.Commit();
            }
        }

        private Nfce BuscarNfce()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            {
                var repositorio = new RepositorioNfce(sessao);
                return repositorio.GetPeloId(Nfce.Id);
            }
        }

        private void SalvarInformacoesNFCe()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                SalvarFormasPagamento(sessao);
                SalvarEmitente(sessao);

                transacao.Commit();
            }
        }

        private void SalvarEmitente(ISession sessao)
        {
            if (Nfce.Emitente != null) return;

            var repositorioNfce = new RepositorioNfce(sessao);

            var emitente = new NfceEmitente
            {
                Nfce = Nfce,
                Empresa = SessaoSistemaNfce.Configuracao.EmissorFiscal.Empresa
            };

            Nfce.Emitente = emitente;

            repositorioNfce.SalvarEmitente(Nfce.Emitente);
        }

        private void SalvarFormasPagamento(ISession sessao)
        {
            var repositorioFormaPagamentoNfce = new RepositorioNfce(sessao);

            var formaPagamento = new Dinheiro(((decimal)(Total + Nfce.TotalAcrescimo - Nfce.DescontoNoTotalSoma())).Arredonda());

            repositorioFormaPagamentoNfce.SalvarFormaPagamento(new FormaPagamentoNfce
            {
                Nfce = Nfce,
                IdFormaPagamento = formaPagamento.Id.ToString("D2"),
                Nome = formaPagamento.Descricao,
                ValorPagamento = formaPagamento.Valor
            });
        }

        public void ExtrairLogs()
        {
            var acbr = CriaAcbrSat.Criar();
            new AtivarSat(acbr).Ativar();

            using (acbr)
            {
                var resposta = acbr.ExtrairLogs();

                if (resposta.MensagemSEFAZ.IsNotNullOrEmpty())
                {
                    DialogBox.MostraInformacao("Mensagem Sefaz: " + resposta.MensagemSEFAZ);
                }

                if (resposta.CodigoDeRetorno != 15000)
                {
                    throw new InvalidOperationException(resposta.MensagemDoCodigoDeRetorno().Mensagem + "\nObservação: " + resposta.MensagemDoCodigoDeRetorno().Observacao);
                }

                var assemblyDir = DiretorioAssembly.GetPastaTemp();
                var tempFile = Path.Combine(assemblyDir, Md5Helper.ComputaUnique() + ".txt");

                File.WriteAllText(tempFile, resposta.Log);
                Process.Start(tempFile);
            }
        }

        public void DesbloquearSat()
        {
            var acbr = CriaAcbrSat.Criar();
            new AtivarSat(acbr).Ativar();

            using (acbr)
            {
                var resposta = acbr.DesbloquearSAT();

                if (resposta.MensagemSEFAZ.IsNotNullOrEmpty())
                {
                    DialogBox.MostraInformacao("Mensagem Sefaz: " + resposta.MensagemSEFAZ);
                }

                if (resposta.CodigoDeRetorno != 17000)
                {
                    throw new InvalidOperationException(resposta.MensagemDoCodigoDeRetorno().Mensagem + "\nObservação: " 
                        + resposta.MensagemDoCodigoDeRetorno().Observacao + "\n" + resposta.MensagemRetorno);
                }
            }
        }

        public void BloquearSat()
        {
            var acbr = CriaAcbrSat.Criar();
            new AtivarSat(acbr).Ativar();

            using (acbr)
            {
                var resposta = acbr.BloquearSAT();

                if (resposta.MensagemSEFAZ.IsNotNullOrEmpty())
                {
                    DialogBox.MostraInformacao("Mensagem Sefaz: " + resposta.MensagemSEFAZ);
                }

                if (resposta.CodigoDeRetorno != 16000)
                {
                    throw new InvalidOperationException(resposta.MensagemDoCodigoDeRetorno().Mensagem + "\nObservação: " 
                        + resposta.MensagemDoCodigoDeRetorno().Observacao
                        + "\n" + resposta.MensagemRetorno);
                }
            }
        }

        public void AtualizarSat()
        {
            var acbr = CriaAcbrSat.Criar();
            new AtivarSat(acbr).Ativar();

            using (acbr)
            {
                var resposta = acbr.AtualizarSoftwareSAT();

                if (resposta.MensagemSEFAZ.IsNotNullOrEmpty())
                {
                    DialogBox.MostraInformacao("Mensagem Sefaz: " + resposta.MensagemSEFAZ);
                }

                if (resposta.CodigoDeRetorno != 14000)
                {
                    throw new InvalidOperationException(resposta.MensagemDoCodigoDeRetorno().Mensagem + "\nObservação: "
                        + resposta.MensagemDoCodigoDeRetorno().Observacao
                        + "\n" + resposta.MensagemRetorno);
                }

                if (resposta.CodigoDeRetorno == 14000)
                {
                    DialogBox.MostraInformacao(resposta.MensagemDoCodigoDeRetorno().Mensagem + "\nObservação: "
                        + resposta.MensagemDoCodigoDeRetorno().Observacao
                        + "\n" + resposta.MensagemRetorno);
                }
            }
        }

        public void ConsultarStatusSAT()
        {
            var acbr = CriaAcbrSat.Criar();
            new AtivarSat(acbr).Ativar();

            using (acbr)
            {
                var resposta = acbr.ConsultarStatusOperacional();

                if (resposta.MensagemSEFAZ.IsNotNullOrEmpty())
                {
                    DialogBox.MostraInformacao("Mensagem Sefaz: " + resposta.MensagemSEFAZ);
                }

                if (resposta.CodigoDeRetorno != 10000)
                {
                    throw new InvalidOperationException(resposta.MensagemDoCodigoDeRetorno().Mensagem + "\nObservação: "
                        + resposta.MensagemDoCodigoDeRetorno().Observacao
                        + "\n" + resposta.MensagemRetorno);
                }

                if (resposta.CodigoDeRetorno == 10000)
                {
                    DialogBox.MostraInformacao(resposta.MensagemDoCodigoDeRetorno().Mensagem + "\nObservação: "
                        + resposta.MensagemDoCodigoDeRetorno().Observacao
                        + "\n" + resposta.MensagemRetorno);
                }


                var resultado = new StringBuilder();

                resultado.Append("Série: ").Append(resposta.RetornoLst[0]).Append(Environment.NewLine);
                resultado.Append("Nível Bateria: ").Append(resposta.RetornoLst[14]).Append(Environment.NewLine);
                resultado.Append("Memória Total: ").Append(resposta.RetornoLst[15]).Append(Environment.NewLine);
                resultado.Append("Memória Total Usada: ").Append(resposta.RetornoLst[16]).Append(Environment.NewLine);
                resultado.Append("Versão do Software Básico: ").Append(resposta.RetornoLst[18]).Append(Environment.NewLine);
                resultado.Append("Versão Layout: ").Append(resposta.RetornoLst[19]).Append(Environment.NewLine);
                resultado.Append("Ultimo CF-e Emitido: ").Append(resposta.RetornoLst[20]).Append(Environment.NewLine);

                var dataEmissaoCertificado = resposta.RetornoLst[25].Insert(4, "-").Insert(7, "-");

                resultado.Append("Data de emissão do certificado instalado: ").Append(Convert.ToDateTime(dataEmissaoCertificado)).Append(Environment.NewLine);

                var dataVencimentoCertificado = resposta.RetornoLst[26].Insert(4, "-").Insert(7, "-");

                resultado.Append("Data de vencimento do certificado instalado: ").Append(Convert.ToDateTime(dataVencimentoCertificado)).Append(Environment.NewLine);

                var estadoOperacao = new StringBuilder();

                switch (resposta.RetornoLst[27])
                {
                    case "0":
                        estadoOperacao.Append("DESBLOQUEADO");
                        break;
                    case "1":
                        estadoOperacao.Append("BLOQUEIO SEFAZ");
                        break;
                    case "2":
                        estadoOperacao.Append("BLOQUEIO CONTRIBUINTE");
                        break;
                    case "3":
                        estadoOperacao.Append("BLOQUEIO AUTÔNOMO");
                        break;
                    case "4":
                        estadoOperacao.Append("BLOQUEIO PARA DESATIVAÇÃO");
                        break;
                }

                resultado.Append("Estado Operação: ").Append(estadoOperacao).Append(Environment.NewLine);


                var assemblyDir = DiretorioAssembly.GetPastaTemp();
                var tempFile = Path.Combine(assemblyDir, Md5Helper.ComputaUnique() + ".txt");

                File.WriteAllText(tempFile, resultado.ToString());
                Process.Start(tempFile);

            }
        }

        private void OnFinalizacaoRapidaHandler()
        {
            FinalizacaoRapidaHandler?.Invoke(this, EventArgs.Empty);
        }

        public void AbrirGaveta()
        {
            FusionCore.FusionAdm.Acbr.Servicos.AbrirGaveta.Executar();
        }

        public void AtualizarAbrirGaveta()
        {
            IsImpressaoDiretaAtiva =  SessaoSistemaNfce.ImpressaoDireta.Ativa;
        }

        public void AtualizarStatusVendaPendente()
        {
            IsVendasPendente = ExisteVendaPendente();
            IsVendasPendenteOffline = ExisteVendasPendenteOffline();
        }

        private bool ExisteVendasPendenteOffline()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                var quantidadePendencias = new RepositorioNfce(sessao).BuscaQuantidadeElegiveisPendenteOffline(Nfce);
                var existeVenda = quantidadePendencias > 0;
                return existeVenda;
            }
        }

        private bool ExisteVendaPendente()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                var quantidadePendencias = new RepositorioNfce(sessao).BuscaQuantidadeElegiveisParaRecuperacao(Nfce);
                var existeVenda = quantidadePendencias > 0;
                return existeVenda;
            }
        }

        public void VerificaVendaEmAndamento()
        {
            if (Nfce != null && Nfce.Id != 0)
            {
                throw new InvalidOperationException("Existe uma venda em andamento");
            }
        }

        public void BloquearCaixaCorrigirVendasPendente()
        {
            if (SessaoSistemaNfce.ConfiguracaoFrenteCaixa == null ||
                SessaoSistemaNfce.ConfiguracaoFrenteCaixa.IsBloquearVendaParaResolverPendencia == false) return;

            var vendaPendenteMensal = CriaVendaModelSeNaoExistir();

            if (vendaPendenteMensal.IsResolvido) return;

            VerificaSePendenciasForamResolvida(vendaPendenteMensal);

            var today = DateTime.Today;

            if (today.DayOfWeek != DayOfWeek.Saturday 
                && today.DayOfWeek != DayOfWeek.Sunday  
                && vendaPendenteMensal.IsNaoResolvido)
            {
                throw new InvalidOperationException("Caixa bloqueado para vendas, resolva as vendas não finalizadas para continuar");
            }
        }

        private void VerificaSePendenciasForamResolvida(VendaPendenteMensal vendaPendenteMensal)
        {
            var dataFinal = new DateTime(vendaPendenteMensal.Ano, vendaPendenteMensal.Mes, 1).UltimoDiaDoMesAtual();
            
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioNfce = new RepositorioNfce(sessao);
                var repositorioVendaPendenteMensal = new RepositorioVendaPendenteMensal(sessao);

                var quantidadePendenciasOffline = repositorioNfce.BuscaQuantidadeElegiveisPendenteOffline(Nfce, dataFinal);
                var isNaoExisteVendaPendenteOffline = quantidadePendenciasOffline == 0;

                var quantidadePendencias = repositorioNfce.BuscaQuantidadeElegiveisParaRecuperacao(Nfce, dataFinal);
                var isNaoExisteVendaPendenteNormal = quantidadePendencias == 0;

                vendaPendenteMensal.IsResolvido = isNaoExisteVendaPendenteNormal && isNaoExisteVendaPendenteOffline;
                repositorioVendaPendenteMensal.Salvar(vendaPendenteMensal);

                transacao.Commit();
            }
        }

        private static VendaPendenteMensal CriaVendaModelSeNaoExistir()
        {
            var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao();
            var transacao = sessao.BeginTransaction();
            var repositorio = new RepositorioVendaPendenteMensal(sessao);
            VendaPendenteMensal vendaPendenteMensal;

            using (repositorio)
            {
                var dataMesAnterior = DateTime.Now.AddMonths(-1);

                vendaPendenteMensal = repositorio.ObterVendaPendentePeloAnoEMes(dataMesAnterior);

                if (vendaPendenteMensal.IsNull())
                {
                    vendaPendenteMensal = new VendaPendenteMensal
                    {
                        Ano = dataMesAnterior.Year,
                        Mes = dataMesAnterior.Month
                    };

                    repositorio.Salvar(vendaPendenteMensal);
                }

                transacao.Commit();
            }

            return vendaPendenteMensal;
        }

        public bool IsTemFormaDePagamento()
        {
            var isTemFormaDePagamento = Nfce != null && Nfce.ObterFormaPagamentoNfces() != null && Nfce.ObterFormaPagamentoNfces().Any();

            return isTemFormaDePagamento;
        }

        private void InicializaTef()
        {
            IsTefAtivo = SessaoSistemaNfce.ConfigTef.IsAtivo;
            var requisicao = FabricaTef.ObterRequisicao();
            requisicao.AguardandoResposta += TefAguardandoRequisicao;
            requisicao.ExibeMensagem += TefExibeMensagemAction;
            requisicao.ImprimirVia += TefImprimirViaAction;

            _tef = FabricaTef.ObterTefDial(requisicao);
            _tef.Inicializa();
        }

        public async Task Tef_Adm()
        {
            await Task.Run(() => { _tef.Adm(); });

            InformacaoCaixa = $"Caixa Livre {Homologacao()}";
        }

        private async void TefImprimirViaAction(object sender, IImprimeViaEventArgs e)
        {

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (e.IsTemViaCliente == false && e.IsTemViaEstabelecimento == false)
                {
                    new TefImpressaoFacade().Imprimir(e.ViaUnica, SessaoSistemaNfce.Preferencia.NomeImpressora);
                    return;
                }

                if (e.IsTemViaCliente == true && e.IsTemViaEstabelecimento == false)
                {
                    new TefImpressaoFacade().Imprimir(e.ViaCliente, SessaoSistemaNfce.Preferencia.NomeImpressora);
                    return;
                }

                if (e.IsTemViaEstabelecimento == true && e.IsTemViaCliente == false)
                {
                    new TefImpressaoFacade().Imprimir(e.ViaEstabelecimento,
                        SessaoSistemaNfce.Preferencia.NomeImpressora);
                    return;
                }


                if (e.IsTemVia1)
                {
                    new TefImpressaoFacade().Imprimir(e.Via1, SessaoSistemaNfce.Preferencia.NomeImpressora);
                }

                if (e.IsTemVia2)
                {
                    Thread.Sleep(3);
                    new TefImpressaoFacade().Imprimir(e.Via2, SessaoSistemaNfce.Preferencia.NomeImpressora);
                }
            });


        }

        private void TefExibeMensagemAction(object sender, ExibeMensagemEventArgs e)
        {
            DialogBox.MostraInformacao(e.Mensagem);
        }

        private async void TefAguardandoRequisicao(object sender, AguardaRespostaEventArgs e)
        {
            await Application.Current.Dispatcher.InvokeAsync(() => { InformacaoCaixa = $"{e.ArquivoSts} {e.Segundos}"; });
        }

        public ProdutoNfce BuscarProdutoPorId(int produtoId)
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            {
                return new RepositorioProdutoNfce(sessao).GetPeloId(produtoId);
            }
        }

        public void CarregarPreferencias()
        {
            using (var sessao = SessaoSistemaNfce.SessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioPreferencia(sessao);
                var preferencia = repositorio.BuscarExistente();

                SessaoSistemaNfce.Preferencia = preferencia;
            }
        }

        public void RemoverItemSilencioso(NfceItem nfceItem)
        {
            _itens.Remove(nfceItem);
        }

        public void AdicionarVendedor(VendedorNfce vendedor)
        {
            _vendedor = vendedor;
            NomeVendedor = vendedor.Nome;

            if (Nfce != null && Nfce.Id != 0)
                SalvarNfce(Nfce);
        }

        public string NomeVendedor
        {
            get => _nomeVendedor;
            set
            {
                _nomeVendedor = value;
                PropriedadeAlterada();
            }
        }

        public IEnumerable<TabelaPrecoDto> TabelasPrecosLista
        {
            get => GetValue<IEnumerable<TabelaPrecoDto>>();
            set => SetValue(value);
        }

        public TabelaPrecoDto TabelaPrecoSelecionada
        {
            get => _tabelaPrecoSelecionada;
            set
            {
                if (_tabelaPrecoSelecionada == value) return;
                _tabelaPrecoSelecionada = value;

                AtualizarPrecosComTabelaPreco();
                PropriedadeAlterada();
            }
        }

        public bool NaoTemVendedorCadastrado()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                return new RepositorioPessoaNfce(sessao).NaoExisteVendedorCadastrado();
            }
        }

        public VendedorNfce ObterVendedor()
        {
            return _vendedor;
        }

        public void CarregarTabelasPrecos()
        {
            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                var repositorio = new RepositorioTabelaPrecoNfce(sessao);
                var tabelas = repositorio.BuscarTodasTabelasDto();

                TabelasPrecosLista = tabelas;
            }
        }

        public void AtualizarPrecosComTabelaPreco()
        {
            if (Nfce == null) return;

            Nfce = AtualizaObjetoNfce();
            var tabelaPrecoNfce = CarregaTabelaPrecoPorId(TabelaPrecoSelecionada);

            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioNfce = new RepositorioNfce(sessao);

                Nfce.TabelaPreco = tabelaPrecoNfce;
                Nfce.RecalcularComTabelaPreco(new RepositorioTabelaPrecoNfce(sessao)
                    , new RepositorioIbptNfce(sessao));

                repositorioNfce.Salvar(Nfce);
                Nfce.ObterTodosItens().ForEach(repositorioNfce.SalvarItemESincronizar);

                transacao.Commit();
            }

            RecuperarVenda(null, new NfceEvent(Nfce));
        }

        public TabelaPrecoNfce CarregaTabelaPrecoPorId(TabelaPrecoDto tabela)
        {
            if (tabela == null) return null;

            using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
            {
                var repositorioTabelaPreco = new RepositorioTabelaPrecoNfce(sessao);

                return repositorioTabelaPreco.GetPeloId(tabela.Id);
            }
        }

        public void DefinirTabelaPadrao()
        {
            if (SessaoSistemaNfce.Preferencia != null)
            {
                TabelaPrecoSelecionada = TabelasPrecosLista.FirstOrDefault(i => i.Id == SessaoSistemaNfce.Preferencia.TabelaPrecoPadrao);
            }
        }

        private void OnAtualizarListaItens()
        {
            AtualizarListaItens?.Invoke(this, EventArgs.Empty);
        }
    }
}