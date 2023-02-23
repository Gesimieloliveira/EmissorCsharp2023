using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fusion.Helpers;
using Fusion.Visao.PedidoDeVenda;
using Fusion.Visao.SelecionarNfce;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.NotaFiscalEletronica.Principal
{
    public partial class NfeEletronicaGridControl
    {
        private readonly PedidoVendaController _pedidoVendaController;
        private readonly NfeletronicaGridModel _contexto;

        public NfeEletronicaGridControl(
            NfeletronicaGridModel model, 
            PedidoVendaController pedidoVendaController
        ) {
            _contexto = model;
            _pedidoVendaController = pedidoVendaController;

            InitializeComponent();
            FiltroHelper.RegitrarAtalhoFiltro(PainelFiltro, BotaoFiltro);

        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _contexto.Inicializar();

            DataContext = _contexto;
        }

        private void DoubleClickDataGridRow(object sender, MouseButtonEventArgs e)
        {
            _contexto.EditarSelecionada();
        }

        private void ClickNovoHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                _contexto.NovaNota();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void ClickOpcoesHandler(object sender, RoutedEventArgs e)
        {
            _contexto.OpcoesDaNota();
        }

        private void ClickCopyChave(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string chave)
            {
                Clipboard.SetText(chave);
            }
        }

        private void AplicarFiltroClickHandler(object sender, RoutedEventArgs e)
        {
            _contexto.AplicarFiltro();
        }

        private void FaturarPedidoClickHandler(object sender, RoutedEventArgs e)
        {
            _pedidoVendaController.DialogListagemImportacao();
        }

        private void ConverteNfceClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                bool existePerfilNormal;

                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var repositorio = new RepositorioPerfilNfe(sessao);
                    existePerfilNormal = repositorio.ExistePerfilNfeSaidaNormal();
                }

                if (existePerfilNormal == false)
                    throw new InvalidOperationException("Não existe perfil nf-e normal.\nCadastre um perfil nf-e.");

                var modelo = new SelecionadorNfceFormularioModelo();
                new SelecionadorNfceFormulario(modelo).ShowDialog();
                _contexto.AplicarFiltro();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }
    }
}