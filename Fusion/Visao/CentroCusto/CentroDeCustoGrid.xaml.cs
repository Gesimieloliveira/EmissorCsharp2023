using System.Windows;
using System.Windows.Input;
using CentroDeCusto = FusionCore.FusionAdm.Financeiro.CentroCusto;

namespace Fusion.Visao.CentroCusto
{
    public partial class CentroDeCustoGrid
    {
        private readonly CentroDeCustoGridModel _centroDeCustoGridModel;

        public CentroDeCustoGrid()
        {
            _centroDeCustoGridModel = new CentroDeCustoGridModel();
            DataContext = _centroDeCustoGridModel;
            InitializeComponent();
        }

        private void Editar_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _centroDeCustoGridModel.Editar();
        }

        private void OnClickBotaoNovo(object sender, RoutedEventArgs e)
        {
            new CentroCustoForm(new CentroDeCusto()).ShowDialog();
            _centroDeCustoGridModel.ObterCentroDeCustos();
        }

        private void OnSearch(object sender, RoutedEventArgs e)
        {
            _centroDeCustoGridModel.ObterCentroDeCustosPorDescricao();
        }

        private void OnDoubleClickGridRow(object sender, MouseButtonEventArgs e)
        {
            _centroDeCustoGridModel.Editar();
        }
    }
}