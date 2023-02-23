using System.Windows;
using System.Windows.Input;
using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.ListarAbertos;
using FusionCore.Vendas.Faturamentos;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Vendas.FaturamentoCheckout
{
    public partial class FaturamentoCheckout
    {
        private void OnClickAbrirListagem(object sender, RoutedEventArgs args)
        {
            AcaoListarFaturamentos();
        }

        private void AcaoListarFaturamentos()
        {
            const string msg = "Você já possui um faturamento aberto, continuar com a troca?";

            if (ViewModel.PossuiFaturamento && DialogBox.MostraConfirmacao(msg) != MessageBoxResult.Yes)
            {
                Keyboard.Focus(ControlCheckoutBox);
                return;
            }

            var childModel = new ListarAbertosViewModel(_sessaoSistema.SessaoManager);
            var childView = new ListarAbertosView(childModel);

            childView.ViewModel.Impressao += OnImpressaoHandler;
            childView.ViewModel.Selecionado += OnSelecionadoHandler;

            AbrirChildWindow(childView);

            #region events

            void OnImpressaoHandler(object s, FaturamentoSlim e)
            {
                _impressor.Imprime(e, ViewModel.Preferencias.GetPreferenciaDaMaquina());
            }

            void OnSelecionadoHandler(object s, FaturamentoSlim e)
            {
                if (e.EstadoAtual != Estado.Aberto)
                {
                    DialogBox.MostraAviso("Preciso que selecione um faturamento aberto!");
                    return;
                }

                ViewModel.CarregarComFaturamento(e.Id);
                childView.Close(true);
            }

            #endregion
        }
    }
}