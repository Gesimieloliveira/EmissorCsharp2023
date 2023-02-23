using System;
using System.Threading;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.ImportarIbpt
{
    public partial class JanelaImportarIbpt
    {
        private readonly ImportarIbptModel _model;

        public JanelaImportarIbpt()
        {
            _model = new ImportarIbptModel();
            DataContext = _model;
            InitializeComponent();
        }

        private void OnClickImportar(object sender, RoutedEventArgs e)
        {
            var thread = new Thread(() =>
            {
                try
                {
                    _model.FazerImportacao();
                    DialogBox.MostraInformacao("Ibpt foi importado com sucesso");
                    Dispatcher.Invoke(Close);
                }
                catch (Exception ex)
                {
                    DialogBox.MostraAviso(ex.Message);
                }
            })
            {
                IsBackground = true
            };

            Closing += (a, b) =>
            {
                if (thread.ThreadState == ThreadState.Running)
                    thread.Abort();
            };

            thread.Start();
        }
    }
}