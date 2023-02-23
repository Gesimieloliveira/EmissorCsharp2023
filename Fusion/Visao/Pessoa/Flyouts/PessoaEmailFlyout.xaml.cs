using System.Windows;

namespace Fusion.Visao.Pessoa.Flyouts
{
    public partial class PessoaEmailFlyout
    {
        private PessoaEmailFlyoutModel ViewModel => DataContext as PessoaEmailFlyoutModel;

        public PessoaEmailFlyout()
        {
            InitializeComponent();
        }

        private void ClickSalvarHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.AdicionarEmail();
            TbEmail.Focus();
        }
    }
}