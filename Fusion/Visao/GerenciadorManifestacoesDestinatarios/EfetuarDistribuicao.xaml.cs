using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.GerenciadorManifestacoesDestinatarios
{
    public partial class EfetuarDistribuicao
    {
        private readonly EfetuarDistribuicaoModel _model;

        public EfetuarDistribuicao(EfetuarDistribuicaoModel model)
        {
            InitializeComponent();

            _model = model;
            _model.Fechar += delegate { Application.Current.Dispatcher.Invoke(Close); };

            DataContext = model;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _model.Inicializar();
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_model.PodeConsultar)
            {
                DialogBox.MostraAviso("Seguindo as exigências da SEFAZ a consulta deve ter intervalos de 1 hora");
                return;
            }

            if (_model.UsarNsuZero && !ConfirmaUsoNsuZero())
            {
                return;
            }

            await RunTaskWithProgress(() =>
            {
                try
                {
                    _model.ResultadoDocumentosProcessados.Clear();
                    _model.EfetuarDistribuicao();
                    Dispatcher.Invoke(AcaoSucessoManifestacao);
                }
                catch (Exception exception)
                {
                    Dispatcher.Invoke(() => DialogBox.MostraAviso(exception.Message));
                }
            });
        }

        private bool ConfirmaUsoNsuZero()
        {
            const string aviso =
                "Usar NSU=0 faz com que o Sistema consulte todos os documentos disponívies pela SEFAZ" +
                ", esse recurso deve ser utilizado apenas quando existir suspeita de documentos não localizados." +
                "\r\n\r\nO USO FREQUENTE pode causar BLOQUEIO do serviço pela SEFAZ.";

            DialogBox.MostraAviso(aviso);

            var result = DialogBox.MostraConfirmacao("Deseja continuar a Consultar com o NSU='0'?");

            return result == MessageBoxResult.Yes;
        }

        private void AcaoSucessoManifestacao()
        {
            DialogBox.MostraInformacao("Tudo ok ao consultar os documentos. Verifique os itens processados!");
        }
    }
}