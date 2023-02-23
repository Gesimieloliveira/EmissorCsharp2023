using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.ExportacaoBuscaRapida
{
    public partial class ExportacaoBuscaRapidaForm
    {
        private ExportacaoBuscaRapidaFormModel _model;

        public ExportacaoBuscaRapidaForm(ExportacaoBuscaRapidaFormModel model)
        {
            _model = model;
            InitializeComponent();
            DataContext = _model;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _model.Inicializar();
        }

        private void LayoutChangedHandler(object sender, SelectionChangedEventArgs e)
        {
            _model.CarregarPreferencias();
        }

        private void ClickAddDestinoCopia(object sender, RoutedEventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "Texto (*.txt)|*.txt|All files (*.*)|*.*";
                dialog.DefaultExt = ".txt";

                if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                _model.AdicionaDestinoCopia(dialog.FileName);
            }
        }

        private void RemoverLocalHandler(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn)
            {
                _model.RemoverLocal(btn.Tag as Local);
            }
        }

        private void ExportarClickHandler(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;

            Task.Run(() =>
            {
                try
                {
                    _model.ExportarParaLocal();

                    Dispatcher.Invoke(() =>
                    {
                        if (_model.Avisos.Count > 0)
                        {
                            DialogBox.MostraAviso("Arquivo exportado com avisos", _model.Avisos);
                            Close();
                            return;
                        }

                        DialogBox.MostraInformacao($"Arquivo exportado com sucesso");
                        Close();
                        Close();
                    });
                }
                catch (InvalidOperationException ex)
                {
                    Dispatcher.Invoke(() => { DialogBox.MostraAviso(ex.Message); });
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() => { DialogBox.MostraErro(ex.Message, ex); });
                }
                finally
                {
                    Dispatcher.Invoke(() => IsEnabled = true);
                }
            });
        }
    }
}
