using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FusionCore.BalancaIntegrada;
using FusionCore.Preferencias;
using OpenAC.Net.Balanca;

namespace FusionWPF.CapturarPeso
{
    public partial class CapturarPesoBalanca
    {
        private readonly PreferenciaSistemaService _preferencias;
        private bool _digitacaoAtiva;

        public CapturarPesoBalanca(PreferenciaSistemaService preferencias, CapturarPesoBalancaContexto viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            _preferencias = preferencias;
        }

        public CapturarPesoBalancaContexto ViewModel { get; private set; }

        private void PreviewKeyDownHandler(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Tab:
                    e.Handled = true;
                    return;
                case Key.Enter:
                    e.Handled = true;
                    AcaoConfirmarPeso();
                    return;
                case Key.F2:
                    e.Handled = true;
                    AcaoDigitarPeso();
                    break;
            }
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = ViewModel;
            SpAviso.Visibility = Visibility.Collapsed;
            TbPesoBalanca.Focus();

            if (_preferencias.Obter(Preferencias.Balanca.Checkout.UsarBalanca, false))
            {
                TryConnectCheckoutBalance();
                return;
            }

            AcaoDigitarPeso();
        }

        private async void TryConnectCheckoutBalance()
        {
            OpenBal balancaConectada = null;

            try
            {
                balancaConectada = await Task.Run(() =>
                {
                    try
                    {
                        var balanca = BalancaCheckoutFactory.ConfigurarSerial(_preferencias, monitorar: true);

                        balanca.AoLerPeso += AoLerPesoBalancaHandler;
                        balanca.Conectar();

                        if (balanca.Conectado)
                        {
                            return balanca;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    return null;
                });

                Closing += (s, ce) => balancaConectada?.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (balancaConectada?.Conectado == true) return;

            SpAviso.Visibility = Visibility.Visible;
            AcaoDigitarPeso();
        }

        private void AoLerPesoBalancaHandler(object sender, BalancaEventArgs e)
        {
            if (_digitacaoAtiva)
            {
                return;
            }

            ViewModel.PesoItem = e.Peso ?? 0.00M;

            switch (e.Peso)
            {
                case 0:
                case null:
                    ViewModel.TextoInformativo = "Coloque o item na balança";
                    break;
                case -1:
                    ViewModel.TextoInformativo = "Aguardando peso estabilizar...";
                    break;
                default:
                    ViewModel.TextoInformativo = "Peso estabilizado";
                    break;
            }
        }

        private void ClosingHandler(object sender, CancelEventArgs e)
        {
        }

        private void ConfirmarPesoHandler(object sender, RoutedEventArgs e)
        {
            AcaoConfirmarPeso();
        }

        private void AcaoConfirmarPeso()
        {
            TbPesoBalanca.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            ViewModel.ConfirmarPeso();
        }

        private void DigitarPesoHandler(object sender, RoutedEventArgs e)
        {
            AcaoDigitarPeso();
        }

        public void AcaoDigitarPeso()
        {
            TbPesoBalanca.IsReadOnly = false;
            TbPesoBalanca.Focus();
            TbPesoBalanca.SelectAll();

            _digitacaoAtiva = true;
            ViewModel.TextoInformativo = "DIGITE O PESO";
        }
    }
}