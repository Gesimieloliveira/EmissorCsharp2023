using System.Windows;
using Fusion.Visao.MdfeEletronico.Aba.Model;

namespace Fusion.Visao.MdfeEletronico.Aba
{
    public partial class ListaCondutor
    {
        public ListaCondutor()
        {
            InitializeComponent();
        }

        private void OnClickDeletaItem(object sender, RoutedEventArgs e)
        {
            var model = DataContext as AbaRodoviarioMdfeModel;

            model?.DeletarCondutorSelecionado();
        }
    }
}
