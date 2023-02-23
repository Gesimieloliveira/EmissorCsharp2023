using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FusionLibrary.Helper.Diversos;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionNfce.Visao.Principal.SolicitaTotal
{
    public partial class SolicitaTotalForm
    {
        private readonly SolicitaTotalContexto _contexto;

        public SolicitaTotalForm(SolicitaTotalContexto contexto)
        {
            _contexto = contexto;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;

            try
            {
                DataContext = _contexto;
            }
            finally
            {
                IsEnabled = true;

                TbValorTotal.Focus();
                TbValorTotal.SelectAll();
            }
        }

        private void ValorTotalChangedHandler(object sender, TextChangedEventArgs e)
        {
            if (IsEnabled == false)
            {
                return;
            }

            var input = DecimalHelper.ConverteFrom(TbValorTotal.Text);

            _contexto.CalcularQuantidade(input);
        }

        private void ValorTotalKeyDownHandler(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    e.Handled = true;
                    return;
                }

                if (e.Key == Key.Enter)
                {
                    _contexto.FianlizarSolicitacao();
                }
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}
