using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FusionWPF.Base.GridPicker.Contrato;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Utils;

namespace FusionWPF.Base.GridPicker
{
    public partial class GridPicker
    {
        private bool _estaCarregando;
        private GridPickerModel ViewModel => DataContext as GridPickerModel;

        public GridPicker(GridPickerModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
            ViewModel.CloseRequest += (s, e) => { Close(); };
            ViewModel.InicializarFiltro();
            AdicionaConteudoFiltroSeTiver();
        }

        private void PreviewKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (_estaCarregando)
            {
                return;
            }

            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                Close();
            }
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _estaCarregando = true;

            try
            {
                ViewModel.Inicializar();
                SearchTextBox.Focus();
            }
            finally
            {
                _estaCarregando = false;
            }
        }

        private void AdicionaConteudoFiltroSeTiver()
        {
            var userControlFiltro = ViewModel.FiltroFlayout();

            if (userControlFiltro != null)
            {
                FlyoutGridPickerFiltro.ConteinerFlyout.Children.Add(userControlFiltro);
            }
        }

        private void PickerEventHandler(object sender, GridPickerEventArgs e)
        {
            Close();
        }        

        private void DoubleClickItemHandler(object sender, MouseButtonEventArgs e)
        {
            ViewModel.SelecionarItem();
        }

        private void ClickBtnItemHandler(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button))
            {
                DialogBox.MostraAviso("Não consegui identificar este item! Pode tentar novamente?");
                return;
            }

            ViewModel.ItemSelecionado = (IGridPickerItem) button.Tag;
            ViewModel.SelecionarItem();
        }

        private void ClickBtnEditarItemHandler(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button))
            {
                DialogBox.MostraAviso("Não consegui identificar este item! Pode tentar novamente?");
                return;
            }

            ViewModel.ItemSelecionado = (IGridPickerItem) button.Tag;
            ViewModel.EditarItemSelecionado();
        }

        private void SearchTextBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    e.Handled = true;
                    DispararEventoPesquisa();
                    break;

                case Key.Down:
                case Key.Tab:
                    e.Handled = true;
                    MoverFocoParaListagem();
                    break;
            }
        }

        private void MoverFocoParaListagem()
        {
            if (!ViewModel.ItensLista.Any())
            {
                return;
            }

            ListBoxItens.FocusFirstItem();
        }

        private void DispararEventoPesquisa()
        {
            ViewModel.AplicaPesquisa(ViewModel.TextoPesquisado);
        }

        private void PreviewKeyDownItemHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                ViewModel.SelecionarItem();
            }
        }
    }
}