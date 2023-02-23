using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fusion.Visao.Compras.NotaFiscal.Opcoes;
using FusionCore.FusionAdm.Compras;
using FusionWPF.Base.Utils.Dialogs;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.Compras.NotaFiscal
{
    public partial class NotaFiscalCompraView
    {
        private TotaisCompraChildViewModel _childModel;

        public NotaFiscalCompraView(NotaFiscalCompra nota = null)
        {
            var model = new NotaFiscalCompraViewModel(nota);
            model.Fechar += Fechar;
            DataContext = model;
            InitializeComponent();
        }

        private NotaFiscalCompraViewModel GetModel => DataContext as NotaFiscalCompraViewModel;

        private void Fechar(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadeHandler(object sender, RoutedEventArgs e)
        {
            DropDownOpcoes.ItemsSource = new List<IOutraOpcao>
            {
                new GerarPrecoVenda(),
                new GerarDocumentosApagar(this),
                new EstornarContasApagar(),
            };

            GetModel.Inicializar();
            CbEmpresa.Focus();
        }

        private void ClickSalvarNotaHandler(object sender, RoutedEventArgs e)
        {
            GetModel.SalvarAlteracoes();
        }

        private void NovoItemHandler(object sender, RoutedEventArgs e)
        {
            if (!GetModel.NotaEstaSalva)
            {
                DialogBox.MostraAviso("Preciso que salve a nota para poder adicionar itens");
                return;
            }

            if (GetModel.PossuiFinanceiro)
            {
                DialogBox.MostraInformacao("Preciso que estorne o financeiro para poder adicionar itens");
                return;
            }

            var vm = new CompraItemViewModel(GetModel.GetNota());
            var view = new CompraItemView(vm);
            view.ShowDialog();

            GetModel.RefreshNota();
        }

        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2 && _childModel?.IsOpen != true)
            {
                AbreChildWindowTotais();
            }
        }

        private async void AbreChildWindowTotais()
        {
            _childModel = new TotaisCompraChildViewModel(GetModel.GetNota())
            {
                IsOpen = true
            };
            var view = new TotaisCompraChildView(_childModel);

            _childModel.ConfirmouValores += ConfirmouValores;
            await this.ShowChildWindowAsync(view, ChildWindowManager.OverlayFillBehavior.FullWindow);
        }

        private void ConfirmouValores(object sender, TotaisCompraChildViewModel e)
        {
            GetModel.AlteraValoresNota(e);
            e.IsOpen = false;
        }

        private void RemoveItemHanlder(object sender, RoutedEventArgs e)
        {
            if (DialogBox.MostraConfirmacao("Remover este item?") != MessageBoxResult.Yes)
            {
                return;
            }

            GetModel.RemoveItemSelecionado();
        }

        private void DoubleClickItemHandler(object sender, MouseButtonEventArgs e)
        {
            var vm = new CompraItemViewModel(GetModel.GetNota(), GetModel.ItemSelecionado);
            var view = new CompraItemView(vm);
            view.ShowDialog();

            GetModel.RefreshNota();
        }

        private void ClickExcluirHandler(object sender, RoutedEventArgs e)
        {
            if (DialogBox.MostraConfirmacao("Processo de exclusão é irreversivel. Deseja continuar?") != MessageBoxResult.Yes)
            {
                return;
            }

            if (GetModel.PossuiFinanceiro)
            {
                const string msg = "Encontrei financeiro para essa nota, o financeiro será estornado. Deseja continuar?";

                if (DialogBox.MostraConfirmacao(msg) != MessageBoxResult.Yes)
                {
                    return;
                }
            }

            try
            {
                GetModel.ExcluiNotaCompra();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void OutraOpcoesItemClickHandler(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is MenuItem mi && mi.DataContext is IOutraOpcao op)
            {
                op.ExeuctaAcao(GetModel);
            }
        }
    }
}