using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Fusion.Sessao;
using Fusion.Visao.NotaFiscalEletronica.Principal.Aba.Models;
using Fusion.Visao.NotaFiscalEletronica.Principal.ExceptionCustom;
using Fusion.Visao.NotaFiscalEletronica.Principal.Finalizar;
using FusionCore.AutorizacaoOperacao.Autorizacao;
using FusionCore.AutorizacaoOperacao.PayloadTypes;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Papeis.Enums;
using FusionCore.Sessao;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Controles;
using FusionWPF.SharedViews.AutorizarOperacao;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Aba
{
    public partial class AbaItensNfe
    {
        private FusionWindow _window;

        public AbaItensNfe()
        {
            InitializeComponent();
        }

        private AbaItensNfeModel ViewModel => DataContext as AbaItensNfeModel;

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _window = this.TryFindParent<FusionWindow>();
        }

        private void CatchException(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
            catch (InvalidDataException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
            catch (PagamentoNfeException ex)
            {
                DialogBox.MostraAviso(ex.Message);
                ClickPagamentoHandler(null, null);
            }
            catch (Exception e)
            {
                DialogBox.MostraErro(e.Message, e);
            }
        }

        private void ClickReferenciasNfeHandler(object sender, RoutedEventArgs e)
        {
            CatchException(ViewModel.OnReferenciarNfeCalled);
        }

        private void OnClickBotaoAnterior(object sender, RoutedEventArgs e)
        {
            CatchException(ViewModel.OnPassoAnteriorCalled);
        }

        private void OnClickEmiteNfe(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Nfe.SemPagamento == false
                && (ViewModel.Nfe.FinalidadeEmissao == FinalidadeEmissao.Ajuste
                    || ViewModel.Nfe.FinalidadeEmissao == FinalidadeEmissao.Devolucao)
            )
            {
                DialogBox.MostraAviso("Para emitir uma NF-e de Ajuste ou Devolução, você deve marcar sem pagamento em pagamento");
                return;
            }

            if (ViewModel.Nfe.PossuiPagamento() || ViewModel.Nfe.SemPagamento)
            {
                AcaoFinalizarNfe();
                return;
            }

            const string aviso = "Pagamento não foi definido, deseja finalizar em Dinheiro?";

            if (DialogBox.MostraConfirmacao(aviso) == MessageBoxResult.Yes)
            {
                AcaoFinalizarNfe();
                return;
            }

            AcaoGerenciarPagamento();
        }

        private void AcaoFinalizarNfe()
        {
            CatchException(ViewModel.OnEmiteNfeCalled);
        }

        private void ClickAdicionarItemHandler(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;

            try
            {
                ViewModel.Nfe.ChecarCobranca();

                while (true)
                {
                    var view = new ItemView();

                    view.ViewModel.PrepararCom(ViewModel.Nfe);

                    var result = view.ShowDialog();
                    ViewModel.AtualizaDadosView();

                    if (result != true)
                    {
                        break;
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            finally
            {
                IsEnabled = true;
            }
        }

        private void ItemDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ViewModel.Nfe.ChecarCobranca();

                var view = new ItemView();

                view.ViewModel.PrepararCom(ViewModel.ItemSelecionado.Nfe);
                view.ViewModel.ItemEdicao(ViewModel.ItemSelecionado);
                view.ShowDialog();

                ViewModel.AtualizaDadosView();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void ClickPagamentoHandler(object sender, RoutedEventArgs e)
        {
            AcaoGerenciarPagamento();
        }

        private void AcaoGerenciarPagamento()
        {
            var dialog = new NfePagamentoView(_window, ViewModel.Nfe, new SessaoManagerAdm());

            dialog.Contexto.PagamentoConcluido += (sender, nfe) =>
            {
                dialog.Close(true);
                AcaoFinalizarNfe();
            };

            _window.ShowChildWindowAsync(dialog);
        }

        private void ClickReferenciasCfHandler(object sender, RoutedEventArgs e)
        {
            CatchException(ViewModel.OnReferenciarCfCalled);
        }

        private void PreVisualizarDanfeClickHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.PreVisualizarDanfe();
        }

        private void ClickRemoveItemHandler(object sender, RoutedEventArgs e)
        {
            var payloadItem = new NfeItemExcluido(
                ViewModel.ItemSelecionado.Id,
                ViewModel.Nfe.Id,
                ViewModel.ItemSelecionado.Produto.Id,
                ViewModel.ItemSelecionado.Produto.Nome,
                ViewModel.ItemSelecionado.Quantidade,
                ViewModel.ItemSelecionado.ValorUnitario,
                ViewModel.ItemSelecionado.TotalItem);

            var sessao = SessaoSistema.Instancia.SessaoManager;

            var autorizarUsuario = new AutorizarUsuarioAdm(sessao);

            var autorizarRemoverItemNfe = new AutorizarOperacaoView(
                sessao,
                autorizarUsuario,
                SessaoSistema.Instancia.UsuarioLogado,
                ViewModel.Nfe.Id.ToString(), 
                Permissao.EXCLUIR_ITEM_NFE, 
                payloadItem,
                () => ActionRemoverItemNfe());

            autorizarRemoverItemNfe.ExecutarAcao();
        }

        private void ActionRemoverItemNfe()
        {
            ViewModel.RemoverItemSelelecioando();
        }

        private void ClickAlterarTotaisFixoHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.OnAlterarTotaisFixoCalled();
        }

        private void ClickEditarProdutoHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.AlterarProdutoItemSelecionado();
        }
    }
}