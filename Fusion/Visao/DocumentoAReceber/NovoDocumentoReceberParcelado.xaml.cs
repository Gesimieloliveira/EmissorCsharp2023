using System;
using System.Windows;
using Fusion.Sessao;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Controles;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.DocumentoAReceber
{
    public partial class NovoDocumentoReceberParcelado
    {
        private readonly NovoDocumentoReceberParceladoContexto _contexto;

        public NovoDocumentoReceberParcelado(NovoDocumentoReceberParceladoContexto contexto)
        {
            _contexto = contexto;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = _contexto;
            CbEmpresa.Focus();
        }

        private async void GerarParcelasClickHandler(object sender, RoutedEventArgs e)
        {
            if (_contexto.Valor <= 0)
            {
                DialogBox.MostraAviso("Preciso que informe um valor total para ser gerado!");
                return;
            }

            Visibility = Visibility.Collapsed;

            var dialog = SessaoSistema.Instancia.ParcelamentoFactory.CriaDialog(_contexto.Valor);

            dialog.Contexto.ParceladoComSucesso += (o, args) =>
            {
                _contexto.ComParcelas(args);
                dialog.Close(true);
            };

            await this.TryFindParent<FusionWindow>().ShowChildWindowAsync(dialog);

            Visibility = Visibility.Visible;
            BtnOk.Focus();
        }

        private void CriarDocumentosClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                _contexto.GerarDocumentosParcelados();

                DialogBox.MostraInformacao("Documentos foram gerados com sucesso!");
                Close(true);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}