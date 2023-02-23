using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fusion.Helpers;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MdfeEletronico
{
    public partial class GridMdfe
    {
        public readonly GridMdfeModel GridMdfeModel;

        public GridMdfe()
        {
            GridMdfeModel = new GridMdfeModel();
            DataContext = GridMdfeModel;
            InitializeComponent();
            FiltroHelper.RegitrarAtalhoFiltro(PainelFiltro, BotaoFiltro);

        }

        private void GridMdfe_OnLoaded(object sender, RoutedEventArgs e)
        {
            GridMdfeModel.AplicarPesquisa();
        }

        private void DoubleClickDataGridRow(object sender, MouseButtonEventArgs e)
        {   
            try
            {
                GridMdfeModel.Editar();
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

        private void ClickBtnOpcoesHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                GridMdfeModel.Opcoes();
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
            if (GridMdfeModel.Selecionado != null)
                GridMdfeModel.Selecionado.IsSelecionado = true;
        }

        private void AtualizarNotCheckedSelecionadoHandler(object sender, RoutedEventArgs e)
        {
            if (GridMdfeModel.Selecionado != null)
                GridMdfeModel.Selecionado.IsSelecionado = false;
        }

        private void AplicarFiltroClickHandler(object sender, RoutedEventArgs e)
        {
            GridMdfeModel.AplicarPesquisa();
        }

        private void ClickCopyChave(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string chave)
            {
                Clipboard.SetText(chave);
            }
        }
    }
}
