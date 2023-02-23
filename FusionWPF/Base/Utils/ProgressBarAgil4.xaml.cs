using System;

namespace FusionWPF.Base.Utils
{
    public partial class ProgressBarAgil4
    {
        private static ProgressBarAgil4 _instancia;
        private bool _isClosed;

        private ProgressBarAgil4()
        {
            InitializeComponent();
        }

        private static ProgressBarAgil4 Instancia
        {
            get
            {
                if (_instancia == null || _instancia._isClosed)
                    _instancia = new ProgressBarAgil4();

                return _instancia;
            }
        }

        public static void ShowProgressBar()
        {
            Instancia.ShowDialogSemEspera();
        }

        public static void CloseProgressBar()
        {
            Instancia.Fechar();
        }

        private void ShowDialogSemEspera()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    ShowDialog();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }));
        }

        private void Fechar()
        {
            _isClosed = true;
            Dispatcher.Invoke(Close);
        }
    }
}