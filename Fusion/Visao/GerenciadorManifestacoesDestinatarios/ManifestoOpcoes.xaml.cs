using System;
using System.Windows;
using FusionCore.GerenciarManifestacoesEletronicas;
using FusionCore.Helpers.Hidratacao;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.GerenciadorManifestacoesDestinatarios
{
    public partial class ManifestoOpcoes
    {
        private readonly ManifestoOpcoesModel _model;

        public ManifestoOpcoes(NfeResumidaGrid nfeResumidaGrid)
        {
            InitializeComponent();
            _model = new ManifestoOpcoesModel(nfeResumidaGrid);
            DataContext = _model;
        }

        private async void CienciaDaEmissao_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!DialogBox.MostraConfirmacao("Confirmar Ciência da Emissao?", MessageBoxImage.Question)) return;

                _model.IsContemCienciaEmissao();

                await RunTaskWithProgress(() => _model.CienciaEmissaoAcao());
            }
            catch (InvalidOperationException exception)
            {
                DialogBox.MostraInformacao(exception.Message);
            }
        }

        private async void ConfirmacaoDaOperacao_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!DialogBox.MostraConfirmacao("Confirmar Confirmação da Operação?", MessageBoxImage.Question)) return;

                
                _model.IsContemConfirmacaoDaOperacao();

                await RunTaskWithProgress(() => _model.ConfirmacaoOperacao());
            }
            catch (InvalidOperationException exception)
            {
                DialogBox.MostraInformacao(exception.Message);
            }
        }

        private async void OperacaoNaoRealizada_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!DialogBox.MostraConfirmacao("Confirmar Operação não Realizada?", MessageBoxImage.Question)) return;

                _model.IsContemOperacaoNaoRealizada();

                var modelJustificativa = new JustificativaOperacaoNaoRealizadaModel();
                new JustificativaOperacaoNaoRealizada(modelJustificativa).ShowDialog();

                if (modelJustificativa.Justificativa.IsNullOrEmpty())
                {
                    DialogBox.MostraInformacao("Cancelada ação pelo usuário");
                    return;
                }

                await RunTaskWithProgress(() => _model.OperacaoNaoRealizada(modelJustificativa.Justificativa));
            }
            catch (InvalidOperationException exception)
            {
                DialogBox.MostraInformacao(exception.Message);
            }
        }

        private async void DesconhecimentoDaOperacao_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!DialogBox.MostraConfirmacao("Confirmar Desconheicmento da Operação?", MessageBoxImage.Question)) return;


                _model.IsContemDesconhecimentoDaOperacao();

                await RunTaskWithProgress(() => _model.DesconhecimentoDaOperacao());
            }
            catch (InvalidOperationException exception)
            {
                DialogBox.MostraInformacao(exception.Message);
            }
        }

        private async void DownloadXml_OnClick(object sender, RoutedEventArgs e)
        {
            var nfeResuminda = _model.BuscarNFeResumida();

            if (nfeResuminda.IsDownloadDisponivel())
            {
                _model.SalvarXmlEmDisco(nfeResuminda);
                return;
            }

            await RunTaskWithProgress(() => _model.DownloadXml(nfeResuminda));
        }

        private void MarcarComoImportada_OnClick(object sender, RoutedEventArgs e)
        {
            if (!DialogBox.MostraConfirmacao("Deseja marcar como importada?\n Esse Processo é inrreversivel!",
                MessageBoxImage.Question)) return;

            _model.MarcarComoImportada();
        }
    }
}
