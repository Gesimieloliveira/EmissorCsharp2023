using System.Windows;
using System.Windows.Input;
using Fusion.Visao.MdfeEletronico.Aba.Model;

namespace Fusion.Visao.MdfeEletronico.Aba
{
    public partial class ListaPercurso
    {
        public ListaPercurso()
        {
            InitializeComponent();
        }

        private void OnDoubleClickItem(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void OnClickDeletaItem(object sender, RoutedEventArgs e)
        {
            var model = DataContext as AbaMdfeCarregamentoModel;
            model?.DeletarPercursoSelecionado();
        }
    }
}
