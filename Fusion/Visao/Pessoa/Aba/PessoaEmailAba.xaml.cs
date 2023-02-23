using System.Windows;
using System.Windows.Controls;
using FusionCore.FusionAdm.Pessoas;

namespace Fusion.Visao.Pessoa.Aba
{
    public partial class PessoaEmailAba
    {
        private PessoaFormModel ViewModel => DataContext as PessoaFormModel;

        public PessoaEmailAba()
        {
            InitializeComponent();
        }

        private void DeleteEmailHandler(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button))
            {
                return;
            }

            var pessoaEmail = (PessoaEmail) button.Tag;

            ViewModel.DeletarPessoaEmail(pessoaEmail);
        }

        private void AdicionarEmailHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.IniciaFlyoutEmail();
        }
    }
}