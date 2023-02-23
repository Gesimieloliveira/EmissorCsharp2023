using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ACBrFramework;
using ACBrFramework.TEFD;
using FontAwesome.WPF;
using FusionCore.FusionAdm.Configuracoes;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.FusionPdv.Flags;
using FusionCore.FusionPdv.Helper;
using FusionCore.FusionPdv.Servico.Estoque;
using FusionCore.FusionPdv.Sessao;
using FusionCore.Helpers.Patterns.Observer;
using FusionCore.Helpers.Sincronizador;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Base.Execao;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionCore.Repositorio.Legacy.Flags;
using FusionLibrary.Helper.Conversores;
using FusionLibrary.Helper.Criptografia;
using FusionLibrary.VisaoModel;
using FusionPdv.Acbr;
using FusionPdv.Ecf;
using FusionPdv.ManipulaValor;
using FusionPdv.Modelos;
using FusionPdv.Modelos.FormaPagamento;
using FusionPdv.Servicos.ArquivoAuxiliar;
using FusionPdv.Servicos.Ecf;
using FusionPdv.Servicos.Ecf.EstadoEcf;
using FusionPdv.Servicos.Tef;
using FusionPdv.Servicos.ValidacaoInicial;
using FusionWPF.Base.Utils.Dialogs;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Util;
using EstoqueModel = FusionCore.FusionPdv.Servico.Estoque.EstoqueModel;

namespace FusionPdv.Visao.Principal
{
    public class CaixaModel : ModelBase, ICoreObservable<Caixa>
    {
        private readonly Caixa _caixa;
        private ACBrTEFD _tef;
        private string _codigoBarra;
        private string _informacaoRodape;
        private ObservableCollection<VendaEcfItemDt> _listaDeVendaEcfItem = new ObservableCollection<VendaEcfItemDt>();
        private string _mensagemConsultarProduto = "Para consultar produtos(F6)";
        private string _menssagemCaixa;
        private string _nomeCliente;
        private decimal _precoUnitario;
        private decimal _quantidade;
        private decimal _quantidadeAAdicionar = 1;
        private decimal _subTotal;
        private decimal _totalDoCupom;
        private decimal _totalDoItem;
        private VendaEcfDt _vendaEcf;
        private readonly IList<VendaEcfItemDt> _listaEsperaVendaEcfItem = new List<VendaEcfItemDt>();
        private DispatcherTimer _timerVender;
        private string _valorKiloItem;
        private bool _editarValorItem;
        private ProdutoDt _produtoPorKilo;
        private Caixa _observer;
        private bool _vendaEmAndamento;
        private DispatcherTimer _timerStatusServidor;
        private FontAwesomeIcon _statusServidor;
        private Brush _corStatus;
        private ViasGerencialTefCappta _viasGerencialTefCappta;
        private DispatcherTimer _timerFinanceiro;
        private bool _naoTransmitiuFinanceiro;
        private readonly int _tempoSincronizaFinanceiroSegundos = 1;
        private BitmapImage _logo;
        public bool TemErro { get; set; }
        public Exception UltimaException { get; set; }
        public bool UsandoTef { get; set; }

        public FontAwesomeIcon StatusServidor
        {
            get => _statusServidor;
            set
            {
                _statusServidor = value;
                PropriedadeAlterada();
            }
        }

        public Brush CorStatus
        {
            get => _corStatus;
            set
            {
                _corStatus = value;
                PropriedadeAlterada();
            }
        }

        public ProdutoDt ProdutoPorKilo
        {
            get => _produtoPorKilo;
            set
            {
                _produtoPorKilo = value;
                PropriedadeAlterada();
            }
        }

        public bool EditarValorItem
        {
            get => _editarValorItem;
            set
            {
                _editarValorItem = value;
                PropriedadeAlterada();
            }
        }

        public string ValorKiloItem
        {
            get => _valorKiloItem;
            set
            {
                _valorKiloItem = value;
                PropriedadeAlterada();
            }
        }

        public bool ClienteAdicionadoPeloCabecalho { get; set; }

        public string InformacaoRodape
        {
            get => _informacaoRodape;
            set
            {
                _informacaoRodape = value;
                PropriedadeAlterada();
            }
        }

        public string MensagemConsultarProduto
        {
            get => _mensagemConsultarProduto;
            set
            {
                _mensagemConsultarProduto = value;
                PropriedadeAlterada();
            }
        }

        public string MenssagemCaixa
        {
            get => _menssagemCaixa;
            set
            {
                _menssagemCaixa = value;
                PropriedadeAlterada();
            }
        }

        public decimal SubTotal
        {
            get => _subTotal;
            set
            {
                _subTotal = value;
                PropriedadeAlterada();
            }
        }

        public decimal TotalDoCupom
        {
            get => _totalDoCupom;
            set
            {
                _totalDoCupom = value;
                PropriedadeAlterada();
            }
        }

        public decimal TotalDoItem
        {
            get => _totalDoItem;
            set
            {
                _totalDoItem = value;
                PropriedadeAlterada();
            }
        }

        public decimal PrecoUnitario
        {
            get => _precoUnitario;
            set
            {
                _precoUnitario = value;
                PropriedadeAlterada();
            }
        }

        public decimal Quantidade
        {
            get => _quantidade;
            set
            {
                _quantidade = value;
                PropriedadeAlterada();
            }
        }

        public string NomeCliente
        {
            get => _nomeCliente;
            set
            {
                _nomeCliente = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<VendaEcfItemDt> ListaDeVendaEcfItem
        {
            get => _listaDeVendaEcfItem;
            set => _listaDeVendaEcfItem = value;
        }

        public decimal QuantidadeAAdicionar
        {
            get => _quantidadeAAdicionar;
            set
            {
                _quantidadeAAdicionar = value;
                PropriedadeAlterada();
            }
        }

        public string CodigoBarra
        {
            get => _codigoBarra;
            set
            {
                _codigoBarra = value;
                PropriedadeAlterada();
            }
        }

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

        public bool NaoTransmitiuFinanceiro
        {
            get => _naoTransmitiuFinanceiro;
            set
            {
                if (value == _naoTransmitiuFinanceiro) return;
                _naoTransmitiuFinanceiro = value;
                PropriedadeAlterada();
            }
        }

        public bool ReducaoZEmAndamento { get; set; }

        public CaixaModel(Caixa caixa)
        {
            _caixa = caixa;
            ReiniciarCaixa();
            CarregarLogo();
            IniciaTimerVender();
            IniciaTimerStatusServico();
            VerificaStatusServidor();
            InicializaTef();
        }

        private void CarregarLogo()
        {
            Logo = ConverteImage.ByteEmImagem(SessaoSistema.LogoCaixa?.Logo);
        }

        private void InicializaTef()
        {
            if (VerificaSeTefEstaAtivo()) return;
            AdicionaEventosTef();
            _tef.Initializar(TefTipo.TefDial);
        }

        private bool VerificaSeTefEstaAtivo()
        {
            var manipulaTef = new ManipulaTef();
            manipulaTef.CriarArquivoSeNaoExistir();
            var configTef = manipulaTef.LerArquivo();
            SessaoSistema.ConfigTef = configTef;

            _tef = AcbrFactory.ObterAcbrTefd(configTef);

            return !configTef.Ativo;
        }

        public void AdicionaEventosTef()
        {
            _tef.OnAguardaResp += OnAguardaResposta;
            _tef.OnExibeMensagem += OnExibeMensagem;
            _tef.OnBloqueiaMouseTeclado += OnBloqueiaMouseTeclado;
            _tef.OnRestauraFocoAplicacao += OnRestauraFocoAplicacao;
            _tef.OnLimpaTeclado += OnLimpaTeclado;
            _tef.OnComandaECF += OnComandaEcf;
            _tef.OnComandaECFSubtotaliza += OnComandaEcfSubtotaliza;
            _tef.OnComandaECFPagamento += OnComandaECFPagamento;
            _tef.OnComandaECFAbreVinculado += OnComandaECFAbreVinculado;
            _tef.OnComandaECFImprimeVia += OnComandaEcfImprimeVia;
            _tef.OnInfoECF += OnInfoEcf;
            _tef.OnAntesFinalizarRequisicao += OnAntesFinalizarRequisicao;
            _tef.OnDepoisConfirmarTransacoes += OnDepoisConfirmarTransacoes;
            _tef.OnAntesCancelarTransacao += OnAntesCancelarTransacao;
            _tef.OnDepoisCancelarTransacoes += OnDepoisCancelarTransacoes;
            _tef.OnMudaEstadoReq += OnMudaEstadoReq;
            _tef.OnMudaEstadoResp += OnMudaEstadoResp;
        }

        private void OnMudaEstadoResp(object sender, MudaEstadoRespEventArgs e)
        {
        }

        private void OnMudaEstadoReq(object sender, MudaEstadoReqEventArgs e)
        {
        }

        private void OnDepoisCancelarTransacoes(object sender, DepoisCancelarTransacoesEventArgs e)
        {
        }

        private void OnAntesCancelarTransacao(object sender, AntesCancelarTransacaoEventArgs e)
        {
            try
            {
                var estado = SessaoEcf.EcfFiscal.Estado();

                switch (estado)
                {
                    case EstadoEcfFiscal.Pagamento:
                    case EstadoEcfFiscal.Venda:
                        CancelarCupom();
                        break;

                    case EstadoEcfFiscal.Relatorio:
                        SessaoEcf.EcfFiscal.FechaRelatorio();

                        if (e.RespostaPendente.Header.Equals("CRT"))
                            CancelarCupom();
                        break;

                    case EstadoEcfFiscal.Livre:
                    case EstadoEcfFiscal.Desconhecido:
                    case EstadoEcfFiscal.NaoInicializada:
                        break;

                    default:
                        SessaoEcf.EcfFiscal.CorrigeEstadoErro();
                        break;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void OnDepoisConfirmarTransacoes(object sender, DepoisConfirmarTransacoesEventArgs e)
        {
        }

        private void OnAntesFinalizarRequisicao(object sender, AntesFinalizarRequisicaoEventArgs e)
        {
        }

        private void OnInfoEcf(object sender, InfoECFEventArgs e)
        {
            switch (e.Operacao)
            {
                case InfoECF.EstadoECF:
                    switch (SessaoEcf.EcfFiscal.Estado())
                    {
                        case EstadoEcfFiscal.Livre:
                            e.RetornoECF = RetornoECF.Livre;
                            break;

                        case EstadoEcfFiscal.Venda:
                            e.RetornoECF = RetornoECF.VendaDeItens;
                            break;

                        case EstadoEcfFiscal.Pagamento:
                            e.RetornoECF = RetornoECF.PagamentoOuSubTotal;
                            break;

                        case EstadoEcfFiscal.Relatorio:
                            e.RetornoECF = RetornoECF.RelatorioGerencial;
                            break;

                        default:
                            e.RetornoECF = RetornoECF.Outro;
                            break;
                    }
                    break;

                case InfoECF.SubTotal:
                    break;

                case InfoECF.TotalAPagar:
                    break;
            }
        }

        private void OnComandaEcfImprimeVia(object sender, ComandaECFImprimeViaEventArgs e)
        {
            try
            {
                switch (e.TipoRelatorio)
                {
                    case TipoRelatorio.Gerencial:

                        switch (SessaoSistema.ConfigTef.OperadoraTef)
                        {
                            case OperadorasTef.Cappta:
                                SessaoEcf.EcfFiscal.LinhaRelatorioGerencial(
                                    _viasGerencialTefCappta.ImprimiComprovante(e.Via));
                                break;
                            default:
                                SessaoEcf.EcfFiscal.LinhaRelatorioGerencial(e.ImagemComprovante);
                                break;
                        }

                        break;

                    case TipoRelatorio.Vinculado:
                        SessaoEcf.EcfFiscal.LinhaCupomVinculado(e.ImagemComprovante);
                        break;
                }

                e.RetornoECF = true;
            }
            catch (Exception)
            {
                e.RetornoECF = false;
            }
        }

        private void OnComandaECFAbreVinculado(object sender, ComandaECFAbreVinculadoEventArgs e)
        {
            try
            {
                SessaoEcf.EcfFiscal.AbreCupomVinculado(e.COO, e.IndiceECF, e.Valor);
                e.RetornoECF = true;
            }
            catch (Exception)
            {
                e.RetornoECF = false;
            }
        }

        private void OnComandaECFPagamento(object sender, ComandaECFPagamentoEventArgs e)
        {
            try
            {
                e.RetornoECF = true;
            }
            catch (Exception)
            {
                e.RetornoECF = false;
            }
        }

        private void OnComandaEcfSubtotaliza(object sender, ComandaECFSubtotalizaEventArgs e)
        {
            try
            {
                e.RetornoECF = true;
            }
            catch (Exception)
            {
                e.RetornoECF = false;
            }
        }

        private void OnComandaEcf(object sender, ComandaECFEventArgs e)
        {
            try
            {
                switch (e.Operacao)
                {
                    case OperacaoECF.AbreGerencial:

                        if (SessaoSistema.ConfigTef.OperadoraTef == OperadorasTef.Cappta)
                        {
                            _viasGerencialTefCappta = new ViasGerencialTefCappta(SessaoSistema.ConfigTef.ArqResp);
                            _viasGerencialTefCappta.CarregaArquivo();
                            _tef.NumVias = _viasGerencialTefCappta.QuantidadeVias;
                        }

                        SessaoEcf.EcfFiscal.AbreRelatorioGerencial();
                        break;

                    case OperacaoECF.PulaLinhas:
                        SessaoEcf.EcfFiscal.PulaLinhas(SessaoEcf.EcfFiscal.LinhasEntreCupons);
                        SessaoEcf.EcfFiscal.CortaPapel(true);
                        Thread.Sleep(200);
                        break;

                    case OperacaoECF.FechaVinculado:
                    case OperacaoECF.FechaGerencial:
                        SessaoEcf.EcfFiscal.FechaRelatorio();
                        break;
                    case OperacaoECF.FechaCupom:
                        SessaoEcf.EcfFiscal.FechaCupom(_vendaEcf.Observacao);
                        break;
                    case OperacaoECF.CancelaCupom:
                        CancelarCupom();
                        break;
                    case OperacaoECF.ImprimePagamentos:
                        break;
                }

                e.RetornoECF = true;
            }
            catch (Exception)
            {
                e.RetornoECF = false;
            }
        }

        private void OnLimpaTeclado(object sender, ExecutaAcaoEventArgs e)
        {
        }

        private void OnRestauraFocoAplicacao(object sender, ExecutaAcaoEventArgs e)
        {
        }

        private void OnBloqueiaMouseTeclado(object sender, BloqueiaMouseTecladoEventArgs e)
        {
        }

        private void OnExibeMensagem(object sender, ExibeMensagemEventArgs e)
        {
            switch (e.Operacao)
            {
                case OperacaoMensagem.OK:
                    InvocaComponenteTela(() => DialogBox.MostraInformacao(e.Mensagem));
                    e.ModalResult = ModalResult.Yes;
                    break;
                case OperacaoMensagem.RemoverMsgOperador:
                    InvocaComponenteTela(() =>
                    {
                        MenssagemCaixa = "Caixa Livre";
                        InformacaoRodape = string.Empty;
                    });
                    e.ModalResult = ModalResult.Yes;
                    break;
                case OperacaoMensagem.DestaqueVia:
                    InvocaComponenteTela(() =>
                    {
                        MenssagemCaixa = e.Mensagem;
                        InformacaoRodape = e.Mensagem;
                    });
                    e.ModalResult = ModalResult.Yes;
                    Thread.Sleep(3000);
                    break;
                case OperacaoMensagem.ExibirMsgCliente:
                case OperacaoMensagem.ExibirMsgOperador:
                    InvocaComponenteTela(() =>
                    {
                        MenssagemCaixa = e.Mensagem;
                        InformacaoRodape = e.Mensagem;
                    });
                    e.ModalResult = ModalResult.Yes;
                    break;
                case OperacaoMensagem.YesNo:
                    var ret = false;

                    InvocaComponenteTela(
                        () => { ret = DialogBox.MostraConfirmacao(e.Mensagem, MessageBoxImage.Question); });

                    e.ModalResult = ret ? ModalResult.Yes : ModalResult.No;
                    break;
            }
        }

        private void OnAguardaResposta(object sender, AguardaRespEventArgs e)
        {
        }

        private void IniciaTimerStatusServico()
        {
            _timerStatusServidor = new DispatcherTimer(DispatcherPriority.Background, _caixa.Dispatcher);
            _timerStatusServidor.Tick += DispatcherTimer_StatusServidor;
            _timerStatusServidor.Interval = new TimeSpan(0, 0, 0, 15, 0);
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
                    new ConsultarStatusDoServico().Executar("FusionPdvSincronizador");
                    StatusServidor = FontAwesomeIcon.CheckCircle;
                    CorStatus = Brushes.LawnGreen;
                }
                catch (Exception)
                {
                    StatusServidor = FontAwesomeIcon.TimesCircle;
                    CorStatus = Brushes.DarkRed;
                }
            }).Start();
        }

        private void IniciaTimerVender()
        {
            _timerVender = new DispatcherTimer(DispatcherPriority.Background, _caixa.Dispatcher);
            _timerVender.Tick += DispatcherTimer_Vender;
            _timerVender.Interval = new TimeSpan(0, 0, 0, 0, 100);
            _timerVender.Start();
        }

        private void DispatcherTimer_Vender(object sender, EventArgs e)
        {
            _timerVender.Stop();
            if (_listaEsperaVendaEcfItem.Count == 0)
            {
                _timerVender.Start();
                return;
            }

            if (_vendaEmAndamento)
            {
                _timerVender.Start();
                return;
            }

            new Thread(() =>
            {
                try
                {
                    TemErro = false;
                    UltimaException = null;

                    _vendaEmAndamento = true;
                    var vendaEcfItem = _listaEsperaVendaEcfItem[0];
                    AbreVenda(vendaEcfItem);
                    _listaEsperaVendaEcfItem.Remove(vendaEcfItem);

                    MensagemPodeFinalizar();
                }
                catch (ExceptionGtEcf)
                {
                    new AtualizarGt(SessaoEcf.EcfFiscal.GrandeTotal().ToString(CultureInfo.CurrentCulture)).Executar();
                    _vendaEmAndamento = false;
                }
                catch (Exception ex)
                {
                    _listaEsperaVendaEcfItem.Clear();
                    TemErro = true;
                    _vendaEmAndamento = false;
                    UltimaException = ex;
                    _timerVender.Stop();
                    NotificarObservadores();
                }
                finally
                {
                    _timerVender.Start();
                }
            }).Start();
        }

        private void MensagemPodeFinalizar()
        {
            if (ListaDeVendaEcfItem.Count(item => item.Cancelado == VendaItemCancelado.NaoEstaCancelado) > 0)
            {
                InformacaoRodape = "Pode finalizar";
            }
        }

        public void ReiniciarCaixa()
        {
            ClienteAdicionadoPeloCabecalho = false;
            MenssagemCaixa = "Caixa Livre";
            NomeCliente = "Sem cliente informado.";
            MensagemConsultarProduto = "Para consultar produtos(F6)";
            CodigoBarra = "";
            QuantidadeAAdicionar = 1;
            Quantidade = 0;
            PrecoUnitario = 0;
            TotalDoItem = 0;
            TotalDoCupom = 0;
            SubTotal = 0;
            _vendaEcf = new VendaEcfDt
            {
                UuidVenda = GuuidHelper.Computar("PDV" + DateTime.Now.ToString("G") + SessaoSistema.UsuarioLogado.Login)
            };
            Application.Current.Dispatcher.Invoke(() => ListaDeVendaEcfItem?.Clear());
            InformacaoRodape = "";
        }

        public void VerificaEstadoEcf()
        {
            try
            {
                var estadoEcf = new EstadoEcf();
                estadoEcf.ProcessaEstadoEcf();
                _vendaEcf = estadoEcf.VendaEmAndamento;
                CarregaVendaSeExistirAberta();
            }
            catch (ACBrException ex)
            {
                throw new ACBrException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        private void CarregaVendaSeExistirAberta()
        {
            if (_vendaEcf.VendaEcfItens.Count == 0 && _vendaEcf.DocumentoCliente.Trim() == "")
            {
                _vendaEcf = new VendaEcfDt
                {
                    UuidVenda = GuuidHelper.Computar("PDV" + DateTime.Now.ToString("G") + SessaoSistema.UsuarioLogado.Login)
                };
                return;
            }

            if (!string.IsNullOrEmpty(_vendaEcf.DocumentoCliente))
            {
                AbreVenda(new ClienteCupom
                {
                    Cliente = new ClienteDt
                    {
                        Nome = _vendaEcf.NomeCliente,
                        Cnpj = _vendaEcf.DocumentoCliente.Length == 14 ? _vendaEcf.DocumentoCliente : "",
                        Cpf = _vendaEcf.DocumentoCliente.Length == 11 ? _vendaEcf.DocumentoCliente : "",
                        Endereco = _vendaEcf.EnderecoCliente
                    },
                    Observacao = _vendaEcf.Observacao
                });
            }

            _vendaEcf.VendaEcfItens.OrderBy(x => x.NumeroItem).ForEach(AbreVenda);

            _vendaEcf.VendaEcfItens.Where(i => i.Cancelado.Equals(VendaItemCancelado.SimCancelado))
                .OrderBy(x => x.NumeroItem)
                .ForEach(
                    itemCancelado => { CancelarItem(itemCancelado.NumeroItem.ToString(), true); });

            MensagemPodeFinalizar();
        }

        public decimal QuantidadeAAdicionarEValido(string quantidade)
        {
            char[] delimitadores = {'*'};
            var partes = quantidade.Split(delimitadores);

            try
            {
                partes[1] = partes[1].Replace(".", ",");
                var valor = decimal.Parse(partes[1]);


                if (valor == 0 || valor < 0)
                {
                    throw new InvalidOperationException("Valor da quantidade tem que ser maior que 0");
                }

                if (valor.ToString(CultureInfo.CurrentCulture).Length > 7)
                {
                    throw new InvalidOperationException("Valor máximo para quantidade é 999999");
                }

                return valor;
            }
            catch (FormatException ex)
            {
                throw new InvalidOperationException("Número digitado está inválido.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        public ProdutoDt BuscarProdutoPorCodigoBarra(string codigoBarra)
        {
            codigoBarra = codigoBarra?.Trim();

            try
            {
                using (var sessao = GerenciaSessao.ObterSessao(SessaoPdv.FabricaSessaoPdv).AbrirSessao())
                {
                    var repositorio = new ProdutoRepositorio(sessao);
                    var produto = BuscaProdutoPorCodigoBalanca(repositorio, codigoBarra);

                    if (produto != null)
                    {
                        PegaPrecoOuPesoDoProduto(codigoBarra, produto);
                        return produto;
                    }

                    produto = repositorio.BuscarPorCodigoBarraOuCodigo(codigoBarra);

                    if (produto != null)
                    {
                        return produto;
                    }
                }

                throw new InvalidOperationException("Não encontrei produto para esse código.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new ArgumentOutOfRangeException(ex.Message, ex);
            }
            catch (FormatException ex)
            {
                throw new FormatException(ex.Message, ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao tentar buscar um produto pelo código de barras", ex);
            }
        }

        private void PegaPrecoOuPesoDoProduto(string codigoBarra, ProdutoDt produto)
        {
            var configuracaoBalanca = SessaoSistema.ConfiguracoesBalanca;

            var tamanhoDigitoVerificador = configuracaoBalanca.DigitoVerificador.ToString().Length;

            var inicio = configuracaoBalanca.TamanhoCodigo + tamanhoDigitoVerificador;
            var casasDecimais = configuracaoBalanca.CasasDecimais;

            if (SessaoSistema.ConfiguracoesBalanca.ModoDeOperacao == ModoDeOperacao.Preco)
            {
                var precoTemp = codigoBarra?.Substring(inicio,
                    Math.Abs(inicio - codigoBarra.Length));

                if (string.IsNullOrEmpty(precoTemp))
                {
                    throw new ArgumentException("O preço no código de barras está incorreto");
                }

                precoTemp = precoTemp.Remove(precoTemp.Length - 1, 1);
                precoTemp = precoTemp.Insert(precoTemp.Length - casasDecimais, ",");

                var preco = decimal.Parse(precoTemp);

                var quantidade = Math.Abs(preco/produto.PrecoVenda);

                var quantidadeTruncadaOuArredondada = new TruncaArredonda(quantidade, 3).ExecutaComQuantidadeDecimal();

                QuantidadeAAdicionar = quantidade <= 0 ? 1 : quantidadeTruncadaOuArredondada;
            }

            if (SessaoSistema.ConfiguracoesBalanca.ModoDeOperacao != ModoDeOperacao.Peso) return;

            var pesoTemp = codigoBarra?.Substring(inicio,
                Math.Abs(inicio - codigoBarra.Length));

            if (string.IsNullOrEmpty(pesoTemp))
            {
                throw new ArgumentException("O peso no código de barras está incorreto");
            }

            pesoTemp = pesoTemp.Remove(pesoTemp.Length - 1, 1);
            pesoTemp = pesoTemp.Insert(pesoTemp.Length - casasDecimais, ",");

            var peso = decimal.Parse(pesoTemp);

            QuantidadeAAdicionar = peso;
        }

        private ProdutoDt BuscaProdutoPorCodigoBalanca(ProdutoRepositorio repositorio, string codigoBarra)
        {
            if (SessaoSistema.ConfiguracoesBalanca == null) return null;
            if (!SessaoSistema.ConfiguracoesBalanca.Ativo) return null;

            var tamanhoCodigo = SessaoSistema.ConfiguracoesBalanca.TamanhoCodigo;

            if (codigoBarra.Length < 8)
            {
                return null;
            }

            try
            {
                var digitoVerificador = byte.Parse(codigoBarra.Substring(0, 1));

                // ReSharper disable once ConvertIfStatementToReturnStatement, deixando assim porque melhora na hora de debugar
                if (SessaoSistema.ConfiguracoesBalanca.DigitoVerificador == digitoVerificador)
                {
                    return repositorio.BuscaPorCodigoBalanca(codigoBarra.Substring(1, tamanhoCodigo));
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new ArgumentOutOfRangeException(
                    @"Código de balança está incorreto pois o mesmo tem que ter o tamanho exato de " + tamanhoCodigo,
                    ex);
            }

            return null;
        }

        public void BuscarOuAdicionarQuantidade(ProdutoDt produto)
        {
            try
            {
                if (MensagemConsultarProduto.Equals("Cancelamento de item"))
                {
                    CancelarItem(CodigoBarra);
                    MensagemConsultarProduto = "Para consultar produtos(F6)";
                }
                else if (!string.IsNullOrEmpty(CodigoBarra) && CodigoBarra.Contains("*"))
                {
                    QuantidadeAAdicionar = QuantidadeAAdicionarEValido(_codigoBarra);
                }
                else
                {
                    if (produto == null)
                    {
                        throw new InvalidOperationException("Produto não encontrado.");
                    }

                    ChecarEstoqueNegativo(produto, QuantidadeAAdicionar);
                    ValidarDadosFiscais(produto);
                    ValidarStatusDoProduto(produto);
                    PodeFracionar(produto);
                    AdicionaProdutoListaEspera(produto);
                }

                CodigoBarra = "";
            }
            catch (ACBrException ex)
            {
                throw new ACBrException(ex.Message);
            }
            catch (ExceptionGtEcf ex)
            {
                throw new ExceptionGtEcf(ex.Message, ex);
            }
            catch (NaoExisteCupomAbertoException ex)
            {
                throw new NaoExisteCupomAbertoException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        private void ChecarEstoqueNegativo(ProdutoDt produto, decimal quantidade)
        {
            using (var sessao = GerenciaSessao.ObterSessao(SessaoPdv.FabricaSessaoPdv).AbrirSessao())
            {
                BoqueioEstoqueHelper.ChecaEstoqueNegativoPdv(produto, quantidade, sessao);
            }
        }

        private void AdicionaProdutoListaEspera(ProdutoDt produto)
        {
            if (!_timerVender.IsEnabled)
            {
                _timerVender.Start();
            }

            var vendaEcfItem = new VendaEcfItemDt
            {
                VendaEcfDt = _vendaEcf,
                ProdutoDt = produto,
                NomeProduto = produto.Nome,
                SituacaoTributariaIcms = produto.SituacaoTributariaIcms,
                Icms = produto.AliquotaIcmsPaf,
                Quantidade = new TruncaArredonda(QuantidadeAAdicionar, 3).ExecutaComQuantidadeDecimal(),
                IcmsEcf = produto.IcmsEcf,
                CodigoBarra = produto.Id.ToString(),
                PrecoUnitario = new TruncaArredonda(produto.PrecoVenda).Executa(),
                SiglaUnidadeProduto = produto.SiglaUnidade,
                Total =
                    new TruncaArredonda(new TruncaArredonda(QuantidadeAAdicionar, 3).ExecutaComQuantidadeDecimal()*
                                        produto.PrecoVenda).Executa()
            };

            produto.ProdutosAlias?.ForEach(alias =>
            {
                if (alias.IsCodigoBarras)
                    vendaEcfItem.CodigoBarra = alias.Alias;
            });

            _listaEsperaVendaEcfItem.Add(vendaEcfItem);
            QuantidadeAAdicionar = 1;
            ProdutoPorKilo = null;
        }

        private void ValidarStatusDoProduto(ProdutoDt produto)
        {
            if (produto?.Ativo == IntBinario.Sim)
                return;

            throw new InvalidOperationException("Produto não está ativo. Solicite a ativação do mesmo!");
        }

        private void ValidarDadosFiscais(ProdutoDt produto)
        {
            if (produto == null)
                return;

            if (string.IsNullOrEmpty(produto.Nome))
                throw new InvalidOperationException("Nome pdv do produto está em branco");

            if (produto.PrecoVenda <= 0)
                throw new InvalidOperationException("Preço de venda do produto está zerado");

            if (produto.AliquotaIcmsPaf != 0 &&
                (produto.SituacaoTributariaIcms == "41" ||
                 produto.SituacaoTributariaIcms == "40" ||
                 produto.SituacaoTributariaIcms == "60"))
                throw new InvalidOperationException("Aliquto e Situação tributária do produto não estão correpondendo");

            if (produto.AliquotaIcmsPaf == 0 &&
                (produto.SituacaoTributariaIcms == "00" ||
                 produto.SituacaoTributariaIcms == "10" ||
                 produto.SituacaoTributariaIcms == "20" ||
                 produto.SituacaoTributariaIcms == "70"))
                throw new InvalidOperationException(
                    "Aliquota e Situtação tributária do produto não estão correspondendo");

            if (produto.IcmsEcf == "FF" || produto.IcmsEcf == "II" || produto.IcmsEcf == "NN") return;

            var temAAliquota = false;

            SessaoSistema.AliquotasDoEcf.ForEach(a =>
            {
                var aliquota = a.Valor + a.Tipo;
                if (aliquota.Equals(produto.IcmsEcf))
                {
                    temAAliquota = true;
                }
            });

            if (temAAliquota == false)
            {
                throw new InvalidOperationException(
                    "Aliquota não existe na ECF\nPara solucionar o problema você deve trocar a aliquota do produto ou " +
                    "se você deseja cadastrar outra aliquota, antes verificar com o seu contador");
            }
        }

        private void PodeFracionar(ProdutoDt produto)
        {
            if ((QuantidadeAAdicionar%1) == 0)
                return;
            if (produto.PodeFracionar == IntBinario.Sim)
                return;
            throw new InvalidOperationException("Unidade do produto não permite fracionar.");
        }

        private void CancelarItem(string codigoBarra, bool recuperacao = false)
        {
            VerificaSeExisteCupomAberto();

            var vendaItem =
                (VendaEcfItemDt)
                    ListaDeVendaEcfItem.Where(item => item.NumeroItem.Equals(int.Parse(codigoBarra))).FirstOrNull();

            if (vendaItem == null)
                throw new InvalidOperationException("Item não encontrado!");

            if (!recuperacao)
                if (vendaItem.Cancelado.Equals(VendaItemCancelado.SimCancelado))
                    throw new InvalidOperationException("Item já foi cancelado!");


            var sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao();
            var transacao = sessao.BeginTransaction();

            try
            {
                vendaItem.Cancelado = VendaItemCancelado.SimCancelado;
                var repositorioItem = new VendaEcfItemRepositorio(sessao);
                repositorioItem.Salvar(vendaItem);

                var estoqueServico = EstoqueServicoPdvFactory.CriarEstoqueServico(sessao);
                var estoqueModel = CriarEstoqueModelCancelamento(vendaItem);
                estoqueServico.Acrescentar(estoqueModel);


                SessaoEcf.EcfFiscal.CancelarItem(int.Parse(codigoBarra));
                transacao.Commit();
            }
            catch (ACBrException ex)
            {
                transacao?.Rollback();
                vendaItem.Cancelado = VendaItemCancelado.NaoEstaCancelado;
                throw new ACBrException(ex.Message);
            }
            catch (Exception ex)
            {
                transacao?.Rollback();
                vendaItem.Cancelado = VendaItemCancelado.NaoEstaCancelado;
                throw new InvalidOperationException("Não foi possivel cancelar o item", ex);
            }
            finally
            {
                sessao.Close();
                _caixa.Dispatcher.Invoke(() => { CollectionViewSource.GetDefaultView(ListaDeVendaEcfItem).Refresh(); });

                AtualizaCalculosCupomFinal();
            }
        }

        private static EstoqueModel CriarEstoqueModelCancelamento(VendaEcfItemDt vendaItem)
        {
            return new EstoqueModel
            {
                OrigemEvento = OrigemEventoEstoque.CancelamentoItemPdv,
                Produto = vendaItem.ProdutoDt,
                Quantidade = vendaItem.Quantidade,
                Usuario = SessaoSistema.UsuarioLogado
            };
        }

        public void CancelarCupom()
        {
            var sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao();
            ITransaction transacao = null;

            try
            {
                var vendaRepositorio = new VendaEcfRepositorio(sessao);
                var venda = vendaRepositorio.ObterUltimaVendaParaCancelamento();

                if (venda.Status.Equals(VendaStatus.Cancelada) && venda.Cancelado.Equals(IntBinario.Sim))
                {
                    throw new CancelarVendaException("Última Venda já foi cancelada");
                }

                venda.Status = VendaStatus.Cancelada;
                venda.Cancelado = IntBinario.Sim;

                transacao = sessao.BeginTransaction();

                var estoqueServico = EstoqueServicoPdvFactory.CriarEstoqueServico(sessao);
                var repositorioItem = new VendaEcfItemRepositorio(sessao);

                venda.VendaEcfItens.ForEach(item =>
                {
                    if (item.Cancelado == VendaItemCancelado.SimCancelado)
                        return;

                    item.Cancelado = VendaItemCancelado.SimCancelado;
                    repositorioItem.Salvar(item);

                    var estoqueModel = CriarEstoqueModelCancelamento(item);
                    estoqueServico.Acrescentar(estoqueModel);
                });

                vendaRepositorio.Salvar(venda);
                SessaoEcf.EcfFiscal.CancelarCupom();

                ReiniciarCaixa();
                transacao.Commit();
            }
            catch (ACBrException ex)
            {
                transacao?.Rollback();
                throw new ACBrException(ex.Message);
            }
            catch (CancelarVendaException ex)
            {
                throw new CancelarVendaException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                transacao?.Rollback();
                throw new InvalidOperationException("Impossível cancelamento do cupom", ex);
            }
            finally
            {
                sessao.Close();
            }
        }

        private void VerificaSeExisteCupomAberto()
        {
            var estadoEcf =
                SessaoEcf.EcfFiscal.Estado();

            if (estadoEcf != EstadoEcfFiscal.Venda &&
                estadoEcf != EstadoEcfFiscal.Pagamento)
            {
                throw new NaoExisteCupomAbertoException("Não existe cupom aberto.");
            }
        }

        public void AbreVenda(ClienteCupom clienteCupom)
        {
            InicializaVenda(clienteCupom);
        }

        public void AbreVenda(VendaEcfItemDt ecfItemDt)
        {
            if (!string.IsNullOrEmpty(_vendaEcf.NomeCliente.Trim())) NomeCliente = _vendaEcf.NomeCliente;

            InicializaVenda();

            VendeItem(ecfItemDt);
            _vendaEmAndamento = false;
        }

        public void AbreVenda(ProdutoDt produto = null)
        {
            if (produto != null)
            {
                ValidarDadosFiscais(produto);
                ValidarStatusDoProduto(produto);
                PodeFracionar(produto);
            }
            AdicionaProdutoListaEspera(produto);
        }

        private void InicializaVenda(ClienteCupom clienteCupom)
        {
            NomeCliente = clienteCupom.Cliente.Nome.Trim();
            var documento = string.IsNullOrEmpty(clienteCupom.Cliente?.Cpf?.Trim())
                ? clienteCupom.Cliente.Cnpj
                : clienteCupom.Cliente.Cpf;
            var observacao = clienteCupom.Observacao ?? "";
            var idCliente = clienteCupom.Cliente.Id;

            if (idCliente != 0)
            {
                _vendaEcf.ClienteDt = clienteCupom.Cliente;
            }

            _vendaEcf.NomeCliente = NomeCliente;
            _vendaEcf.DocumentoCliente = documento.Trim();
            _vendaEcf.EnderecoCliente = clienteCupom.Cliente.Endereco.Trim();
            _vendaEcf.Observacao = observacao.Trim();

            try
            {
                new EcfAbrirCupom(_vendaEcf).IniciarVenda();
            }
            catch (ExceptionGtEcf ex)
            {
                throw new ExceptionGtEcf(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Falha ao abrir o cupom: \n" + ex.Message, ex);
            }
        }

        private void InicializaVenda()
        {
            try
            {
                InformacaoRodape = "";
                if (ListaDeVendaEcfItem.Count == 0)
                {
                    InformacaoRodape = "Abrindo Venda";
                }
                new EcfAbrirCupom(_vendaEcf).IniciarVenda();
            }
            catch (ExceptionGtEcf ex)
            {
                throw new ExceptionGtEcf(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Falha ao abrir o cupom: \n" + ex.Message, ex);
            }
        }

        private void VendeItem(VendaEcfItemDt ecfItemDt)
        {
            try
            {
                InformacaoRodape = "Vendendo item";
                new EcfVender(ecfItemDt, ListaDeVendaEcfItem.Count).VendeItem();

                MenssagemCaixa = ecfItemDt.NomeProduto;

                AtualizaListaVendaEcfItens(ecfItemDt);
                AtualizaTotais(ecfItemDt);
                AtualizaVenda(_vendaEcf);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        private void AtualizaVenda(VendaEcfDt vendaEcf)
        {
            using (var sessao = GerenciaSessao.ObterSessao(SessaoPdv.FabricaDois).AbrirSessao())
            {
                vendaEcf.AlteradoEm = DateTime.Now;
                new VendaEcfRepositorio(sessao).Salvar(vendaEcf);
            }
        }

        private void AtualizaTotais(VendaEcfItemDt vendaEcfItem)
        {
            CalculaTotaisVenda();
            Quantidade = vendaEcfItem.Quantidade;
            PrecoUnitario = vendaEcfItem.PrecoUnitario;
            TotalDoItem = vendaEcfItem.Total;

            AtualizaCalculosCupomFinal();
        }

        private void AtualizaCalculosCupomFinal()
        {
            CalculaTotaisVenda();
            var total = _vendaEcf.TotalFinal;
            TotalDoCupom = total;
            SubTotal = total;

            AtualizaGt();
        }

        private void AtualizaGt()
        {
            new AtualizarGt(SessaoEcf.EcfFiscal.GrandeTotal().ToString(CultureInfo.CurrentCulture)).Executar();
        }

        private void AtualizaListaVendaEcfItens(VendaEcfItemDt vendaEcfItem)
        {
            _caixa.Dispatcher.Invoke(() => { ListaDeVendaEcfItem.Insert(0, vendaEcfItem); });
        }

        public VendaEcfDt ObterVenda()
        {
            _vendaEcf.VendaEcfItens = _listaDeVendaEcfItem;
            return _vendaEcf;
        }

        public bool ExisteVendaEmAndamento()
        {
            return _vendaEcf.Id != 0;
        }

        public void VerificacaoInicial()
        {
            try
            {
                new VerificacaoInicial().Executar();
            }
            catch (ExceptionSerieEcf ex)
            {
                throw new ExceptionSerieEcf(ex.Message, ex);
            }
            catch (ExceptionCarregarFormaPagamento ex)
            {
                throw new ExceptionCarregarFormaPagamento(ex.Message, ex);
            }
            catch (ExceptionGtEcf ex)
            {
                throw new ExceptionGtEcf(ex.Message, ex);
            }
            catch (ExceptionDataInvalidaEcf ex)
            {
                throw new ExceptionDataInvalidaEcf(ex.Message, ex);
            }
            catch (ExceptionExisteAliquota ex)
            {
                throw new ExceptionExisteAliquota(ex.Message, ex);
            }
            catch (JsonReaderException)
            {
                RestaurarArquivoAuxiliar();
                throw;
            }
            catch (ArquivoAuxiliarInvalidoException)
            {
                RestaurarArquivoAuxiliar();
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private static void RestaurarArquivoAuxiliar()
        {
            try
            {
                new FazerBackupArquivoAuxiliar("2").RestaurarArquivoAuxiliar();
            }
            catch (Exception)
            {
                try
                {
                    new FazerBackupArquivoAuxiliar("1").RestaurarArquivoAuxiliar();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(ex.Message, ex);
                }
            }
        }

        public void AdicionarClienteNoCupom(ClienteCupom clienteCupom)
        {
            var nome = clienteCupom.Cliente.Nome.Trim();
            var cpfOuCnpj = string.IsNullOrEmpty(clienteCupom.Cliente?.Cpf?.Trim())
                ? clienteCupom.Cliente.Cnpj
                : clienteCupom.Cliente.Cpf;
            var endereco = clienteCupom.Cliente.Endereco.Trim();
            string observacao = null;


            if (!string.IsNullOrEmpty(clienteCupom.Observacao))
            {
                observacao = clienteCupom.Observacao.Trim();
            }

            _vendaEcf.NomeCliente = nome;
            _vendaEcf.DocumentoCliente = cpfOuCnpj.Trim();
            _vendaEcf.EnderecoCliente = endereco;
            _vendaEcf.Observacao = observacao ?? "";

            NomeCliente = nome;

            var sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao();
            var transacao = sessao.BeginTransaction();

            try
            {
                SessaoEcf.EcfFiscal.IdentificaConsumidor(cpfOuCnpj, nome, endereco);
                new VendaEcfRepositorio(sessao).Salvar(_vendaEcf);

                transacao.Commit();
            }
            catch (Exception ex)
            {
                transacao?.Rollback();
                throw new RepositorioExeption("Erro ao salvar a venda", ex);
            }
            finally
            {
                sessao.Close();
            }
        }

        public void CalculaTotaisVenda()
        {
            CalculaTotalFinalECupom();
            CalculaTotalCancelado();
            CalculaTotalProdutos();
            CalculaTotalBaseIcms();
        }

        private void CalculaTotalFinalECupom()
        {
            _vendaEcf.TotalFinal =
                new TruncaArredonda(ListaDeVendaEcfItem.Where(
                    item => item.Cancelado.Equals(VendaItemCancelado.NaoEstaCancelado))
                    .ToList().Sum(itemS => itemS.Total)).Executa();

            _vendaEcf.TotalCupom = _vendaEcf.TotalFinal;
        }

        private void CalculaTotalBaseIcms()
        {
            var lista =
                ListaDeVendaEcfItem.Where(item => item.Cancelado.Equals(VendaItemCancelado.NaoEstaCancelado))
                    .ToList();

            var totalBaseIcms = 0.0m;

            lista.ForEach(item => { totalBaseIcms += ((item.Total*item.ProdutoDt.AliquotaIcmsPaf)/100); });

            _vendaEcf.TotalBaseIcms = new TruncaArredonda(totalBaseIcms).Executa();
        }

        private void CalculaTotalProdutos()
        {
            _vendaEcf.TotalProdutos =
                new TruncaArredonda(ListaDeVendaEcfItem.Where(
                    item => item.Cancelado.Equals(VendaItemCancelado.NaoEstaCancelado))
                    .ToList()
                    .Sum(itemValido => itemValido.Total)).Executa();
        }

        private void CalculaTotalCancelado()
        {
            _vendaEcf.TotalCancelado =
                new TruncaArredonda(
                    ListaDeVendaEcfItem.Where(item => item.Cancelado.Equals(VendaItemCancelado.SimCancelado))
                        .ToList()
                        .Sum(itemCancelado => itemCancelado.Total)).Executa();
        }

        public void ValidarFormaPagamento()
        {
            new ExisteFormaDePagamentoMapeada().Executar();
        }

        public void CalculaIbpt()
        {
            var impostoFederal = 0.0M;
            var impostoMunicipal = 0.0M;
            var impostoEstadual = 0.0M;
            var fonte = string.Empty;

            ListaDeVendaEcfItem.Where(item => item.Cancelado.Equals(VendaItemCancelado.NaoEstaCancelado))
                .ForEach(item =>
                {
                    impostoFederal += ((item.Total*item.ProdutoDt.TributacaoNacional)/100);
                    impostoEstadual += ((item.Total*item.ProdutoDt.TributacaoEstadual)/100);
                    fonte = $"IBPT - chave {item.ProdutoDt.ChaveIbpt}";
                });

            SessaoEcf.EcfFiscal.Ibtp(fonte, impostoFederal, impostoMunicipal, impostoEstadual);
        }

        public void ValidaQuantidadeDeItems()
        {
            var items =
                _listaDeVendaEcfItem.Where(item => item.Cancelado.Equals(VendaItemCancelado.NaoEstaCancelado)).ToList();

            if (items.Count == 0)
            {
                throw new QuantidadeItemZeroException("Quantidade de items tem que ser maior que zero.");
            }
        }

        public void ValidarAdiconaCliente()
        {
            if (!string.IsNullOrEmpty(_vendaEcf.NomeCliente) || !string.IsNullOrEmpty(_vendaEcf.DocumentoCliente)
                || !string.IsNullOrEmpty(_vendaEcf.EnderecoCliente))
                throw new InvalidOperationException("Cliente já foi adicionado.");
        }

        public bool ListaEsperaProdutoConcluida()
        {
            return _listaEsperaVendaEcfItem.Count == 0;
        }

        public void Inscrever(Caixa observer)
        {
            _observer = observer;
        }

        public void NotificarObservadores()
        {
            _observer?.Notificacao(this);
        }

        public void RemoveEventosTef()
        {
            _tef.OnAguardaResp -= OnAguardaResposta;
            _tef.OnExibeMensagem -= OnExibeMensagem;
            _tef.OnBloqueiaMouseTeclado -= OnBloqueiaMouseTeclado;
            _tef.OnRestauraFocoAplicacao -= OnRestauraFocoAplicacao;
            _tef.OnLimpaTeclado -= OnLimpaTeclado;
            _tef.OnComandaECF -= OnComandaEcf;
            _tef.OnComandaECFSubtotaliza -= OnComandaEcfSubtotaliza;
            _tef.OnComandaECFPagamento -= OnComandaECFPagamento;
            _tef.OnComandaECFAbreVinculado -= OnComandaECFAbreVinculado;
            _tef.OnComandaECFImprimeVia -= OnComandaEcfImprimeVia;
            _tef.OnInfoECF -= OnInfoEcf;
            _tef.OnAntesFinalizarRequisicao -= OnAntesFinalizarRequisicao;
            _tef.OnDepoisConfirmarTransacoes -= OnDepoisConfirmarTransacoes;
            _tef.OnAntesCancelarTransacao -= OnAntesCancelarTransacao;
            _tef.OnDepoisCancelarTransacoes -= OnDepoisCancelarTransacoes;
            _tef.OnMudaEstadoReq -= OnMudaEstadoReq;
            _tef.OnMudaEstadoResp -= OnMudaEstadoResp;
        }

        public async void AdministracaoTef()
        {
            if (ValidacoesAntesIniciarTefAdm()) return;

            UsandoTef = true;
            try
            {
                await Task.Run(() => _tef.ADM(TefTipo.TefDial));
                _tef.NumVias = 2;
            }
            catch (ACBrException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
                FechaRelatorioSeOuverPendente();
                UsandoTef = false;
                return;
            }

            UsandoTef = false;
        }

        private bool ValidacoesAntesIniciarTefAdm()
        {
            if (VerificaSeTefEstaAtivo())
            {
                DialogBox.MostraInformacao("Tef não está ativo, se deseja usar o tef, porfavor ativar o mesmo.");
                return true;
            }
            if (!SessaoEcf.EcfFiscal.Ativo)
            {
                DialogBox.MostraInformacao("Atenção ECF não está ativa");
                return true;
            }
            return false;
        }

        private static void FechaRelatorioSeOuverPendente()
        {
            try
            {
                if (SessaoEcf.EcfFiscal.Estado() == EstadoEcfFiscal.Relatorio)
                {
                    SessaoEcf.EcfFiscal.FechaRelatorio();
                }
            }
            catch (ACBrException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void InvocaComponenteTela(Action action)
        {
            Application.Current.Dispatcher.Invoke(action.Invoke);
        }

        public void AbrirGaveta()
        {
            SessaoEcf.EcfFiscal.AbreGaveta();
        }

        public void AtivarReducaoZ()
        {
            ReducaoZEmAndamento = true;
        }

        public void DesativarReducaoZ()
        {
            ReducaoZEmAndamento = false;
        }
    }
}