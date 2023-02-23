using System.Windows;
using System.Windows.Input;
using Fusion.Visao.MdfeEletronico.Aba.Model;

namespace Fusion.Visao.MdfeEletronico.Aba
{
    public partial class ListaLacre
    {
        public ListaLacre()
        {
            InitializeComponent();
        }

        private void OnDoubleClickItem(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void OnClickDeletaItem(object sender, RoutedEventArgs e)
        {
            var model = DataContext as AbaMdfeCarregamentoModel;

            model?.DeletaLacreSelecionado();
        }
    }
}
