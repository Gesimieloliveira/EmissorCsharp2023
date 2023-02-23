using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FusionLibrary.Wpf.Componentes;
using FusionWPF.Base.Utils.Dialogs;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.Base.Grid
{
    public partial class GridPadrao
    {
        private readonly IGridPadraoModel _gridPadraoModel;

        public GridPadrao(IGridPadraoModel gridPadraoModel)
        {
            _gridPadraoModel = gridPadraoModel;
            DataContext = _gridPadraoModel;

            InitializeComponent();
            MontarLinhasDaGrid();
        }

        private void MontarLinhasDaGrid()
        {
            var starWidth = new DataGridLength(1.0, DataGridLengthUnitType.Star);
            var colunas = _gridPadraoModel.ColunasDaGrid();

            if (_gridPadraoModel.MostraBotaoOpcoes == false)
            {
                DataGrid.Columns.RemoveAt(0);
            }

            foreach (var coluna in colunas)
            {
                if (coluna.Width.IsAuto)
                {
                    coluna.Width = starWidth;
                }

                DataGrid.Columns.Add(coluna);
            }
        }

        private void ListaGenerica_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _gridPadraoModel.Loaded();
                _gridPadraoModel.PopularLista();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro(ex.Message, ex);
            }
        }

        private void ClickBtnNovoHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                var janela = _gridPadraoModel.JanelaNovo();

                if (janela == null)
                    return;

                janela.ShowDialog();

                _gridPadraoModel.PopularLista();
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro(ex.Message, ex);
            }
        }

        private async void ClickBtnFiltroHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                var janela = _gridPadraoModel.JanelaFiltro();

                if (janela == null)
                    return;

                await ((MetroWindow)Application.Current.Windows[0]).ShowChildWindowAsync(janela, ChildWindowManager.OverlayFillBehavior.FullWindow);
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro(ex.Message, ex);
            }
        }

        private void OnSearch(object sender, RoutedEventArgs e)
        {
            try
            {
                var componentePesquisa = sender as TextBoxPesquisa;

                if (componentePesquisa == null)
                {
                    MessageBox.Show("Não foi possível obter o texto de pesquisa.");
                    return;
                }

                _gridPadraoModel.UltimoTextoPesquisado = string.IsNullOrEmpty(componentePesquisa.Texto)
                    ? null
                    : componentePesquisa.Texto;

                _gridPadraoModel.AplicarPesquisa(_gridPadraoModel.UltimoTextoPesquisado);
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro(ex.Message, ex);
            }
        }

        private void DoubleClickDataGridRow(object sender, MouseButtonEventArgs e)
        {
            if (!_gridPadraoModel.EditaRegistro) return;

            try
            {
                var janela = _gridPadraoModel.JanelaAlterar();

                if (janela == null)
                {
                    return;
                }

                janela.ShowDialog();

                if (_gridPadraoModel.AutoReload)
                {
                    _gridPadraoModel.PopularLista();
                }

            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro(ex.Message, ex);
            }
        }

        private void ClickBtnOpcoesHandler(object sender, RoutedEventArgs e)
        {
            MostrarJanelaOpcoes();
        }

        private void MostrarJanelaOpcoes()
        {
            if (!_gridPadraoModel.MostraBotaoOpcoes)
            {
                return;
            }

            try
            {
                var janela = _gridPadraoModel.JanelaOpcoes();

                if (janela == null)
                {
                    DialogBox.MostraAviso("Janela de opções não disponível.");
                    return;
                }

                var showDialog = janela.ShowDialog();

                if (showDialog == true && _gridPadraoModel.AutoReload)
                {
                    _gridPadraoModel.PopularLista();
                }
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro(ex.Message, ex);
            }
        }
    }
}