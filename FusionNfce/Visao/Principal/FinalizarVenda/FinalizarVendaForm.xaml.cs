using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Fusion.FastReport.Facades;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionNfce;
using FusionNfce.AutorizacaoSatFiscal.Helper;
using FusionNfce.Impressao;
using FusionNfce.Parcelamento;
using FusionNfce.Visao.Principal.FinalizarVenda.AlteraTotais;
using FusionNfce.Visao.Principal.FinalizarVenda.CartoesPos;
using FusionNfce.Visao.Principal.FinalizarVenda.FormasPagamento;
using FusionNfce.Visao.Principal.FinalizarVenda.Outros;
using FusionNfce.Visao.Principal.FinalizarVenda.Tef.Pos;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Factories;
using MahApps.Metro.SimpleChildWindow;

namespace FusionNfce.Visao.Principal.FinalizarVenda
{
    public partial class FinalizarVendaForm
    {
        private readonly DanfeNfceFacade _danfeFacade = new DanfeNfceFacade();
        private readonly FinalizarVendaFormModel _model;
        private ChildWindow _childAtual;

        public FinalizarVendaForm(FinalizarVendaFormModel model)
        {
            IsEnabled = false;

            _model = model;
            _model.FocusDigitarPagamento += FocusDigitarPagamento;
            _model.RetiraEventosDeBotoes += RetiraEventos;
            _model.AdicionaEventosDeBotoes += AddEventos;
            _model.SucessoNfce += SucessoEmissaoNfce;
            _model.FocusBotaoTransmitir += FocusBotaoTransmitir;
            DataContext = model;
            InitializeComponent();
        }

        private void FinalizarVendaForm_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (IsEnabled == false)
            {
                return;
            }

            // f1 - Dinheiro
            // f2 - Cartao POS
            // f3 - Cartão Crédito
            // f4 - Cartão Débito
            // f6 - Crediário
            // f7 - Desconto

            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
                case Key.F1:
                    BtDinheiro_OnClick(sender, e);
                    break;
                case Key.F2:
                    BtCartaoPos_OnClick(sender, e);
                    BtCartaoTef_OnClick(sender, e);
                    break;
                case Key.F3:
                    BtCartaoCredito_OnClick(sender, e);
                    break;
                case Key.F4:
                    BtCartaoDebito_OnClick(sender, e);
                    break;
                case Key.F5:
                    _model.BuscarCliente();
                    break;
                case Key.F6:
                    BtCrediario_OnClick(sender, e);
                    break;
                case Key.F7:
                    AlteraTotal_OnClick(sender, e);
                    break;
                case Key.F8:
                    Observacao_OnClick(sender, e);
                    break;
                case Key.F9:
                    BtOutros_OnClick(sender, e);
                    break;
            }
        }

        private void FocusBotaoTransmitir(object sender, EventArgs e)
        {
            BtTransmitir.Focus();
        }

        private void SucessoEmissaoNfce(object sender, SucessoNfceEvent e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _model.SucessoLimparCampos();

                Close();

                try
                {
                    FusionCore.FusionAdm.Acbr.Servicos.AbrirGaveta.Executar();

                    if (SessaoSistemaNfce.IsEmissorNFce())
                    {
                        new ServicoImpressaoNfce(e.Nfce.Id, _danfeFacade).Imprimir();

                        var imprimeEventArgs = e.ImprimeViaEventArgs;

                        if (imprimeEventArgs != null && imprimeEventArgs.IsTemVia1)
                        {
                            Thread.Sleep(3000);

                            new TefImpressaoFacade().Imprimir(imprimeEventArgs.Via1, SessaoSistemaNfce.Preferencia.NomeImpressora);
                        }

                        if (imprimeEventArgs != null && imprimeEventArgs.IsTemVia2)
                        {
                            Thread.Sleep(3000);

                            new TefImpressaoFacade()
                                .Imprimir(imprimeEventArgs.Via2, SessaoSistemaNfce.Preferencia.NomeImpressora);
                        }
                    }

                    if (SessaoSistemaNfce.IsEmissorSat())
                    {
                        var repositorio = new RepositorioNfce(GerenciaSessaoNfce
                            .ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao());

                        XmlAutorizadoDto autorizado;

                        using (repositorio)
                        {
                            autorizado = repositorio.BuscarXmlSatAutorizado(e.Nfce.Id);
                        }

                        DanfeSatHelper.Imprimir(
                            autorizado, 
                            SessaoSistemaNfce.Preferencia.NomeImpressora,
                            SessaoSistemaNfce.Preferencia.NomeFantasiaCustomizado);
                    }
                }
                catch (Exception ex)
                {
                    DialogBox.MostraAviso($"NFC-E autorizada. Falha ao imprimir: {ex.Message}");
                }
            });
        }

        private void RetiraEventos(object sender, EventArgs e)
        {
            Tela.KeyDown -= FinalizarVendaForm_OnKeyDown;
            Tela.KeyDown += FinalizarVendaForm_OnKeyDownFechar;
        }

        private void AddEventos(object sender, EventArgs e)
        {
            Tela.KeyDown += FinalizarVendaForm_OnKeyDown;
            Tela.KeyDown -= FinalizarVendaForm_OnKeyDownFechar;
        }

        private void FocusDigitarPagamento(object sender, EventArgs e)
        {
            TBValorDigitado.SelectAll();
            TBValorDigitado.Focus();
        }

        private void BtDinheiro_OnClick(object sender, RoutedEventArgs e)
        {
            _model.UsarDinheiro();
        }

        private void BtOutros_OnClick(object sender, RoutedEventArgs e)
        {
            _model.UsarOutros();
        }

        private void TbPagamento_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return) return;

            PreLancarPagamento();
        }

        private bool PreLancarPagamento(decimal? acrescimoOuDesconto = null)
        {
            try
            {
                _model.LancaPagamento(acrescimoOuDesconto);
                BtLancar.Focus();
                return true;
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
                TBValorDigitado.SelectAll();
                TBValorDigitado.Focus();
                return false;
            }
        }

        private async void BtLancarPagamento_OnClick(object sender, RoutedEventArgs e)
        {
            if (_model.EnumFormaPagamento == FormaPagamento.Crediario)
            {
                await OpcaoPagamentoCrediario();
                return;
            }

            if (_model.EnumFormaPagamento == FormaPagamento.CartaoCredito ||
                _model.EnumFormaPagamento == FormaPagamento.CartaoDebito)
            {
                if (SessaoSistemaNfce.Preferencia.NaoSolicitaDadosCartaoPos() ||
                    SessaoSistemaNfce.IsEmissorNFce() == false)
                {
                    EfetuaPagamento();
                    return;
                }

                var model = new CartaoPosFormModel();
                model.EnviarDadosCartaoPos += EnviarDadosCartaoPos;
                _childAtual = ChildWindowFactory.Cria(new CartaoPosForm(), model, "Dados Cartão POS");

                await this.ShowChildWindowAsync(_childAtual, ChildWindowManager.OverlayFillBehavior.FullWindow);

                return;
            }

            if (_model.EnumFormaPagamento == FormaPagamento.CartaoPos && SessaoSistemaNfce.IsMFe())
            {
                var model = new EfetuaPagamentoPosFormModel();
                var view = new EfetuaPagamentoPosForm(model);

                view.ShowDialog();

                if (model.IsCancelarOperacao)
                    return;

                EfetuaPagamento(null, null, model);
                return;
            }

            if (_model.EnumFormaPagamento == FormaPagamento.Outros)
            {

                var model = new DescricaoOutrosFormModel();
                model.EnviarDescricaoOutros += EnviarDescricaoOutros;
                _childAtual = ChildWindowFactory.Cria(new DescricaoOutrosForm(), model);

                await this.ShowChildWindowAsync(_childAtual, ChildWindowManager.OverlayFillBehavior.FullWindow);

                return;
            }

            EfetuaPagamento();
        }

        private void EnviarDescricaoOutros(object sender, DescricaoOutrosFormModel e)
        {
            EfetuaPagamento(descricaoOutrosModel:e);

            _childAtual.Close();
        }

        private async Task OpcaoPagamentoCrediario()
        {
            if (_model.NãoPossuiCliente())
            {
                DialogBox.MostraAviso("Preciso de um Cliente para pagamentos com Crediário!");
                return;
            }

            var valor = decimal.Parse(_model.ValorDigitado);

            var factory = new ParcelamentoFactory();
            var dialog = factory.CriaDialog(valor);

            dialog.Contexto.ParceladoComSucesso += (sender, args) =>
            {
                _model.AdicionaCobrancaNaNFCe(args);
                EfetuaPagamento();
            };

            _childAtual = dialog;

            await this.ShowChildWindowAsync(_childAtual, ChildWindowManager.OverlayFillBehavior.FullWindow);
        }

        private void EnviarDadosCartaoPos(object sender, CartaoPosFormModel e)
        {
            EfetuaPagamento(e);
            _childAtual.Close();
        }

        private void EfetuaPagamento(
            CartaoPosFormModel cartaoPosFormModel = null, 
            DescricaoOutrosFormModel descricaoOutrosModel = null,
            EfetuaPagamentoPosFormModel efetuaPagamentoPosFormModel = null)
        {
            try
            {
                _model.EfetuarPagamento(cartaoPosFormModel, descricaoOutrosModel, efetuaPagamentoPosFormModel);
            }
            catch (OverflowException)
            {
                throw new InvalidOperationException("Valor está muito grande");
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void FinalizarVendaForm_OnKeyDownFechar(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
            }
        }

        private void BtTransmissao_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _model.EfetuaTransmissao();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void FinalizarVendaForm_OnContentRendered(object sender, EventArgs e)
        {
            try
            {
                if (_model.NecessarioInformarCliente())
                {
                    var dialogModel = _model.CriarModelClienteObrigatorio();
                    var dialog = new AddClienteVendaForm(dialogModel);

                    dialog.ShowDialog();

                    if (!string.IsNullOrWhiteSpace(_model.Destinatario?.DocumentoUnico))
                    {
                        return;
                    }

                    Close();
                    return;
                }

                _model.Recuperacao();
            }
            finally
            {
                IsEnabled = true;
                Tela.Focus();

                if (_model.HabilitaTransmissaoBotao)
                {
                    BtTransmitir.Focus();
                }
            }
        }

        private void BtCartaoPos_OnClick(object sender, RoutedEventArgs e)
        {
            _model.UsarCartaoPos();
        }

        private void BtCartaoDebito_OnClick(object sender, RoutedEventArgs e)
        {
            _model.UsarCartaoDebito();
        }

        private void BtPix_OnClick(object sender, RoutedEventArgs e)
        {
            _model.UsarPix();
        }

        private void BtCrediario_OnClick(object sender, RoutedEventArgs e)
        {
            if (_model.PossuiFinanceiro == false)
            {
                return;
            }

            try
            {
                _model.UsarCrediario();
            }
            catch (FormatException)
            {
                DialogBox.MostraInformacao("Porfavor digitar um valor valido.");
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void PreLancarPagamento_OnClick(object sender, RoutedEventArgs e)
        {
            PreLancarPagamento();
        }

        private void Observacao_OnClick(object sender, RoutedEventArgs e)
        {
            _model.AdicionarObservacao();
        }

        private void BtCartaoCredito_OnClick(object sender, RoutedEventArgs e)
        {
            _model.UsarCartaoCredito();
        }

        private void BtCartaoTef_OnClick(object sender, RoutedEventArgs e)
        {
            _model.UsarCartaoTef();
        }

        private async void AlteraTotal_OnClick(object sender, RoutedEventArgs e)
        {
            var model = new AdicionaDescontoOuAcrescimoFormModel(_model.Saldo);
            model.EnviarDescontoOuAcrescimo += AplicaDescontoOuAcrescimo;

            _childAtual = ChildWindowFactory.Cria(new AdicionaDescontoOuAcrescimoForm(), model);

            await this.ShowChildWindowAsync(_childAtual, ChildWindowManager.OverlayFillBehavior.FullWindow);
        }

        private void AplicaDescontoOuAcrescimo(object sender, AdicionaDescontoOuAcrescimoFormModel e)
        {
            if (e.EUmAcrescimo())
            {
                if (_model.Acrescimo() == false) return;
            }

            if (e.EUmDesconto())
            {
                if (_model.Desconto() == false) return;
            }

            if (PreLancarPagamento(e.ObterValor()) == false) return;

            EfetuaPagamento();

            _childAtual.Close();
        }
    }
}