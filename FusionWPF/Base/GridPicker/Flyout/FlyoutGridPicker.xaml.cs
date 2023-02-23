using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionWPF.Base.GridPicker.Flyout
{
    public partial class FlyoutGridPicker
    {
        private FlyoutGridPickerModel Model => ((FlyoutGridPickerModel) DataContext);

        public FlyoutGridPicker()
        {
            InitializeComponent();
        }

        private void EfetuaPesquisa_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Model.CommandAplicarFiltro.Execute(Model);
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }
    }
}
