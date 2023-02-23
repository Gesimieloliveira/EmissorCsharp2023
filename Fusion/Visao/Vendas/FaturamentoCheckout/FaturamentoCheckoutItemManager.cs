using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fusion.Controles.Checkout;
using Fusion.Sessao;
using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.EditarItem;
using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.SolictaTotal;
using Fusion.Visao.Vendas.FaturamentoCheckout.Models;
using FusionCore.AutorizacaoOperacao.Autorizacao;
using FusionCore.Papeis.Enums;
using FusionCore.Vendas.Faturamentos;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.CapturarPeso;
using FusionWPF.SharedViews.AutorizarOperacao;

namespace Fusion.Visao.Vendas.FaturamentoCheckout
{
    public partial class FaturamentoCheckout
    {
        private void OnClickRemoverItemSelecionado(object sender, RoutedEventArgs e)
        {
            ViewModel.ItemSelecionado = ((Button)e.OriginalSource).Tag as FaturamentoCheckoutModels.Item;
            AcaoRemoverItemSelecionado();
        }

        private void OnClickEditarItemSelecionado(object sender, RoutedEventArgs e)
        {
            ViewModel.ItemSelecionado = ((Button)e.OriginalSource).Tag as FaturamentoCheckoutModels.Item;
            AcaoEditarItemSelecionado();
        }

        private void ControlCheckoutBox_OnCheckoutItemChanged(object sender, RoutedEventArgs e)
        {
            if (ControlCheckoutBox.CheckoutItem is null) return;
            FazerCheckoutItem(ControlCheckoutBox.CheckoutItem);
        }

        private void ControlCheckoutBox_OnCheckoutErrror(object sender, RoutedEventArgs e)
        {
            var ex = (Exception)e.OriginalSource;
            DialogBox.MostraAviso(ex.Message);
        }

        private void FazerCheckoutItem(CheckoutItem item)
        {
            try
            {
                item.Produto.NaoAtivoThrowInvalidOperation();

                if (item.CodigoBalanca)
                {
                    ViewModel.FaturarItem(item.Produto, item.Quantidade);
                    return;
                }

                if (item.Produto.ProdutoUnidadeDTO.SolicitarPeso)
                {
                    var childModel = new CapturarPesoBalancaContexto(item.Produto.Nome);
                    var childView = new CapturarPesoBalanca(SessaoSistema.Instancia.Preferencias, childModel);

                    childModel.PesoConfirmado += (s, peso) =>
                    {
                        ViewModel.FaturarItem(item.Produto, peso);
                        childView.Close(true);
                    };

                    AbrirChildWindow(childView);
                    return;
                }

                if (item.Produto.ProdutoUnidadeDTO.SolicitaTotalPdv)
                {
                    var childView = SolicitaTotalView.Criar();

                    childView.ViewModel.NomeItem = item.Produto.Nome;

                    childView.ViewModel.ConfirmouValor += (o, ctx) =>
                    {
                        var novaQuantidade = decimal.Round(ctx.ValorTotal / item.Produto.PrecoVenda, 3);
                        ViewModel.FaturarItem(item.Produto, novaQuantidade);
                    };

                    AbrirChildWindow(childView);
                    return;
                }

                ViewModel.FaturarItem(item.Produto, item.Quantidade);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            finally
            {
                Keyboard.Focus(ControlCheckoutBox);
            }
        }

        private void AcaoRemoverItemSelecionado()
        {
            const string msg = "Deseja realmente remover o item do faturamento?";
            var confirmou = DialogBox.MostraConfirmacao(msg) == MessageBoxResult.Yes;
            if (confirmou == false) return;

            try
            {
                var payloadItem = ViewModel.CriarPayloadItemExcluido();
                var usuarioAutorizar = new AutorizarUsuarioAdm(_sessaoSistema.SessaoManager);

                var autorizarCancelamento = new AutorizarOperacaoView(
                    _sessaoSistema.SessaoManager,
                    usuarioAutorizar,
                    _sessaoSistema.UsuarioLogado,
                    payloadItem.FaturamentoId.ToString(),
                    Permissao.EXCLUIR_ITEM_FATURAMENTO,
                    payloadItem,
                    ViewModel.RemoverItemSelecionado
                );

                autorizarCancelamento.ExecutarAcao();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            finally
            {
                Keyboard.Focus(ControlCheckoutBox);
            }
        }

        public void AcaoEditarItemSelecionado()
        {
            var childModel = ViewModel.CriarContextoEditarItem();
            var childView = new EidtarItemView(childModel);
            childView.ViewModel.EditadoSucesso += EditadoSucessoHandler;

            AbrirChildWindow(childView);

            void EditadoSucessoHandler(object sender, FaturamentoProduto item)
            {
                ViewModel.AtualizarFaturamento();
            }
        }
    }
}