using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FusionCore.Helpers.Binding;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Utils;
using Xceed.Wpf.AvalonDock.Controls;

namespace FusionWPF.Parcelamento
{
    public partial class ParcelamentoDialog
    {
        public ParcelamentoDialog(IParcelamentoFactory factory, decimal valorParcelar)
        {
            Contexto = new ParcelamentoContexto(factory)
            {
                ValorParcelar = valorParcelar
            };

            InitializeComponent();
        }

        public ParcelamentoContexto Contexto { get; }

        private void RootKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (IsEnabled == false)
            {
                return;
            }

            if (e.Key == Key.F2)
            {
                e.Handled = true;
                AcaoConfirmar();
            }

            if (e.Key == Key.F5)
            {
                AcaoEditarParcelas();
                e.Handled = true;
            }

            if (e.Key == Key.F6)
            {
                AcaoFocarNumeroParcelas();
                e.Handled = true;
            }
        }

        private void BorderParcelasMouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            if (Contexto.ParcelasIsEnabled)
            {
                return;
            }

            AcaoEditarParcelas();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            Contexto.CarregarDados();

            DataContext = Contexto;
            CbTipoDocumento.Focus();
        }

        private void TbQuantidadePKeyDownHandler(object sender, KeyEventArgs e)
        {
            TbQtdeParcelas.UpdateBindingText();

            if (e.Key != Key.Enter)
            {
                return;
            }

            e.Handled = true;

            try
            {
                Contexto.ParcelasIsEnabled = false;
                Contexto.GerarParcelas();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            finally
            {
                AcaoFocarNumeroParcelas();
            }
        }

        private void AcaoFocarNumeroParcelas()
        {
            TbQtdeParcelas.Focus();
            TbQtdeParcelas.SelectAll();
            Contexto.ParcelasIsEnabled = false;
        }

        private void AcaoConfirmar()
        {
            BtnConfirmar.Focus();

            try
            {
                Contexto.ParcelasIsEnabled = false;
                Contexto.ConfirmarParcelamento();

                Close(true);
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void AcaoEditarParcelas()
        {
            Contexto.ParcelasIsEnabled = true;

            LbParcelas.FindFirstItem()?.Focus();
        }

        private void ParcelaItemGotFocusHandler(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is ListBoxItem lbItem))
            {
                return;
            }

            foreach (var dp in lbItem.FindVisualChildren<TextBox>())
            {
                Keyboard.Focus(dp);
                e.Handled = true;
                return;
            }
        }

        private void ConfirmarClickHandler(object sender, RoutedEventArgs e)
        {
            AcaoConfirmar();
        }
    }
}