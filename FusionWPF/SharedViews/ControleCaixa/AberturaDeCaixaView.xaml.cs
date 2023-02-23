using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionWPF.SharedViews.ControleCaixa
{
    public partial class AberturaDeCaixaView
    {
        public AberturaDeCaixaView(AberturaDeCaixaContexto contexto)
        {
            InitializeComponent();
            Contexto = contexto;
        }

        public AberturaDeCaixaContexto Contexto { get; }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            Contexto.Inicializar();
            DataContext = Contexto;

            TbSaldoInicial.Focus();
        }

        private void AbrirNovoCaixaClickHandler(object sender, RoutedEventArgs e)
        {
            var aviso =
                $"Continuar com a abertura de caixa para {Contexto.OperadorCaixa}" +
                $" com valor inicial de {Contexto.SaldoInicial}?";

            if (!DialogBox.MostraDialogoDeConfirmacao(aviso))
            {
                return;
            }

            try
            {
                Contexto.FazerAberturaDeCaixa();

                DialogBox.MostraInformacao("Caixa aberto com sucesso!");
                DialogResult = true;
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}