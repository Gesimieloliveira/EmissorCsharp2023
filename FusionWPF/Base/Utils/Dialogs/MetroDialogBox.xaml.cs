using System.Windows;
using System.Windows.Controls;

namespace FusionWPF.Base.Utils.Dialogs
{
    public partial class MetroDialogBox
    {
        private MetroDialogBoxModel ViewModel => DataContext as MetroDialogBoxModel;

        public MetroDialogBox(MetroDialogBoxModel model)
        {
            InitializeComponent();
            DataContext = model;
        }

        private void ClickOkHandler(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ClickConfirmarHandler(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void ClickDetalheErro(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;

            try
            {
                button.IsEnabled = false;
                ViewModel.MostrarInformacoesErro();
            }
            finally
            {
                button.IsEnabled = true;
            }
        }
    }
}