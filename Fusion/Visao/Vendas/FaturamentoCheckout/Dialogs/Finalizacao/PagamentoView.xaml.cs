using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Fusion.Parcelamento;
using Fusion.Sessao;
using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Finalizacao.MeioPagamento;
using FusionCore.Helpers.Binding;
using FusionCore.Sessao;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Controles;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Finalizacao
{
    public partial class PagamentoView
    {
        private readonly ISessaoManager _sessaoManager = SessaoSistema.Instancia.SessaoManager;
        private readonly FusionWindow _ownerWindow;

        public PagamentoView(FusionWindow ownerWindow, PagamentoViewModel viewModel)
        {
            InitializeComponent();
            _ownerWindow = ownerWindow;
            ViewModel = viewModel;
            ViewModel.PropertyChanged += PropertyChangedHandler;
        }

        public readonly PagamentoViewModel ViewModel;

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            CntrConfirmacao.Visibility = Visibility.Collapsed;
            ViewModel.Inicializar();
            DataContext = ViewModel;
            AcaoEscolherDinheiro();
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PagamentoViewModel.TemPagamento))
            {
                PropertyTemPagamentoChangedHandler();
            }
        }

        private void PropertyTemPagamentoChangedHandler()
        {
            if (ViewModel.TemPagamento)
            {
                BlockAcoes.Visibility = Visibility.Collapsed;
                BtnDinheiro.IsEnabled = false;
                BtnPrazo.IsEnabled = false;
                BtnPix.IsEnabled = false;
                return;
            }

            BlockAcoes.Visibility = Visibility.Visible;
            BtnDinheiro.IsEnabled = true;
            BtnPrazo.IsEnabled = true;
            BtnPix.IsEnabled = true;
        }

        private void PKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2 && BtnDinheiro.IsEnabled)
            {
                e.Handled = true;
                AcaoEscolherDinheiro();
            }

            if (e.Key == Key.F3 && BtnPrazo.IsEnabled)
            {
                e.Handled = true;
                AcaoEscolherPrazo();
            }

            if (e.Key == Key.F4 && BtnCCredito.IsEnabled)
            {
                e.Handled = true;
                AcaoEscolherCartaoCredito();
            }

            if (e.Key == Key.F5 && BtnCDebito.IsEnabled)
            {
                e.Handled = true;
                AcaoEscolherCartaoDebito();
            }

            if (e.Key == Key.F6 && BtnPix.IsEnabled)
            {
                e.Handled = true;
                AcaoEscolherPix();
            }

            if (e.Key == Key.F11)
            {
                e.Handled = true;
                AcaoLimparLancamentos();
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

                ViewModel.ValidarPagamento();
                ViewModel.PrepararPagamento();

                ViewModel.OpcaoPagamento.PagarAsync(ViewModel.ValorPagamento, PagamentoCallbackHandler);
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
                    FocarTbComando();
                    return;
                }

                if (resultado.HasError)
                {
                    throw resultado.Error;
                }

                ViewModel.AdicionarPagamento(resultado.Pagamento);

                if (ViewModel.PossuiSaldoParaPagamento())
                {
                    FocarTbComando();
                    return;
                }

                ShowButtonConfirmar();
            });
        }

        private void FocarTbComando()
        {
            if (!TbComando.IsFocused)
            {
                TbComando.Focus();
            }

            TbComando.SelectAll();
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
                $"{ViewModel.OpcaoPagamento.Descricao} " +
                $"no valor de {ViewModel.ValorPagamento:C2}";

            return DialogBox.MostraConfirmacao(msg) == MessageBoxResult.Yes;
        }

        private void ClickDinheiroHandler(object sender, RoutedEventArgs e)
        {
            AcaoEscolherDinheiro();
        }

        private void AcaoEscolherDinheiro()
        {
            ViewModel.OpcaoPagamento = new OpcaoDinheiro();
            FocarTbComando();
        }

        private void ClickCrediarioHandler(object sender, RoutedEventArgs e)
        {
            AcaoEscolherPrazo();
        }

        private void AcaoEscolherPrazo()
        {
            try
            {
                if (ViewModel.TemPagador == false)
                {
                    DialogBox.MostraAviso("Essa Espécie de Pagamento precisa de um Cliente");
                    return;
                }

                if (ViewModel.JaPossuiPagamentoNoPrazo())
                {
                    DialogBox.MostraAviso("Já existe uma Espécie de Pagamento PRAZO definida.");
                }

                ViewModel.OpcaoPagamento = new OpcaoPrazo(_ownerWindow, this, new ParcelamentoFactory(_sessaoManager));
            }
            finally
            {
                FocarTbComando();
            }
        }

        private void ClickConfirmarHandler(object sender, RoutedEventArgs e)
        {
            BtnConfirmar.IsEnabled = false;

            try
            {
                ViewModel.FinalizarDocumento();
                Close(true);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            finally
            {
                BtnConfirmar.IsEnabled = true;
            }
        }

        private void CCreditoClickHandler(object sender, RoutedEventArgs e)
        {
            AcaoEscolherCartaoCredito();
        }

        private void AcaoEscolherCartaoCredito()
        {
            ViewModel.OpcaoPagamento = new OpcaoCartaoCredito();
            FocarTbComando();
        }

        private void CDebitoClickHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.OpcaoPagamento = new OpcaoCartaoDebito();
            FocarTbComando();
        }

        private void PixClickHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.OpcaoPagamento = new OpcaoPix();
            FocarTbComando();
        }

        private void AcaoEscolherCartaoDebito()
        {
            ViewModel.OpcaoPagamento = new OpcaoCartaoDebito();
            FocarTbComando();
        }

        private void AcaoEscolherPix()
        {
            ViewModel.OpcaoPagamento = new OpcaoPix();
            FocarTbComando();
        }

        private void ClearClickHandler(object sender, RoutedEventArgs e)
        {
            AcaoLimparLancamentos();
        }

        private void AcaoLimparLancamentos()
        {
            var msg = "Continuar com a exclusão dos pagamentos lançados?";

            if (DialogBox.MostraConfirmacao(msg) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                ViewModel.LimparLancamentos();
                HideButtonConfirmar();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}