using System.Windows;
using FusionCore.ControleCaixa;
using FusionWPF.Base.Utils.Dialogs;
using MahApps.Metro.SimpleChildWindow;

namespace FusionWPF.SharedViews.ControleCaixa
{
    public partial class ResumoCaixaIndividualView
    {
        public ResumoCaixaIndividualView(ResumoCaixaIndividualContexto contexto)
        {
            InitializeComponent();
            Contexto = contexto;
        }

        public ResumoCaixaIndividualContexto Contexto { get; }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = Contexto;
            Contexto.Inicializar();
        }

        private void FecharCaixaClickHandler(object sender, RoutedEventArgs e)
        {
            if (Contexto.CaixaIndividual.EstadoAtual == EEstadoCaixa.Fechado)
            {
                DialogBox.MostraAviso("Caixa já está fechado!");
                return;
            }

            if (Contexto.CaixaIndividual.LocalEvento != Contexto.CaixaProvider.GetLocalEvento())
            {
                DialogBox.MostraAviso("Caixa só pode ser fechado no local de abertura!");
                return;
            }

            var ctxConfirmacao = new ConfirmacaoFechamentoContexto
            {
                ValorEmDinheiroCalculado = Contexto.TotalizarSaldoEsperadoEmDinheiro()
            };

            var dialog = new ConfirmacaoFechamentoDialog(ctxConfirmacao);

            dialog.Contexto.ConfirmouOperacao += (o, args) =>
            {
                Contexto.FecharCaixa(ctxConfirmacao.ValorConferidoEmCaixa);
                DialogBox.MostraInformacao("Caixa foi fechado com sucesso!");
                Close();
            };

            this.ShowChildWindowAsync(dialog);
        }
    }
}