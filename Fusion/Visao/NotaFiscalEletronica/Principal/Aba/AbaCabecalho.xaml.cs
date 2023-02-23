using System.Windows;
using Fusion.Visao.NotaFiscalEletronica.Principal.Aba.Models;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Aba
{
    public partial class AbaCabecalho
    {
        private AbaCabecalhoModel ViewModel => DataContext as AbaCabecalhoModel;

        public AbaCabecalho()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ComboBoxTipoOperacao.Focus();
        }

        private void OnClickProximoPasso(object sender, RoutedEventArgs e)
        {
            ViewModel.OnProximoPassoCalled();
        }

        private void ClickAlterarEmissorHandler(object sender, RoutedEventArgs e)
        {
            ViewModel.OnAlterarEmissorCalled();
        }

        private void ClickAlocarProximoNmeroHandler(object sender, RoutedEventArgs e)
        {
            const string msg =
                "Utilizar o próximo número definido no emissor? Utilize apenas para correção de duplicidade.";

            var confirm = DialogBox.MostraConfirmacao(msg);

            if (confirm == MessageBoxResult.Yes)
            {
                ViewModel.OnAlterarNumeroCalled();
            }
        }
    }
}