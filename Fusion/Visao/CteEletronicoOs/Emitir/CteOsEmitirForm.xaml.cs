using System;
using System.Windows;

namespace Fusion.Visao.CteEletronicoOs.Emitir
{
    public partial class CteOsEmitirForm
    {
        public CteOsEmitirForm(CteOsEmitirFormModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }

        private CteOsEmitirFormModel Model => DataContext as CteOsEmitirFormModel;

        private void CteOsEmitirForm_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Model == null)
            {
                return;
            }

            Model.Loaded();
            Model.Fechar += (s, ex) => Close();
        }

        private void CteOsEmitirForm_OnContentRendered(object sender, EventArgs e)
        {
            Model.VerificaHistoricoPendente();
        }
    }
}