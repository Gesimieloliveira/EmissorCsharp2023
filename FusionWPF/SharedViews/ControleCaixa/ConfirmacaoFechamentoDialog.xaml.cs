using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionWPF.SharedViews.ControleCaixa
{
    internal partial class ConfirmacaoFechamentoDialog
    {
        internal ConfirmacaoFechamentoDialog(ConfirmacaoFechamentoContexto contexto)
        {
            InitializeComponent();
            Contexto = contexto;
        }

        internal ConfirmacaoFechamentoContexto Contexto { get; set; }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = Contexto;
            TbValorConferido.Focus();
        }

        private void IsOpenChangedHandler(object sender, RoutedEventArgs e)
        {
        }

        private void ConfirmarSaldoClickHandler(object sender, RoutedEventArgs e)
        {
            var msg = $"Confrimar o fechamento com Saldo de {Contexto.ValorConferidoEmCaixa:C2} ?";

            if (!DialogBox.MostraDialogoDeConfirmacao(msg))
            {
                return;
            }

            try
            {
                Contexto.ConfirmarFechamento();
                Close(true);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}