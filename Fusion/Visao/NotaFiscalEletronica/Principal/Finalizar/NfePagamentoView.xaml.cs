using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Fusion.Parcelamento;
using Fusion.Visao.NotaFiscalEletronica.Principal.Finalizar.MeioPagamento;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.Helpers.Binding;
using FusionCore.Sessao;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Controles;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Finalizar
{
    public partial class NfePagamentoView
    {
        private readonly FusionWindow _owner;
        private readonly ISessaoManager _sessaoManager;

        public NfePagamentoView(FusionWindow owner, Nfeletronica nfe, ISessaoManager sessaoManager)
        {
            _owner = owner;
            _sessaoManager = sessaoManager;

            Contexto = new NfePagamentoContexto(nfe, sessaoManager);
            Contexto.PropertyChanged += PropertyChangedHandler;

            InitializeComponent();
        }

        public NfePagamentoContexto Contexto { get; }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            Contexto.CarregarDadosDaNfe();
            CntrConfirmacao.Visibility = Visibility.Collapsed;

            DataContext = Contexto;
            AcaoEscolhaDinheiro();
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NfePagamentoContexto.TemPagamento))
            {
                PropertyTemPagamentoChangedHandler();
            }
        }

        private void PropertyTemPagamentoChangedHandler()
        {
            if (Contexto.TemPagamento)
            {
                BlockAcoes.Visibility = Visibility.Collapsed;
                BtnDinheiro.IsEnabled = false;
                BtnPrazo.IsEnabled = false;
                BtnCDebito.IsEnabled = false;
                BtnCCredito.IsEnabled = false;
                BtnPix.IsEnabled = false;
                return;
            }

            BlockAcoes.Visibility = Visibility.Visible;
            BtnDinheiro.IsEnabled = true;
            BtnPrazo.IsEnabled = true;
            BtnCDebito.IsEnabled = true;
            BtnCCredito.IsEnabled = true;
            BtnPix.IsEnabled = true;
        }

        private void PKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2 && BtnDinheiro.IsEnabled)
            {
                e.Handled = true;
                AcaoEscolhaDinheiro();
            }

            if (e.Key == Key.F3 && BtnPrazo.IsEnabled)
            {
                e.Handled = true;
                AcaoEscolhaPrazo();
            }

            if (e.Key == Key.F4 && BtnCCredito.IsEnabled)
            {
                e.Handled = true;
                AcaoEscolhaCartaoCredito();
            }

            if (e.Key == Key.F5 && BtnCDebito.IsEnabled)
            {
                e.Handled = true;
                AcaoEscolhaCartaoDebito();
            }

            if (e.Key == Key.F6 && BtnPix.IsEnabled)
            {
                e.Handled = true;
                AcaoEscolhaPix();
            }

            if (e.Key == Key.F11)
            {
                e.Handled = true;
                AcaoLimparLancamentos();
            }

            if (e.Key == Key.F3)
            {
                //impedir abertura dos totais na janela anterior
                e.Handled = true;
            }
        }

        private void PKeyDownComandoHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                AcaoLancarPagamentoAsync();
            }
        }

        private void AcaoLancarPagamentoAsync()
        {
            TbComando.UpdateBindingText();

            try
            {
                if (ConfirmaValorLancamento() == false)
                {
                    return;
                }

                Contexto.ValidarPagamento();
                Contexto.OpcaoPagamento.PagarAsync(Contexto.ValorPagamento, PagamentoCallbackHandler);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void PagamentoCallbackHandler(Resultado resultado)
        {
            Dispatcher.Invoke(() =>
            {
                Focus();

                if (resultado == null)
                {
                    TbComando.Focus();
                    return;
                }

                if (resultado.HasError)
                {
                    DialogBox.MostraAviso(resultado.Error.Message);

                    TbComando.Focus();
                    return;
                }

                Contexto.AdicionarPagamento(resultado.Pagamento);

                if (Contexto.PossuiSaldoParaPagamento())
                {
                    AcaoEscolhaDinheiro();
                    return;
                }

                ShowButtonConfirmar();
            });
        }

        private void HideButtonConfirmar()
        {
            CntrConfirmacao.Visibility = Visibility.Collapsed;
            CntrComando.Visibility = Visibility.Visible;

            Keyboard.Focus(TbComando);
        }

        private void ShowButtonConfirmar()
        {
            CntrComando.Visibility = Visibility.Collapsed;
            CntrConfirmacao.Visibility = Visibility.Visible;

            Keyboard.Focus(BtnConfirmar);
        }

        private bool ConfirmaValorLancamento()
        {
            var msg =
                "Incluir pagamento em " +
                $"{Contexto.OpcaoPagamento.Descricao} " +
                $"no valor de {Contexto.ValorPagamento:C2}";

            return DialogBox.MostraConfirmacao(msg) == MessageBoxResult.Yes;
        }

        private void ClickDinheiroHandler(object sender, RoutedEventArgs e)
        {
            AcaoEscolhaDinheiro();
        }

        private void ClickCrediarioHandler(object sender, RoutedEventArgs e)
        {
            AcaoEscolhaPrazo();
        }

        private void CCreditoClickHandler(object sender, RoutedEventArgs e)
        {
            AcaoEscolhaCartaoCredito();
        }

        private void CDebitoClickHandler(object sender, RoutedEventArgs e)
        {
            AcaoEscolhaCartaoDebito();
        }

        private void PixClickHandler(object sender, RoutedEventArgs e)
        {
            AcaoEscolhaPix();
        }

        private void AcaoEscolhaDinheiro()
        {
            Contexto.OpcaoPagamento = new OpcaoDinheiro();
            TbComando.Focus();
        }

        private void AcaoEscolhaPrazo()
        {
            if (Contexto.PossuiLancamentoPrazo())
            {
                DialogBox.MostraAviso("Só é possível um pagamento do tipo prazo");
                TbComando.Focus();
                return;
            }

            Contexto.OpcaoPagamento = new OpcaoPrazo(_owner, this, new ParcelamentoFactory(_sessaoManager));
            TbComando.Focus();
        }

        private void AcaoEscolhaCartaoCredito()
        {
            Contexto.OpcaoPagamento = new OpcaoCartaoCredito();
            TbComando.Focus();
        }

        private void AcaoEscolhaCartaoDebito()
        {
            Contexto.OpcaoPagamento = new OpcaoCartaoDebito();
            TbComando.Focus();
        }

        private void AcaoEscolhaPix()
        {
            Contexto.OpcaoPagamento = new OpcaoPix();
            TbComando.Focus();
        }

        private void ClickConfirmarHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                Contexto.FinalizarPagamentos();
                Close(true);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void ClearClickHandler(object sender, RoutedEventArgs e)
        {
            AcaoLimparLancamentos();
        }

        private void AcaoLimparLancamentos()
        {
            var msg = "Deseja limpar os lançamentos atuais?";

            if (DialogBox.MostraConfirmacao(msg) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                Contexto.LimparLancamentos();
                HideButtonConfirmar();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void IncluiCobrancaXmlChangedHandler(object sender, EventArgs e)
        {
            try
            {
                Contexto.SalvarAlteracaoParaIncluirCobrancaXml();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}