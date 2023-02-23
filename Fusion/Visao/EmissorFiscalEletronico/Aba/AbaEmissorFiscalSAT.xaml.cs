using System.Drawing;
using System.Windows;
using System.Windows.Input;
using FusionLibrary.Helper.Conversores;
using FusionWPF.Base.Utils.Dialogs;
using Microsoft.Win32;

namespace Fusion.Visao.EmissorFiscalEletronico.Aba
{
    public partial class AbaEmissorFiscalSAT
    {
        public AbaEmissorFiscalSAT()
        {
            InitializeComponent();
        }

        private void OnMouseClick(object sender, MouseButtonEventArgs e)
        {
            UploadLogo();
        }

        private void OnClickUploadLogo(object sender, RoutedEventArgs e)
        {
            UploadLogo();
        }

        private void OnClickDeletaLogo(object sender, RoutedEventArgs e)
        {
            ((EmissorFiscalFormModel)DataContext).ArquivoLogoSat = null;
        }

        private void UploadLogo()
        {
            var janelaArquivo = new OpenFileDialog
            {
                Filter = "(*.png)|*.png"
            };

            if (janelaArquivo.ShowDialog() != true) return;

            var caminhoArquivo = janelaArquivo.FileName;

            if (string.IsNullOrEmpty(caminhoArquivo))
            {
                DialogBox.MostraAviso("Selecione uma logo para a emissão da Nfc-e, a logo tem que ser 50x50");
                return;
            }


            var imagemRedimensionada = ConverteImage.Redimensionar(Image.FromFile(caminhoArquivo), 50, 50);

            var logo = ConverteImage.ToBitmapImage(imagemRedimensionada);

            ((EmissorFiscalFormModel)DataContext).ArquivoLogoSat = logo;
        }

        private void HabilitarEdicaoAmbiente_OnClick(object sender, RoutedEventArgs e)
        {
            if (DialogBox.MostraConfirmacaoComMensagemDeConfirmacao("Alterar AMBIENTE",
                "Deseja realmente editar o ambiente?\nATENÇÃO: Ao Editar o Ambiente",
                "ALTERAR AMBIENTE"))
            {
                ((EmissorFiscalFormModel)DataContext).EditarAmbiente = true;
                ((EmissorFiscalFormModel)DataContext).MensagemEdicaoAmbiente = true;
            }
        }
    }
}
