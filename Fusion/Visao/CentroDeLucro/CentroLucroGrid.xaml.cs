using System.Windows;
using System.Windows.Input;
using FusionCore.FusionAdm.Financeiro;

namespace Fusion.Visao.CentroDeLucro
{
    public partial class CentroLucroGrid
    {
        private readonly CentroLucroGridModel _centroDeCustoGridModel;

        public CentroLucroGrid()
        {
            _centroDeCustoGridModel = new CentroLucroGridModel();
            DataContext = _centroDeCustoGridModel;
            InitializeComponent();
        }

        private void Novo_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _centroDeCustoGridModel.Novo();
        }

        private void Editar_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _centroDeCustoGridModel.Editar();
        }

        private void OnClickBotaoNovo(object sender, RoutedEventArgs e)
        {
            new CentroLucroForm(new CentroLucro()).ShowDialog();
            _centroDeCustoGridModel.ObterCentroDeLucro();
        }

        private void OnSearch(object sender, RoutedEventArgs e)
        {
            _centroDeCustoGridModel.ObterCentroDeLucroPorDescricao();
        }

        private void OnDoubleClickGridRow(object sender, MouseButtonEventArgs e)
        {
            _centroDeCustoGridModel.Editar();
        }
    }
}
