using System.Windows;
using System.Windows.Input;
using Fusion.Visao.MdfeEletronico.Aba.Model;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MdfeEletronico.Aba
{
    public partial class ListaPagamento
    {
        public ListaPagamento()
        {
            InitializeComponent();
        }

        private void OnClickDeletaItem(object sender, RoutedEventArgs e)
        {

            if (!DialogBox.MostraConfirmacao("Deseja realmente deletar a informação pagamento?",
                MessageBoxImage.Question)) return;

            var model = DataContext as AbaRodoviarioMdfeModel;
            model?.DeletarInformacaoPagamento();
        }

        private void OnClickEditarItem(object sender, RoutedEventArgs e)
        {
            var model = DataContext as AbaRodoviarioMdfeModel;

            model?.AddInformacaoPagamento(true);
        }

        private void DoubleClickDataGridRow(object sender, MouseButtonEventArgs e)
        {
            OnClickEditarItem(sender, e);
        }
    }
}
