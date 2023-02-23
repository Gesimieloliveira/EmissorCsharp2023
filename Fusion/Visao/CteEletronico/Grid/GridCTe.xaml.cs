using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fusion.Helpers;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronico.Grid
{
    public partial class GridCTe
    {
        private readonly GridCTeModel _gridCTeModel;

        public GridCTe()
        {
            _gridCTeModel = new GridCTeModel();
            DataContext = _gridCTeModel;
            InitializeComponent();
            FiltroHelper.RegitrarAtalhoFiltro(PainelFiltro, BotaoFiltro);
        }

        private void DoubleClickDataGridRow(object sender, MouseButtonEventArgs e)
        {
            try
            {
                _gridCTeModel.Editar();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void ClickBtnOpcoesHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                _gridCTeModel.Opcoes();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void AtualizarCheckSelecionadoHandler(object sender, RoutedEventArgs e)
        {
            if (_gridCTeModel.Selecionado != null)
                _gridCTeModel.Selecionado.IsSelecionado = true;
        }

        private void GridCte_OnLoaded(object sender, RoutedEventArgs e)
        {
            _gridCTeModel.InicializarPermissoes();
            _gridCTeModel.BuscarCtesGrid(new CteFiltroGridDto());
        }

        private void AtualizarNotCheckedSelecionadoHandler(object sender, RoutedEventArgs e)
        {
            if (_gridCTeModel.Selecionado != null)
                _gridCTeModel.Selecionado.IsSelecionado = false;
        }

        private void CopiarChave(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string chave)
            {
                Clipboard.SetText(chave);
            }
        }
    }
}
