using System.Windows;
using Fusion.Visao.MdfeEletronico.Aba.Model;

namespace Fusion.Visao.MdfeEletronico.Aba
{
    public partial class ListaContratante
    {
        public ListaContratante()
        {
            InitializeComponent();
        }

        private void OnClickDeletaItem(object sender, RoutedEventArgs e)
        {
            var model = DataContext as AbaRodoviarioMdfeModel;

            model?.DeletarContratanteSelecionado();
        }
    }
}
