using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Compras.Precos
{
    public partial class GeradorPrecoVendaView
    {
        private GeradorPrecoVendaViewModel GetModel => DataContext as GeradorPrecoVendaViewModel;

        public GeradorPrecoVendaView(GeradorPrecoVendaViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            GetModel.Inicializar();
            TbLucroGeral.Focus();
        }

        private void KeyDown_Window(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    AplicaLucroGeral();
                    break;
                case Key.F3:
                    MantemLucroAtual();
                    break;
                case Key.F4:
                    MantemVendaAtual();
                    break;
                case Key.F5:
                    GravaPrecos();
                    break;
                case Key.Escape:
                    Close();
                    break;
            }
        }

        private void DataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            /*
            var dgRow = DataGridPrecos.ItemContainerGenerator.ContainerFromIndex(0) as DataGridRow;

            if (dgRow == null)
            {
                return;
            }

            var presenter = GetVisualChild<DataGridCellsPresenter>(dgRow);
            var idx = 0;

            while (true)
            {
                try
                {
                    var currentCell = presenter.ItemContainerGenerator.ContainerFromIndex(idx++) as DataGridCell;

                    if (currentCell != null && !currentCell.IsReadOnly && !currentCell.IsFocused)
                    {
                        Keyboard.Focus(currentCell);
                        e.Handled = true;
                        break;
                    }
                }
                catch
                {
                    break;
                }
            }
            */
        }

        private void DataGridCell_GotFocus(object sender, RoutedEventArgs e)
        {
            var cell = sender as DataGridCell;

            if (cell != null && !cell.IsEditing && !cell.IsReadOnly)
            {
                DataGridPrecos.BeginEdit();
            }
        }

        private void DataGridCell_KeyDown(object sender, KeyEventArgs e)
        {
            var cell = sender as DataGridCell;

            if (e.Key != Key.Enter || cell == null)
            {
                return;
            }

            e.Handled = true;

            var nextIndex = DataGridPrecos.SelectedIndex + 1;
            var maxIndex = DataGridPrecos.Items.Count - 1;

            if (nextIndex <= maxIndex)
            {
                var focusedElement = Keyboard.FocusedElement as UIElement;
                focusedElement?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                DataGridPrecos.SelectedIndex = nextIndex;
                DataGridPrecos.BeginEdit();
                return;
            }

            Keyboard.ClearFocus();
            DataGridPrecos.SelectedIndex = -1;
            BtnSalvar.Focus();
        }

        private void KeyDown_TbLucroGeral(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            AplicaLucroGeral();
            TbLucroGeral.SelectAll();
            e.Handled = true;
        }

        private void AplicaLucroGeral()
        {
            if (DialogBox.MostraConfirmacao("Aplicar lucro tem todos os itens?") != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                TbLucroGeralUpdateSource();
                GetModel.AplicaLucroGeral();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        private void TbLucroGeralUpdateSource()
        {
            var binding = TbLucroGeral.GetBindingExpression(TextBox.TextProperty);
            binding?.UpdateSource();
        }

        private void AplicarLucroHandler(object sender, RoutedEventArgs e)
        {
            AplicaLucroGeral();
        }

        private void ManterLucroHandler(object sender, RoutedEventArgs e)
        {
            MantemLucroAtual();
        }

        private void MantemLucroAtual()
        {
            if (DialogBox.MostraConfirmacao("Manter lucro atual em todos os itens?") != MessageBoxResult.Yes)
            {
                return;
            }

            GetModel.MantemLucroAtual();
        }

        private void ManterVendaHandler(object sender, RoutedEventArgs e)
        {
            MantemVendaAtual();
        }

        private void MantemVendaAtual()
        {
            if (DialogBox.MostraConfirmacao("Manter venda atual em todos os itens?") != MessageBoxResult.Yes)
            {
                return;
            }

            GetModel.MantemVendaAtual();
        }

        private void GravarPrecosHandler(object sender, RoutedEventArgs e)
        {
            GravaPrecos();
        }

        private void GravaPrecos()
        {
            if (DialogBox.MostraConfirmacao("Salvar as alterações de preços?") != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                GetModel.SalvarAlteracoes();
                DialogBox.MostraInformacao("Consegui salvar os preços com sucesso :)");
                Close();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void FecharHandler(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}