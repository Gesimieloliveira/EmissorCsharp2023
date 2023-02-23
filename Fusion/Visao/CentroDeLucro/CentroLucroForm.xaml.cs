using System.Windows;
using FusionCore.FusionAdm.Financeiro;

namespace Fusion.Visao.CentroDeLucro
{
    public partial class CentroLucroForm
    {
        private readonly CentroLucroFormModel _centroLucroFormModel;

        public CentroLucroForm(CentroLucro centroCusto)
        {
            _centroLucroFormModel = new CentroLucroFormModel(centroCusto);
            DataContext = _centroLucroFormModel;
            InitializeComponent();
        }

        private void OnClickSalvar(object sender, RoutedEventArgs routedEventArgs)
        {
            _centroLucroFormModel.Salvar();
            Close();
        }

        private void OnClickFechar(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnClickDeletar(object sender, RoutedEventArgs e)
        {
            _centroLucroFormModel.Deletar();
            Close();
        }
    }
}
