using System.Windows;
using Fusion.Visao.MdfeEletronico.Aba.Model;

namespace Fusion.Visao.MdfeEletronico.Aba
{
    public partial class ListaSeguro
    {
        public ListaSeguro()
        {
            InitializeComponent();
        }

        private void OnClickDeletaItem(object sender, RoutedEventArgs e)
        {
            var model = DataContext as AbaMdfeCarregamentoModel;
            model?.DeletarSeguroCargaSelecionado();
        }
    }
}
