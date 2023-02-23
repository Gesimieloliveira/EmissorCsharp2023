using System;
using System.Windows;

namespace FusionNfce.Visao.BarraDeProgresso
{
    public partial class ProgressBarAgil4
    {
        private static ProgressBarAgil4 _instancia;
        private bool _isClosed;

        private static ProgressBarAgil4 Instancia
        {
            get
            {
                if (_instancia == null || _instancia._isClosed)
                    _instancia = new ProgressBarAgil4();

                return _instancia;
            }
        }

        private ProgressBarAgil4()
        {
            InitializeComponent();
        }

        public static void ShowProgressBar()
        {
            Instancia.ShowDialogSemEspera();
        }

        public static void CloseProgressBar()
        {
            Instancia?.Fechar();
        }

        private void ShowDialogSemEspera()
        {
            Application.Current.Dispatcher.BeginInvoke(new Func<bool?>(ShowDialog));
        }

        private void Fechar()
        {
            if (_isClosed)
            {
                return;
            }

            _isClosed = true;
            Application.Current.Dispatcher.BeginInvoke(new Action(Close));
        }
    }
}