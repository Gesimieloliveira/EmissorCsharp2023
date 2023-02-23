using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using ACBrFramework;
using ACBrFramework.TEFD;
using FusionCore.FusionPdv.Flags;
using FusionCore.FusionPdv.Servico.Estoque;
using FusionCore.FusionPdv.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionCore.Repositorio.Legacy.Flags;
using FusionLibrary.Helper.Criptografia;
using FusionLibrary.VisaoModel;
using FusionPdv.Acbr;
using FusionPdv.Ecf;
using FusionPdv.Modelos;
using FusionPdv.Modelos.FormaPagamento;
using FusionPdv.Modelos.Pagamento;
using FusionPdv.Servicos.ArquivoAuxiliar;
using FusionPdv.Visao.Principal;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate;
using NHibernate.Util;

namespace FusionPdv.Visao.Pagamento
{
    public class FalhaTransmissao : EventArgs
    {
        public FalhaTransmissao(Exception exception, string mensagem)
        {
            Exception = exception;
            Mensagem = mensagem;
        }

        public Exception Exception { get; set; }
        public string Mensagem { get; set; }
    }

    public sealed class EfetuaPagamentoModel : ViewModel
    {

        private ACBrTEFD _tef;
        private readonly VendaEcfDt _vendaEcf;
        private IList<IPagamento> _formaPagamentos;
        private IList<Informacao> _informacoes;
        private string _menssagemTop;
        private string _pagamento;
        private bool _pagamentoConcluido;
        private decimal _saldo;
        private decimal _subtotal;
        private decimal _totalCupom;
        private decimal _troco;
        private decimal _descontoOuAcrescimo;
        private bool _botaoFinalizarVisivel;
        private bool _layoutMensagem;
        private bool _layoutEfetuarPagamento;
        private string _mensagemOperacao;
        private bool _visivel;
        public IPagamento FormaPagamento { private get; set; }
        public bool UsandoTef { get; set; }

        public string MensagemOperacao
        {
            get { return _mensagemOperacao; }
            set
            {
                if (value == _mensagemOperacao) return;
                _mensagemOperacao = value;
                PropriedadeAlterada();
            }
        }

        public bool BotaoFinalizarVisivel
        {
            get { return _botaoFinalizarVisivel; }
            set
            {
                if (value == _botaoFinalizarVisivel) return;
                _botaoFinalizarVisivel = value;
                PropriedadeAlterada();
            }
        }

        public decimal TotalCupom
        {
            get { return _totalCupom; }
            set
            {
                _totalCupom = value;
                PropriedadeAlterada();
            }
        }

        public IList<IPagamento> FormaPagamentos
        {
            get { return _formaPagamentos; }
            set
            {
                _formaPagamentos = value;
                PropriedadeAlterada();
            }
        }

        public IList<IPagamento> FormaPagamentosTemp { get; set; }

        public string Pagamento
        {
            get { return _pagamento; }
            set
            {
                _pagamento = value;
                PropriedadeAlterada();
            }
        }

        public decimal Subtotal
        {
            get { return _subtotal; }
            set
            {
                _subtotal = value;
                PropriedadeAlterada();
            }
        }

        public decimal Saldo
        {
            get { return _saldo; }
            set
            {
                _saldo = value;
                PropriedadeAlterada();
            }
        }

        public string MenssagemTop
        {
            get { return _menssagemTop; }
            set
            {
                _menssagemTop = value;
                PropriedadeAlterada();
            }
        }

        public IList<Informacao> Informacoes
        {
            get { return _informacoes; }
            set
            {
                _informacoes = value;
                PropriedadeAlterada();
            }
        }

        public bool TerminouDeLancarPagamento { get; set; }

        public bool LayoutMensagem
        {
            get { return _layoutMensagem; }
            set
            {
                if (value == _layoutMensagem) return;
                _layoutMensagem = value;
                PropriedadeAlterada();
            }
        }

        public bool LayoutEfetuarPagamento
        {
            get { return _layoutEfetuarPagamento; }
            set
            {
                if (value == _layoutEfetuarPagamento) return;
                _layoutEfetuarPagamento = value;
                PropriedadeAlterada();
            }
        }

        public bool GerouFinanceiro { get; set; }

        public bool PossuiFinanceiro
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public EfetuaPagamentoModel(VendaEcfDt obterVenda)
        {
            InicializaFormaPagamentoLocal();

            _vendaEcf = CriaVendaNova(obterVenda);
            _informacoes = new ObservableCollection<Informacao>();
            FormaPagamentosTemp = new List<IPagamento>();
            AtualizarTotais();

            InicializaTef();
            AtivaLayoutPadrao();
            AtualizaModel();
        }

        public event EventHandler DesativaTudo;
        public event EventHandler AtivaTudo;
        public event EventHandler<FalhaTransmissao> FalhaTransmissaoDocumentoReceber;

        private void AtualizaModel()
        {
            PossuiFinanceiro = SessaoSistema.AcessoConcedido.PossuiFusionGestor;
            MensagemOperacao = "Finalizando Cupom Fiscal";
        }

        private void AtivaLayoutPadrao()
        {
            LayoutEfetuarPagamento = true;
            LayoutMensagem = false;
        }

        private void InicializaTef()
        {
            _tef = AcbrFactory.ObterAcbrTefd();
            _tef.OnAguardaResp += OnAguardaResposta;
            _tef.OnExibeMensagem += OnExibeMensagem;
            _tef.OnBloqueiaMouseTeclado += OnBloqueiaMouseTeclado;
            _tef.OnRestauraFocoAplicacao += OnRestauraFocoAplicacao;
            _tef.OnLimpaTeclado += OnLimpaTeclado;
            _tef.OnComandaECF += OnComandaEcf;
            _tef.OnComandaECFSubtotaliza += OnComandaEcfSubtotaliza;
            _tef.OnComandaECFPagamento += OnComandaEcfPagamento;
            _tef.OnComandaECFAbreVinculado += OnComandaEcfAbreVinculado;
            _tef.OnComandaECFImprimeVia += OnComandaEcfImprimeVia;
            _tef.OnInfoECF += OnInfoEcf;
            _tef.OnAntesFinalizarRequisicao += OnAntesFinalizarRequisicao;
            _tef.OnDepoisConfirmarTransacoes += OnDepoisConfirmarTransacoes;
            _tef.OnAntesCancelarTransacao += OnAntesCancelarTransacao;
            _tef.OnDepoisCancelarTransacoes += OnDepoisCancelarTransacoes;
            _tef.OnMudaEstadoReq += OnMudaEstadoReq;
            _tef.OnMudaEstadoResp += OnMudaEstadoResp;
        }

        public event EventHandler FecharTela;

        private void OnMudaEstadoResp(object sender, MudaEstadoRespEventArgs e)
        {
        }

        private void OnMudaEstadoReq(object sender, MudaEstadoReqEventArgs e)
        {
        }

        private void OnDepoisCancelarTransacoes(object sender, DepoisCancelarTransacoesEventArgs e)
        {
            if (e.RespostasPendentes.Count != 0) return;

            _pagamentoConcluido = true;
            OnFecharTela();
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
                        CancelarVenda();
                        break;

                    case EstadoEcfFiscal.Relatorio:
                        SessaoEcf.EcfFiscal.FechaRelatorio();

                        if (e.RespostaPendente.Header.Equals("CRT"))
                        {
                            CancelarVenda();
                        }
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
                OnAtivaTudo();
                AtivaLayoutPadrao();
            }
        }

        private void OnDepoisConfirmarTransacoes(object sender, DepoisConfirmarTransacoesEventArgs e)
        {
            var pagamentosCartao = _vendaEcf.VendaEcfPagamentos.Where(p => p.FormaPagamentoEcfDt.Id == 2).ToList();
            var posicaoPagamento = 0;

            e.RespostasPendentes.ForEach(resp =>
            {
                var codigoAutorizacao = resp.CodigoAutorizacaoTransacao;
                var rede = resp.Rede ?? string.Empty;
                var nsu = resp.NSU ?? string.Empty;
                var tipoTransacao = resp.TipoTransacao;
                var tipoParcelamento = resp.TipoParcelamento;
                var quantidadeParcelas = resp.QtdParcelas;
                var dataEHoraTransacaoComprovante = resp.DataHoraTransacaoComprovante;
                var nomeAdministradora = resp.NomeAdministradora ?? string.Empty;
                var credito = resp.Credito;
                var debito = resp.Debito;
                var desconto = resp.Desconto;
                var saque = resp.Saque;
                var total = resp.ValorTotal;
                var bandeiraCartao = resp.LeInformacao(40) ?? string.Empty;

                var pagamento = pagamentosCartao[posicaoPagamento];
                pagamento.AlteradoEm = DateTime.Now;
                pagamento.CartaoCredito = credito;
                pagamento.CartaoDebito = debito;
                pagamento.Nsu = nsu;
                pagamento.Rede = rede;
                pagamento.CodigoAutorizacao = codigoAutorizacao ?? string.Empty;
                pagamento.QuantidadeParcelas = quantidadeParcelas;
                pagamento.ComprovanteEmitidoEm = dataEHoraTransacaoComprovante;
                pagamento.Desconto = desconto;
                pagamento.Saque = saque;
                pagamento.Valor = total;

                Enum.GetValues(typeof (TipoTransacao))
                    .Cast<TipoTransacao>().ForEach(t =>
                    {
                        if (t == (TipoTransacao) tipoTransacao)
                        {
                            pagamento.TipoTransacao = t;
                        }
                    });

                pagamento.TipoParcelamento = (TipoParcelamento) tipoParcelamento;
                pagamento.NomeAdministradora = nomeAdministradora;
                pagamento.BandeiraCartao = bandeiraCartao;

                posicaoPagamento++;
            });


            var sessao = GerenciaSessao.ObterSessao(nameof(SessaoPdv)).AbrirSessao();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                _vendaEcf.VendaEcfPagamentos.ForEach(pagamento => SalvarPagamento(pagamento, sessao));

                transacao.Commit();
            }
        }

        private void SalvarVendaEcf(ISession sessao)
        {
            var repositorioVendaEcf = new VendaEcfRepositorio(sessao);

            _vendaEcf.AlteradoEm = DateTime.Now;
            _vendaEcf.Status = VendaStatus.Fechada;

            repositorioVendaEcf.Salvar(_vendaEcf);
        }

        private void SalvarPagamento(VendaEcfPagamentoDt pagamento, ISession sessao)
        {
            try
            {
                var repositorioPagamento = new VendaEcfPagamentoRepositorio(sessao);

                pagamento.AlteradoEm = DateTime.Now;
                repositorioPagamento.Salvar(pagamento);
            }
            catch (Exception)
            {
                AtivaLayoutPadrao();
                OnAtivaTudo();
                throw;
            }
        }

        private void OnAntesFinalizarRequisicao(object sender, AntesFinalizarRequisicaoEventArgs e)
        {
        }

        private void OnInfoEcf(object sender, InfoECFEventArgs e)
        {
            try
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
                        var valor = SessaoEcf.EcfFiscal.SubTotal;

                        valor = valor - SessaoEcf.EcfFiscal.TotalPago + _descontoOuAcrescimo;

                        e.Value = valor;
                        break;

                    case InfoECF.TotalAPagar:
                        e.Value = CalculaTotalPago();
                        break;
                }
            }
            catch (Exception)
            {
                OnAtivaTudo();
                AtivaLayoutPadrao();
            }
        }

        private decimal? CalculaTotalPago()
        {
            var valorTotal = FormaPagamentosTemp.Where(f => f.FormaPagamento.Id == 1).Sum(f => f.Valor);

            return valorTotal;
        }

        private void OnComandaEcfImprimeVia(object sender, ComandaECFImprimeViaEventArgs e)
        {
            try
            {
                switch (e.TipoRelatorio)
                {
                    case TipoRelatorio.Gerencial:
                        SessaoEcf.EcfFiscal.LinhaRelatorioGerencial(e.ImagemComprovante);
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
                OnAtivaTudo();
                AtivaLayoutPadrao();
            }
        }

        private void OnComandaEcfAbreVinculado(object sender, ComandaECFAbreVinculadoEventArgs e)
        {
            try
            {
                SessaoEcf.EcfFiscal.AbreCupomVinculado(e.COO, e.IndiceECF, e.Valor);
                e.RetornoECF = true;
            }
            catch (Exception)
            {
                e.RetornoECF = false;
                OnAtivaTudo();
                AtivaLayoutPadrao();
            }
        }

        private void OnComandaEcfPagamento(object sender, ComandaECFPagamentoEventArgs e)
        {
            try
            {
                SessaoEcf.EcfFiscal.EfetuaPagamento(e.IndiceECF, e.Valor);
                e.RetornoECF = true;
            }
            catch (Exception)
            {
                e.RetornoECF = false;
                OnAtivaTudo();
                AtivaLayoutPadrao();
            }
        }

        private void OnComandaEcfSubtotaliza(object sender, ComandaECFSubtotalizaEventArgs e)
        {
            try
            {
                OnDesativaTudo();
                AtivaLayoutPagamento();
                SessaoEcf.EcfFiscal.SubtotalizaCupom(e.DescAcre + _descontoOuAcrescimo);
                _descontoOuAcrescimo = 0;
                e.RetornoECF = true;
            }
            catch (Exception)
            {
                e.RetornoECF = false;
                OnAtivaTudo();
                AtivaLayoutPadrao();
            }
        }

        private void OnComandaEcf(object sender, ComandaECFEventArgs e)
        {
            try
            {
                switch (e.Operacao)
                {
                    case OperacaoECF.AbreGerencial:
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
                        CancelarVenda();
                        break;
                    case OperacaoECF.ImprimePagamentos:
                        FormaPagamentosTemp.ForEach(
                            f =>
                            {
                                SessaoEcf.EcfFiscal.EfetuaPagamento(f.FormaPagamento.CodigoEcf, f.Valor);
                            });

                        FormaPagamentosTemp.Clear();
                        break;
                }

                e.RetornoECF = true;
            }
            catch (Exception)
            {
                e.RetornoECF = false;
                OnAtivaTudo();
                AtivaLayoutPadrao();
            }
        }

        private void AtivaLayoutPagamento()
        {
            LayoutEfetuarPagamento = false;
            LayoutMensagem = true;
        }

        private static void CancelarVenda()
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
                    InvocaComponenteTela(() =>
                    {
                        DialogBox.MostraInformacao(e.Mensagem);
                        OnAtivaTudo();
                    });
                    e.ModalResult = ModalResult.Yes;
                    break;
                case OperacaoMensagem.RemoverMsgOperador:
                    InvocaComponenteTela(() => { MensagemOperacao = "Finalizando Cupom Fiscal"; });
                    e.ModalResult = ModalResult.Yes;
                    break;
                case OperacaoMensagem.ExibirMsgCliente:
                case OperacaoMensagem.ExibirMsgOperador:
                    InvocaComponenteTela(() => { MensagemOperacao = e.Mensagem; });
                    e.ModalResult = ModalResult.Yes;
                    break;
                case OperacaoMensagem.DestaqueVia:
                    InvocaComponenteTela(() => { MensagemOperacao = e.Mensagem; });
                    e.ModalResult = ModalResult.Yes;
                    Thread.Sleep(3000);
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

        private static VendaEcfDt CriaVendaNova(VendaEcfDt obterVenda)
        {
            return new VendaEcfDt
            {
                Acrescimo = obterVenda.Acrescimo,
                AcrescimoItens = obterVenda.AcrescimoItens,
                Cancelado = obterVenda.Cancelado,
                Ccf = obterVenda.Ccf,
                Cfop = obterVenda.Cfop,
                ClienteDt = obterVenda.ClienteDt,
                Coo = obterVenda.Coo,
                Desconto = obterVenda.Desconto,
                DescontoItens = obterVenda.DescontoItens,
                DocumentoCliente = obterVenda.DocumentoCliente,
                EcfDt = obterVenda.EcfDt,
                Id = obterVenda.Id,
                SerieEcf = obterVenda.SerieEcf,
                NomeCliente = obterVenda.NomeCliente,
                Observacao = obterVenda.Observacao,
                TotalCupom = obterVenda.TotalCupom,
                VendidoEm = obterVenda.VendidoEm,
                TaxaAcrescimo = obterVenda.TaxaAcrescimo,
                TaxaDesconto = obterVenda.TaxaDesconto,
                EnderecoCliente = obterVenda.EnderecoCliente,
                QuantidadeItens = obterVenda.QuantidadeItens,
                Status = obterVenda.Status,
                TotalBaseIcms = obterVenda.TotalBaseIcms,
                TotalCancelado = obterVenda.TotalCancelado,
                TotalFinal = obterVenda.TotalFinal,
                TotalProdutos = obterVenda.TotalProdutos,
                TotalRecebido = obterVenda.TotalRecebido,
                Troco = obterVenda.Troco,
                VendaEcfItens = obterVenda.VendaEcfItens,
                VendaEcfPagamentos = obterVenda.VendaEcfPagamentos,
                AlteradoEm = obterVenda.AlteradoEm,
                UuidVenda = GuuidHelper.Computar("PDV" + DateTime.Now.ToString("G") + SessaoSistema.UsuarioLogado.Login)
            };
        }

        public void MensagemAdicionarUmValor()
        {
            var informacao = new Informacao {Texto = "Adicione um valor porfavor."};

            Informacoes.Add(informacao);
        }

        public void AdicionaValor()
        {
            try
            {
                Finaliza();
            }
            catch (ACBrException ex)
            {
                InvocaComponenteTela(() =>
                {
                    TerminouDeLancarPagamento = true;
                    FormaPagamentos.Remove(FormaPagamento);
                    FormaPagamento.Valor = FormaPagamento.Valor*-1;
                    FormaPagamento.Calcula(_vendaEcf);
                    AtualizarTotais();
                    Informacoes.Clear();
                    MenssagemTop = "Escolher um meio de pagamento.";
                });
                throw new ACBrException(ex.Message);
            }
            catch (PagamentoNegativoException ex)
            {
                TerminouDeLancarPagamento = true;
                throw new PagamentoNegativoException(ex.Message, ex);
            }
            catch (ValorMenorQueZeroException ex)
            {
                TerminouDeLancarPagamento = true;
                throw new ValorMenorQueZeroException(ex.Message, ex);
            }
            catch (ArquivoAuxiliarInvalidoException ex)
            {
                TerminouDeLancarPagamento = true;
                throw new InvalidOperationException(ex.Message, ex);
            }
            catch (ExceptionCartao ex)
            {
                TerminouDeLancarPagamento = true;
                throw new ExceptionCartao(ex.Message, ex);
            }
            catch (Exception ex)
            {
                TerminouDeLancarPagamento = true;
                throw new Exception("Falha ao concluir pagamento", ex);
            }
        }

        public bool EUmAjusteDeSaldo()
        {
            if (FormaPagamento.FormaPagamento.Id == 10 && NaoPodeFinalizar())
            {
                Pagamento = 0.ToString("N2");
                return true;
            }
            return false;
        }

        public void AdicionaPagamenoASerImpressosNaEcf()
        {
            if (FormaPagamento.FormaPagamento.Id == 1 || FormaPagamento.FormaPagamento.Id == 3 ||
                FormaPagamento.FormaPagamento.Id == 4)
            {
                FormaPagamentosTemp.Add(FormaPagamento);
            }
        }

        public void ExecutaValidacoesDeIntegridade(decimal valor)
        {
            FormaPagamento.Valor = valor;

            VerificaSeTemCartaoSeTiverValida();

            FormaPagamento.Calcula(_vendaEcf);

            FormaPagamentos.Add(FormaPagamento);
        }

        public decimal PagamentoDeveSerMaiorQueZero()
        {
            var valor = decimal.Parse(_pagamento);

            if (valor <= 0)
            {
                TerminouDeLancarPagamento = true;
                throw new ValorMenorQueZeroException("Valor deve ser maior que 0");
            }
            return valor;
        }

        public void Finaliza()
        {
            FinalizaCupom(FormaPagamentos);
            InvocaComponenteTela(() => { Pagamento = 0.ToString("N2"); });
        }

        public bool NaoPodeFinalizar()
        {
            if (_vendaEcf.TotalRecebido >= _vendaEcf.TotalFinal) return false;

            Pagamento = 0.ToString("N2");
            return true;
        }

        public void VerificaSeTemCartaoSeTiverValida()
        {
            if (FormaPagamento.FormaPagamento.Id == 10) return;
            if (FormaPagamentos.Count(f => f.FormaPagamento.Id == 2) <= 0 && FormaPagamento.FormaPagamento.Id != 2)
                return;
            // ReSharper disable once RedundantBoolCompare
            var valorTotal = FormaPagamentos.Where(f => f.FormaPagamento.Ecf == true).Sum(f => f.Valor) +
                             FormaPagamento.Valor;

            if (valorTotal > Subtotal)
            {
                TerminouDeLancarPagamento = true;
                throw new ExceptionCartao(
                    "Quando a operação tiver cartão não existe troco, o valor total dos pagamentos tem que ser igual ao Subtotal");
            }
        }

        public void ValidarPagamento()
        {
            try
            {
                if (_pagamento.Contains("."))
                {
                    TerminouDeLancarPagamento = true;
                    throw new InvalidOperationException();
                }

                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                decimal.Parse(_pagamento);
            }
            catch (Exception ex)
            {
                TerminouDeLancarPagamento = true;
                throw new ExceptionValorInvalido("Digite um valor valído.", ex);
            }
        }

        private decimal CalculaSaldoRestante()
        {
            var troco = Math.Abs(_vendaEcf.TotalFinal - _vendaEcf.TotalRecebido);

            if (_vendaEcf.TotalRecebido <= _vendaEcf.TotalFinal) return troco;
            _vendaEcf.Troco = troco;
            return 0;
        }

        private decimal CalculaSubTotal()
        {
            return _vendaEcf.TotalFinal;
        }

        public void AtualizaInformacao()
        {
            if (FormaPagamento.GetType() == typeof (PagamentoDinheiro))
            {
                MensagemSucesso();
            }

            if (FormaPagamento.GetType() == typeof (PagamentoCartaoTef))
            {
                MensagemSucesso();
            }

            if (FormaPagamento.GetType() == typeof (PagamentoCartaoPos))
            {
                MensagemSucesso();
            }

            if (FormaPagamento.GetType() == typeof (PagamentoAjuste))
            {
                MessagemAjusteSucesso();
            }
        }

        private void MessagemAjusteSucesso()
        {
            Informacoes.Add(new Informacao
            {
                Texto = "Pagamento ajustado com sucesso."
            });
        }

        public void MensagemNovoValor()
        {
            Informacoes.Add(new Informacao
            {
                Texto = "Informar o novo Valor a ser cobrado."
            });
        }

        private void FinalizaCupom(IEnumerable<IPagamento> formaPagamentos)
        {
            try
            {
                if (_vendaEcf.TotalRecebido >= _vendaEcf.TotalFinal)
                {
                    CalculaDescontoOuAcrescimo();
                    EfetuaPagamento(formaPagamentos);
                }
            }
            catch (ACBrException ex)
            {
                TerminouDeLancarPagamento = true;
                throw new ACBrException(ex.Message);
            }
            catch (PagamentoNegativoException ex)
            {
                TerminouDeLancarPagamento = true;
                throw new PagamentoNegativoException(ex.Message, ex);
            }
            catch (ArquivoAuxiliarInvalidoException ex)
            {
                TerminouDeLancarPagamento = true;
                throw new ArquivoAuxiliarInvalidoException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                TerminouDeLancarPagamento = true;
                throw new InvalidOperationException("Falha ao concluir pagamento", ex);
            }
            finally
            {
                TerminouDeLancarPagamento = true;
            }
        }

        private void EfetuaPagamento(IEnumerable<IPagamento> formaPagamentos)
        {
            _vendaEcf.Status = VendaStatus.Fechada;
            var pagamentos = formaPagamentos as IPagamento[] ?? formaPagamentos.ToArray();
            var formasPagto = pagamentos.Where(fp => fp.FormaPagamento.Ecf.Equals(true)).ToList();
            var pagamentoCartaoSucesso = true;

            AdicionarPagamentos(formasPagto);

            FormaPagamentos.Where(f => f.FormaPagamento.Id == 2).ForEach(f =>
            {
                if (f.Pagou) return;

                pagamentoCartaoSucesso = false;
                UsandoTef = true;

                try
                {
                    pagamentoCartaoSucesso = _tef.CRT(f.Valor, f.FormaPagamento.CodigoEcf, SessaoEcf.EcfFiscal.Coo());
                }
                catch (Exception ex)
                {
                    InvocaComponenteTela(() => { DialogBox.MostraInformacao(ex.Message); });
                    FechaRelatorioSeOuverPendente();
                    UsandoTef = false;
                    return;
                }

                UsandoTef = false;
                f.Pagou = pagamentoCartaoSucesso;
            });

            pagamentoCartaoSucesso = VerificaSeTodosPagamentosDeCartaoForamSucesso(pagamentoCartaoSucesso);
            if (!pagamentoCartaoSucesso) return;

            PersisteDadosDaVenda();
            FechaCupom();

            new AtualizarGt(SessaoEcf.EcfFiscal.GrandeTotal().ToString(CultureInfo.CurrentCulture)).Executar();

            _pagamentoConcluido = true;
            OnFecharTela();
        }

        private void PersisteDadosDaVenda()
        {
            var sessao = GerenciaSessao.ObterSessao(SessaoPdv.FabricaTres).AbrirSessao();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                _vendaEcf.VendaEcfPagamentos.ForEach(p =>
                {
                    SalvarPagamento(p, sessao);
                });
                
                SalvarVendaEcf(sessao);


                transacao.Commit();
            }
        }

        private void FechaRelatorioSeOuverPendente()
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
                InvocaComponenteTela(() => { DialogBox.MostraAviso(ex.Message); });
            }
        }

        private bool VerificaSeTodosPagamentosDeCartaoForamSucesso(bool pagamentoCartaoSucesso)
        {
            if (FormaPagamentos.Where(f => f.FormaPagamento.Id == 2).ToList().Count > 0)
            {
                FormaPagamentos.Where(f => f.FormaPagamento.Id == 2).ForEach(f =>
                {
                    if (f.Pagou == false)
                    {
                        pagamentoCartaoSucesso = false;
                    }
                });
            }
            return pagamentoCartaoSucesso;
        }

        private void AdicionarPagamentos(List<IPagamento> formasPagto)
        {
            var pagamentos = new List<VendaEcfPagamentoDt>();

            formasPagto.ForEach(formaPagamento =>
            {
                var vendaPagamento = MontaObjetoVendaEcfPagamento(formaPagamento);
                vendaPagamento.AlteradoEm = DateTime.Now;
                pagamentos.Add(vendaPagamento);
            });

            _vendaEcf.AlteradoEm = DateTime.Now;
            _vendaEcf.VendaEcfPagamentos = pagamentos;
        }

        private void FechaCupom()
        {
            _tef.FinalizarCupom();
            _tef.ImprimirTransacoesPendentes();
        }

        private void CalculaDescontoOuAcrescimo()
        {
            _descontoOuAcrescimo = _vendaEcf.Acrescimo != 0 ? _vendaEcf.Acrescimo : -_vendaEcf.Desconto;
        }

        public bool PagamentoFoiConcluido()
        {
            return _pagamentoConcluido;
        }

        private void AdicionaMensagemTroco(decimal troco)
        {
            _informacoes.Add(new Informacao
            {
                Texto = "Troco"
            });

            _troco = troco;

            _informacoes.Add(new Informacao
            {
                Texto = _troco.ToString("C")
            });
        }

        private VendaEcfPagamentoDt MontaObjetoVendaEcfPagamento(IPagamento pagamento)
        {
            var vendaEcfPagamento = new VendaEcfPagamentoDt
            {
                FormaPagamentoEcfDt = new FormaPagamentoEcfDt
                {
                    CodigoEcf = pagamento.FormaPagamento.CodigoEcf,
                    Id = pagamento.FormaPagamento.Id,
                    Nome = pagamento.FormaPagamento.Nome,
                    AlteradoEm = DateTime.Now
                },
                Valor = pagamento.Valor,
                SerieEcf = _vendaEcf.SerieEcf,
                VendaEcfDt = _vendaEcf,
                Nsu = string.Empty,
                Rede = string.Empty,
                AlteradoEm = DateTime.Now,
                Ccf = _vendaEcf.Ccf,
                Cco = _vendaEcf.Coo,
                BandeiraCartao = string.Empty,
                NomeAdministradora = string.Empty,
                TipoParcelamento = TipoParcelamento.ParceladoPeloEstabelecimento,
                TipoTransacao = TipoTransacao.NaoDefinido
            };

            return vendaEcfPagamento;
        }

        private void AdicionaMensagemDinheiro()
        {
            try
            {
                _informacoes.Add(new Informacao
                {
                    Texto = decimal.Parse(_pagamento).ToString("C")
                });
            }
            catch (Exception ex)
            {
                throw new ExceptionValorInvalido("Digite um valor valído.", ex);
            }
        }

        private void MensagemSucesso()
        {
            _informacoes.Add(new Informacao
            {
                Texto = "Meio de pagamento adicionado com sucesso."
            });
        }

        public void MensagemPressioneEsc()
        {
            MenssagemTop = "Pressione ESC para sair.";
        }

        public void MensagemValorInvalido()
        {
            _informacoes.Add(new Informacao
            {
                Texto = "Valor digitado é invalido."
            });
        }

        public void InicializaFormaPagamentoLocal()
        {
            if (_formaPagamentos == null)
            {
                FormaPagamentos = new ObservableCollection<IPagamento>();
            }
        }

        public void AtualizarTotais()
        {
            TotalCupom = CalculaTotalCupom();
            Subtotal = CalculaSubTotal();
            Saldo = CalculaSaldoRestante();
        }

        private decimal CalculaTotalCupom()
        {
            return _vendaEcf.TotalCupom;
        }

        public void AtualizaMensagem()
        {
            if (FormaPagamento.GetType() == typeof (PagamentoDinheiro) 
                || FormaPagamento.GetType() == typeof(PagamentoCartaoPos)
                || FormaPagamento.GetType() == typeof(PagamentoCrediario))
            {
                AdicionaMensagemDinheiro();

                ValidarPagamento();

                InicializaFormaPagamentoLocal();
                var valor = decimal.Parse(_pagamento);

                if (valor <= 0)
                {
                    throw new ValorMenorQueZeroException("Valor deve ser maior que 0");
                }

                decimal troco;
                if (CalculaTroco(valor, out troco)) return;

                AdicionaMensagemTroco(troco);

                return;
            }

            AdicionaMensagemDinheiro();
        }

        private bool CalculaTroco(decimal valor, out decimal troco)
        {
            var totalRecebido = valor + _vendaEcf.TotalRecebido;

            if (totalRecebido <= _vendaEcf.TotalFinal)
            {
                troco = 0;
                return true;
            }

            troco = Math.Abs(_vendaEcf.TotalFinal - totalRecebido);
            return false;
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
            _tef.OnComandaECFPagamento -= OnComandaEcfPagamento;
            _tef.OnComandaECFAbreVinculado -= OnComandaEcfAbreVinculado;
            _tef.OnComandaECFImprimeVia -= OnComandaEcfImprimeVia;
            _tef.OnInfoECF -= OnInfoEcf;
            _tef.OnAntesFinalizarRequisicao -= OnAntesFinalizarRequisicao;
            _tef.OnDepoisConfirmarTransacoes -= OnDepoisConfirmarTransacoes;
            _tef.OnAntesCancelarTransacao -= OnAntesCancelarTransacao;
            _tef.OnDepoisCancelarTransacoes -= OnDepoisCancelarTransacoes;
            _tef.OnMudaEstadoReq -= OnMudaEstadoReq;
            _tef.OnMudaEstadoResp -= OnMudaEstadoResp;
        }

        public bool BotaoFinalizar()
        {
            if (Saldo != 0) return false;

            BotaoFinalizarVisivel = true;
            return true;
        }

        private void OnFecharTela()
        {
            FecharTela?.Invoke(this, EventArgs.Empty);
        }

        private void InvocaComponenteTela(Action action)
        {
            Application.Current.Dispatcher.Invoke(action.Invoke);
        }

        private void OnDesativaTudo()
        {
            DesativaTudo?.Invoke(this, EventArgs.Empty);
        }

        private void OnAtivaTudo()
        {
            AtivaTudo?.Invoke(this, EventArgs.Empty);
        }

        public bool ClienteInvalido()
        {

            if (string.IsNullOrEmpty(_vendaEcf.DocumentoCliente)) return true;

            try
            {
                using (var sessaoAdm = GerenciaSessao.ObterSessao(nameof(SessaoAdm)).AbrirSessao())
                using (var sessaoPdv = GerenciaSessao.ObterSessao(nameof(SessaoPdv)).AbrirSessao())
                using (var transacaoPdv = sessaoPdv.BeginTransaction())
                {
                    var repositorioClienteAdm = new RepositorioPessoa(sessaoAdm);

                    var cliente = repositorioClienteAdm.BuscarClientePorCpfOuCnpjPdv(_vendaEcf.DocumentoCliente);

                    if (cliente == null) throw new InvalidOperationException("Este cliente não está cadastrado, porfavor cadastrar o mesmo");

                    _vendaEcf.ClienteDt = cliente;
                    _vendaEcf.DocumentoCliente = string.IsNullOrEmpty(cliente.Cpf) ? cliente.Cnpj : cliente.Cpf;
                    _vendaEcf.NomeCliente = cliente.Nome;
                    cliente.Endereco = cliente.Endereco ?? string.Empty;

                    sessaoPdv.SaveOrUpdate(cliente);

                    transacaoPdv.Commit();
                }
            }
            catch (KeyNotFoundException)
            {
                new Thread(() =>
                {
                    try
                    {
                        GerenciaSessao.GerenciaSessaoInicializar();
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }).Start();
                throw new InvalidOperationException("Servidor não está respondendo, porfavor verificar.");
            }

            

            return _vendaEcf.ClienteDt == null;
        }

        public void AdicionaCliente(ClienteDt cliente)
        {
            _vendaEcf.ClienteDt = cliente;
            _vendaEcf.DocumentoCliente = string.IsNullOrEmpty(cliente.Cpf) ? cliente.Cnpj : cliente.Cpf;
            _vendaEcf.NomeCliente = cliente.Nome;
        }

        public void ValidaLimiteCliente(decimal valor)
        {
            TrataErroServidor(() =>
            {
                using (var sessaoAdm = GerenciaSessao.ObterSessao(nameof(SessaoAdm)).AbrirSessao())
                {
                    var repositorioClienteAdm = new RepositorioPessoa(sessaoAdm);

                    var cliente = repositorioClienteAdm.BuscarClientePorCpfOuCnpjPdv(string.IsNullOrEmpty(_vendaEcf.ClienteDt.Cpf) ? _vendaEcf.ClienteDt.Cnpj : _vendaEcf.ClienteDt.Cpf);

                    if (cliente == null)
                    {
                        throw new ArgumentException("Cliente não existe na base de dados do administrativo, porfavor cadastrar o mesmo");
                    }

                    if (cliente.AplicaLimiteCredito == false) return;

                    var valorAReceber = repositorioClienteAdm.GetTotalEmAbertoFinanceiro(cliente.Id);

                    var limiteDisponivel = _vendaEcf.ClienteDt.LimiteCredito - valorAReceber;

                    if ((limiteDisponivel > 0 && limiteDisponivel < valor) || limiteDisponivel < 0)
                    {
                        throw new ArgumentException("Cliente não tem limite disponivel para efetuar esta compra \n" +
                                                   "O limite disponivel para o cliente é " + limiteDisponivel.ToString("C") + " \n"
                                                   + "Valor total da compra " + valor.ToString("C"));
                    }
                }
            });
        }



        private static void TrataErroServidor(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (KeyNotFoundException)
            {
                DialogBox.MostraInformacao("Servidor não está respondendo, porfavor verificar.");
                new Thread(() =>
                {
                    try
                    {
                        GerenciaSessao.GerenciaSessaoInicializar();
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }).Start();
            }
        }

        public bool AjusteDeSaldoEMenorQueTotalPago(decimal valor)
        {
            if (FormaPagamento.FormaPagamento.Id != 10) return false;

            var valorTotal = FormaPagamentos.Where(f => f.FormaPagamento.Ecf == true).Sum(f => f.Valor);

            if (valor >= valorTotal) return false;

            DialogBox.MostraInformacao("O Ajuste de saldo não pode ser menor que o valor total pago");

            return true;
        }

        public string Coo()
        {
            return _vendaEcf.Coo.ToString();
        }
    }
}