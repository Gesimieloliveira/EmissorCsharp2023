using System;
using System.Windows;
using System.Windows.Input;
using Fusion.Base.Notificacoes;
using FusionCore.FusionAdm.Financeiro;
using FusionWPF.Controles;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.DocumentoAPagar
{
    public partial class GridDocumentoPagar
    {
        private readonly Notificador _notificador;
        private readonly GridDocumentoPagarModel _model;

        public event EventHandler ImprimirRecibo;
        public event EventHandler<DocumentoPagar> ImprimirComDocumento; 

        public GridDocumentoPagar(Notificador notificador)
        {
            _notificador = notificador;
            _model = new GridDocumentoPagarModel(notificador);
            _model.ImprimirRecibo += ImprimirReciboRepassa;
            _model.ImprimirReciboComDocumento += ImprimirReciboDocumento;

            InitializeComponent();
            DataContext = _model;

            notificador.Registrar("documentoPagarSalvo", AtualizarDadosGrid);
            notificador.Registrar("documentoPagarGerados", AtualizarDadosGrid);
            notificador.Registrar("documentoPagarEstornado", AtualizarDadosGrid);
            notificador.Registrar("documentoPagarQuitado", AtualizarDadosGrid);
        }

        private void GridDocumento_OnLoaded(object sender, RoutedEventArgs e)
        {
            _model.InicializaLoadded();
        }

        private void AtualizarDadosGrid(NotificacaoArgs obj)
        {
            _model.AplicarPesquisaAction();
        }

        private void ImprimirReciboDocumento(object sender, DocumentoPagar e)
        {
            OnImprimirComDocumento(e);
        }

        private void ImprimirReciboRepassa(object sender, EventArgs e)
        {
            OnImprimirRecibo();
        }

        private void DoubleClickDataGridRow(object sender, MouseButtonEventArgs e)
        {
            _model.EfetuarLancamentos();
        }

        private void ClickBtnOpcoesHandler(object sender, RoutedEventArgs e)
        {
            _model.AbrirJanelaOpcoes();
        }

        protected virtual void OnImprimirRecibo()
        {
            ImprimirRecibo?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnImprimirComDocumento(DocumentoPagar e)
        {
            ImprimirComDocumento?.Invoke(this, e);
        }

        private async void GerarDocumentosClick(object sender, RoutedEventArgs e)
        {
            var model = GerarContasPagarModelFactory.Criar(_notificador);
            var janela = new GerarContasPagar(model);

            await this.TryFindParent<FusionWindow>().ShowChildWindowAsync(janela);
        }
    }
}
