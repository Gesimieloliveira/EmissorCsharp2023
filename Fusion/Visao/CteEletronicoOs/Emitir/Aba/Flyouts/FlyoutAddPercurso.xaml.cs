using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronicoOs.Emitir.Aba.Flyouts
{
    public partial class FlyoutAddPercurso
    {
        public FlyoutAddPercurso()
        {
            InitializeComponent();
        }

        private FlyoutAddPercursoModel ViewModel => DataContext as FlyoutAddPercursoModel;

        private void OnIsOpenChanged(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsOpen)
            {
                ViewModel.Inicializar();
                CbEstado.Focus();
            }
        }

        private void OnClickAdicionar(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.AdicionarPercurso();
                ViewModel.Limpar();
                CbEstado.Focus();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void OnClickAdicionarAndClose(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.AdicionarPercurso();
                ViewModel.IsOpen = false;
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}