using System;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Sintegra
{
    public partial class SintegraForm
    {
        private readonly SintegraFormModel _model;

        public SintegraForm(SintegraFormModel model)
        {
            InitializeComponent();
            _model = model;
            DataContext = _model;
        }

        private void SintegraForm_OnLoaded(object sender, RoutedEventArgs e)
        {
            _model.Inicializar();
        }

        private async void ClickProcessarIventarioHandler(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Title = @"Escolha um local para salvar o arquivo Sintegra",
                RestoreDirectory = true,
                InitialDirectory = @"C:\SistemaFusion",
                FileName = $@"sintegra-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.zip",
                Filter = @"Zip (.zip)|*.zip"
            };

            var result = dialog.ShowDialog();

            if (result != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            try
            {
                await RunTaskWithProgress(() =>
                {
                    Thread.Sleep(1);
                    _model.GerarArquivoIventario(dialog.FileName);
                });

                DialogBox.MostraInformacao($"Arquivo do Sintegra foi gerado em: {dialog.FileName}");
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}