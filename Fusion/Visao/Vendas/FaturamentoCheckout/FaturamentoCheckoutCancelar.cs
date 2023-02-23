using System;
using System.Windows;
using System.Windows.Input;
using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Cancelamento;
using FusionCore.AutorizacaoOperacao.Autorizacao;
using FusionCore.Papeis.Enums;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.SharedViews.AutorizarOperacao;

namespace Fusion.Visao.Vendas.FaturamentoCheckout
{
    public partial class FaturamentoCheckout
    {
        private void OnClickCancelar(object sender, RoutedEventArgs e)
        {
            AcaoCancelarFaturamento();
        }

        private void AcaoCancelarFaturamento()
        {
            try
            {
                var payload = ViewModel.CriarPayloadCancelamento();
                var autorizarUsuario = new AutorizarUsuarioAdm(_sessaoSistema.SessaoManager);

                var autorizarCancelamento = new AutorizarOperacaoView(
                    _sessaoSistema.SessaoManager,
                    autorizarUsuario,
                    _sessaoSistema.UsuarioLogado,
                    payload.Id.ToString(),
                    Permissao.CANCELAR_FATURAMENTO,
                    payload,
                    ExecutarCancelamento
                );

                autorizarCancelamento.ExecutarAcao();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }

            void ExecutarCancelamento()
            {
                try
                {
                    var childModel = ViewModel.CriarContextoCancelar();
                    var childView = new CancelarFaturamentoView(childModel);

                    childModel.CanceladoSucesso += delegate
                    {
                        ViewModel.LimparViewModel();
                        childView.Close();
                    };

                    childView.ShowDialog();
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
        }
    }
}