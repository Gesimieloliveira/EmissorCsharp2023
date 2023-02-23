using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fusion.Helpers;
using Fusion.Sessao;
using Fusion.Visao.Pessoa.Picker;
using FusionCore.ControleCaixa.Facades;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Pessoas;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Controles;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.DocumentoAReceber
{
    public partial class GridGerenciarAhReceberView
    {
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;
        private readonly GridGerenciarAhReceberContexto _contexto;

        public GridGerenciarAhReceberView(GridGerenciarAhReceberContexto contexto)
        {
            _contexto = contexto;
            InitializeComponent();
            FiltroHelper.RegitrarAtalhoFiltro(PainelFiltro, BotaoFiltro);
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = _contexto;
            _contexto.CarregarDocumentos();
        }

        private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _contexto.TotalizarSelecionados(DgDocumentos.SelectedItems.Cast<ResumoDocumentoReceberDTO>());
        }

        private void QuitarSelecionadosClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                ControleCaixaGestorFacade.ThrowExcetpionSeNaoExistirCaixaAberto(_sessaoSistema.UsuarioLogado);


                if (DgDocumentos.SelectedItems.Count == 0)
                {
                    DialogBox.MostraAviso("Nenhum documento foi selecionado");
                    return;
                }

                var contexto = new QuitarSelecionadosContexto(_sessaoSistema);

                contexto.PrepararCom(DgDocumentos.SelectedItems.Cast<ResumoDocumentoReceberDTO>());

                var dialog = new QuitarSelecionadosView(contexto);

                dialog.ClosingFinished += DialogClosingFinished;

                ShowAsyncDialog(dialog);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void ShowAsyncDialog(ChildWindow dialog)
        {
            this.TryFindParent<FusionWindow>().ShowChildWindowAsync(dialog);
        }

        private void NovoRegistroClickHandler(object sender, RoutedEventArgs e)
        {
            var contexto = new FormularioDocumentoReceberContexto(_sessaoSistema);
            var dialog = new FormularioDocumentoReceberView(contexto);

            dialog.ClosingFinished += DialogClosingFinished;

            ShowAsyncDialog(dialog);
        }

        private void DialogClosingFinished(object sender, RoutedEventArgs e)
        {
            if (!(sender is ChildWindow c) || !(c.ChildWindowResult is bool result) || !result)
            {
                return;
            }

            _contexto.CarregarDocumentos();
        }

        private void DoubleClickRowHandler(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (!(sender is DataGridRow row) || !(row.DataContext is ResumoDocumentoReceberDTO item))
                {
                    return;
                }

                var contexto = new FormularioDocumentoReceberContexto(_sessaoSistema);

                contexto.CarregarDocumento(item.Id);

                var dialog = new FormularioDocumentoReceberView(contexto);

                dialog.ClosingFinished += DialogClosingFinished;

                ShowAsyncDialog(dialog);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void AplicarFiltroClickHandler(object sender, RoutedEventArgs e)
        {
            _contexto.CarregarDocumentos();
        }

        private void FiltroClienteClickHandler(object sender, RoutedEventArgs e)
        {
            var picker = new PessoaPickerModel(new ClienteEngine());
            picker.PickItemEvent += (o, args) => _contexto.Filtro.ClienteIgual = args.GetItem<Cliente>();
            picker.GetPickerView().ShowDialog();
        }

        private void ImprimirDocumentosClickHandler(object sender, RoutedEventArgs e)
        {
            var contexto = new ImpressaoDocumentosAhReceberContexto();
            var dialog = new ImpressaoDocumentosAhReceberView(contexto);

            ShowAsyncDialog(dialog);
        }

        private void GerarVariosClickHandler(object sender, RoutedEventArgs e)
        {
            var contexto = new NovoDocumentoReceberParceladoContexto(_sessaoSistema);
            var dialog = new NovoDocumentoReceberParcelado(contexto);

            dialog.ClosingFinished += DialogClosingFinished;

            ShowAsyncDialog(dialog);
        }

        private void ClearFiltroClienteClickHandler(object sender, RoutedEventArgs e)
        {
            _contexto.Filtro.ClienteIgual = null;
        }
    }
}