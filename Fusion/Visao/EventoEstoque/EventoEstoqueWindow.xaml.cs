using System;
using System.IO;
using System.Windows;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.EventoEstoque
{
    public partial class EventoEstoqueWindow
    {
        private readonly ProdutoDTO _produtoDTO;
        private readonly EventoEstoqueWindowModel _viewModel;

        public EventoEstoqueWindow(ProdutoDTO produtoDTO)
        {
            InitializeComponent();
            _produtoDTO = produtoDTO;
            _viewModel = new EventoEstoqueWindowModel();
            DataContext = _viewModel;
        }

        private void EventoEstoqueWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.PreencherView(_produtoDTO);
            }
            catch (InvalidDataException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}