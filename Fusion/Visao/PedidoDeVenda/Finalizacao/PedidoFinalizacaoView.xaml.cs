using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Fusion.Parcelamento;
using Fusion.Visao.PedidoDeVenda.Finalizacao.MeioPagamento;
using FusionCore.FusionAdm.PedidoVenda;
using FusionCore.Helpers.Binding;
using FusionCore.Sessao;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Controles;

namespace Fusion.Visao.PedidoDeVenda.Finalizacao
{
    public partial class PedidoFinalizacaoView
    {
        private readonly FusionWindow _owner;
        private readonly ISessaoManager _sessaoManager;

        public PedidoFinalizacaoView(PedidoVenda pedido, FusionWindow owner, ISessaoManager sessaoManager)
        {
            _owner = owner;
            _sessaoManager = sessaoManager;

            Contexto = new PedidoFinalizacaoContexto(pedido, sessaoManager);
            Contexto.PropertyChanged += PropertyChangedHandler;

            InitializeComponent();
        }

        public PedidoFinalizacaoContexto Contexto { get; }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            Contexto.CarregarDadosDoPedido();
            CntrConfirmacao.Visibility = Visibility.Collapsed;

            DataContext = Contexto;
            AcaoEscolhaDinheiro();
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PedidoFinalizacaoContexto.TemPagamento))
            {
                PropertyTemPagamentoChangedHandler();
            }
        }

        private void PropertyTemPagamentoChangedHandler()
        {
            BtnDinheiro.IsEnabled = !Contexto.TemPagamento;
            BtnPrazo.IsEnabled = !Contexto.TemPagamento;
            BtnCCredito.IsEnabled = !Contexto.TemPagamento;
            BtnCDebito.IsEnabled = !Contexto.TemPagamento;

            BlockAcoes.Visibility = Contexto.TemPagamento
                ? Visibility.Collapsed
                : Visibility.Visible;
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

                Contexto.AdicionarNegociacao(resultado.Negociacao);

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

        private void AcaoEscolhaDinheiro()
        {
            Contexto.OpcaoPagamento = new OpcaoDinheiro();
            TbComando.Focus();
        }

        private void ClickCrediarioHandler(object sender, RoutedEventArgs e)
        {
            AcaoEscolhaPrazo();
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

        private void ClickCartaoCreditoHandler(object sender, RoutedEventArgs e)
        {
            AcaoEscolhaCartaoCredito();
        }

        private void AcaoEscolhaCartaoCredito()
        {
            Contexto.OpcaoPagamento = new OpcaoCartaoCredito();
            TbComando.Focus();
        }

        private void ClickCartaoDebitoHandler(object sender, RoutedEventArgs e)
        {
            AcaoEscolhaCartaoDebito();
        }

        private void AcaoEscolhaCartaoDebito()
        {
            Contexto.OpcaoPagamento = new OpcaoCartaoDebito();
            TbComando.Focus();
        }
    }
}