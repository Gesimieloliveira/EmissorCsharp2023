using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Fusion.Conversor.Core.Map;
using Fusion.Conversor.Views.Ajuda;
using FusionCore.Helpers.Exceptions;
using FusionWPF.Base.Utils;
using FusionWPF.Base.Utils.Dialogs;
using Microsoft.Win32;

namespace Fusion.Conversor.Views.CvClientes
{
    public partial class PessoaConversaoControl
    {
        private readonly PessoaConversaoContexto _contexto;

        public PessoaConversaoControl(PessoaConversaoContexto contexto)
        {
            _contexto = contexto;
            InitializeComponent();
        }

        private async void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (_contexto == null)
            {
                return;
            }

            DataContext = _contexto;

            await Task.Run(() => { _contexto.ManterCodigo = true; });
        }

        private void PickerCsvClickHandler(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = @"Arquivos CSV|*.csv" };
            var dialogResult = dialog.ShowDialog();

            if (dialogResult == false)
            {
                _contexto.CsvPath = string.Empty;
                return;
            }

            _contexto.CsvPath = dialog.FileName;
        }

        private void AjudaCsvMouseDownHandler(object sender, MouseButtonEventArgs e)
        {
            var contexto = new AjudaArquivoCsvContexto(new PessoaCsvMap());
            var view = new AjudaArquivoCsvView(contexto);

            view.ShowDialog();
        }

        private async void ImportarClickHandler(object sender, RoutedEventArgs e)
        {
            ProgressBarAgil4.ShowProgressBar();

            await Task.Run(() =>
            {
                try
                {
                    _contexto.FazerImportacao();
                    _contexto.Clear();

                    Dispatcher.Invoke(() => DialogBox.MostraInformacao("Pessoas/clientes foram importados com sucesso!"));
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        var messages = string.Join("\n", ex.GetAllMessages());
                        DialogBox.MostraErro(messages, ex);
                    });
                }
                finally
                {
                    ProgressBarAgil4.CloseProgressBar();
                }
            });
        }

        private async void CarregarDadosClickHandler(object sender, RoutedEventArgs e)
        {
            ProgressBarAgil4.ShowProgressBar();

            await Task.Run(() =>
            {
                try
                {
                    _contexto.CarregarCsv();
                    _contexto.ImportarIsEnabled = true;
                }
                finally
                {
                    ProgressBarAgil4.CloseProgressBar();
                }
            });
        }
    }
}
