using System;
using System.Windows;
using System.Windows.Input;
using Fusion.Facades;
using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Finalizacao;
using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Opcoes;
using FusionCore.Vendas.Faturamentos;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Vendas.FaturamentoCheckout
{
    public partial class FaturamentoCheckout
    {
        private void OnClickFinalizar(object sender, RoutedEventArgs e)
        {
            AcaoFinalizar();
        }

        private void AcaoFinalizar()
        {
            try
            {
                if (ViewModel.Vendedor.Id == null)
                {
                    VendedorObrigatorioFacade.ThrowExceptionSeVendedorObrigatorio();
                }

                var view = new PagamentoView(this, ViewModel.CriarContextoPagamento());
                view.ViewModel.FinalizacaoConcluida += FinalizacaoConcluidaHandler;
                AbrirChildWindow(view);
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
            finally
            {
                Keyboard.Focus(ControlCheckoutBox);
            }
        }

        private void FinalizacaoConcluidaHandler(object sender, FaturamentoVenda faturamento)
        {
            ViewModel.LimparViewModel();

            var preferencias = ViewModel.Preferencias.GetPreferenciaDaMaquina();
            if (preferencias is null)
            {
                return;
            }

            if (preferencias.ImprimirFinalizacao)
            {
                _impressor.Imprime(faturamento, preferencias);
            }

            if (preferencias.PreVisualizar)
            {
                _impressor.Viazualiza(faturamento, preferencias);
            }

            if (preferencias.DesabilitarTelaOpcoes != true)
            {
                var opcoesView = new OpcoesFaturamentoView(ViewModel.Preferencias, _impressor, faturamento);
                AbrirChildWindow(opcoesView);
            }
        }
    }
}