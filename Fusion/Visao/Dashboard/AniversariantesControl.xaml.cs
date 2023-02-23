using System.Windows;
using Fusion.Visao.Relatorios.Opcoes;

namespace Fusion.Visao.Dashboard
{
    public partial class AniversariantesControl
    {
        public AniversariantesControl()
        {
            InitializeComponent();
        }

        private AniversarianteContexto Contexto => DataContext as AniversarianteContexto;

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            Contexto?.Refresh();
        }

        private void GerarRelatorioClickHandler(object sender, RoutedEventArgs e)
        {
            var op = new OpcaoRelatorioAniversariantes();

            op.Visualizar();
        }
    }
}