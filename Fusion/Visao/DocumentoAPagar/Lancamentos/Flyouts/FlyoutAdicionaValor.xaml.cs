using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.DocumentoAPagar.Lancamentos.Flyouts
{
    public partial class FlyoutAdicionaValor
    {
        public FlyoutAdicionaValor()
        {
            InitializeComponent();
        }

        public FlyoutAdicionaValorModel Contexto => DataContext as FlyoutAdicionaValorModel;

        private void OnClickAdicionarPagamento(object sender, RoutedEventArgs e)
        {
            if (Contexto.MarcarComoQuitado && !ConfirmacaoDeQuitacao())
            {
                return;
            }

            try
            {
                Contexto.AdicionarPagamento();
                DialogBox.MostraInformacao("Pagamento adicionado ao documento!");
            }
            catch (Exception ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private bool ConfirmacaoDeQuitacao()
        {
            if (Contexto.ValorPagamentoMenorQueRestante())
            {
                return DialogBox.MostraDialogoDeConfirmacao(
                    "Documento será quitado e um lançamento de desconto da diferença será criado!");
            }

            if (Contexto.ValorPagamentoMaiorQueRestante())
            {
                return DialogBox.MostraDialogoDeConfirmacao(
                    "Documento será quitado e um lançamento de juros com a diferença será criado!");
            }

            return true;
        }
    }
}
