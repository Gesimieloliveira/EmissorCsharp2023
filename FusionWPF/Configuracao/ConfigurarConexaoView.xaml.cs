using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionWPF.Configuracao
{
    public partial class ConfigurarConexaoView
    {
        private readonly ConfigurarConexaoViewModel _contexto;

        public ConfigurarConexaoView(ConfigurarConexaoViewModel contexto)
        {
            _contexto = contexto;
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            CServidor.Focus();
            DataContext = _contexto;

            _contexto.CarregarArquivo();
        }

        private async void ClickSalvarHandler(object sender, RoutedEventArgs e)
        {
            await RunTaskWithProgress(() =>
            {
                try
                {
                    _contexto.SalvarConfiguracao();

                    Dispatcher.Invoke(() => { DialogResult = true; });
                }
                catch (Exception ex)
                {
                    DialogBox.MostraErro(ex.Message, ex);
                }
            });
        }
    }
}