using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using FusionCore.Exportacao.ItensBalanca;
using FusionWPF.Base.Utils.Dialogs;
using Button = System.Windows.Controls.Button;

namespace Fusion.Visao.ExportacaoBalanca
{
    public partial class ExportacaoBalancaView
    {
        private readonly ExportacaoBalancaContexto _contexto;

        public ExportacaoBalancaView(ExportacaoBalancaContexto contexto)
        {
            _contexto = contexto;

            InitializeComponent();
            DataContext = contexto;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _contexto.Inicializar();
        }

        private void ExportarClickHandler(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;

            Task.Run(() =>
            {
                try
                {
                    _contexto.ExportarParaLocal();

                    Dispatcher.Invoke(() =>
                    {
                        if (_contexto.Avisos.Count > 0)
                        {
                            DialogBox.MostraAviso("Arquivo exportado com avisos", _contexto.Avisos);
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

                _contexto.AdicionaDestinoCopia(dialog.FileName);
            }
        }

        private void LayoutChangedHandler(object sender, SelectionChangedEventArgs e)
        {
            _contexto.CarregarPreferencias();
        }

        private void RemoverLocalHandler(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                _contexto.RemoverLocal(btn.Tag as PreferenciaExportacao);
            }
        }
    }
}
