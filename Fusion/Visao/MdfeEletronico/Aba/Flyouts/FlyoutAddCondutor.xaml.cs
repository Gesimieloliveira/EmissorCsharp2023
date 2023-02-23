using System;
using System.Windows;
using Fusion.Visao.MdfeEletronico.Aba.Flyouts.Model;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MdfeEletronico.Aba.Flyouts
{
    public partial class FlyoutAddCondutor
    {
        private FlyoutAddCondutorModel ViewModel => DataContext as FlyoutAddCondutorModel;

        public FlyoutAddCondutor()
        {
            InitializeComponent();
        }

        private void OnClickBotaoSalvaCondutor(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
            {
                return;
            }

            try
            {

                ViewModel.SalvarCondutor();
                ViewModel.LimpaCampos();
                ViewModel.IsOpen = false;
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}
