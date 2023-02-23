using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Dialogos.Models;

namespace FusionWPF.Dialogos.Controls
{
    public partial class FiltroPeriodoDialog
    {
        public FiltroPeriodoContexto Contexto { get; } = new FiltroPeriodoContexto();

        public FiltroPeriodoDialog()
        {
            InitializeComponent();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = Contexto;
        }

        private void ClickContinuarHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                Contexto.Confirmar();
                DialogResult = true;
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}
