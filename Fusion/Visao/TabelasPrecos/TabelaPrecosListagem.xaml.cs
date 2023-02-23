using System.Windows;
using System.Windows.Input;
using FusionCore.FusionAdm.TabelasDePrecos;

namespace Fusion.Visao.TabelasPrecos
{
    public partial class TabelaPrecosListagem
    {
        private readonly TabelaPrecosListagemModel _model;

        public TabelaPrecosListagem(TabelaPrecosListagemModel model)
        {
            InitializeComponent();
            _model = model;
            DataContext = _model;
        }

        private void CriaNovaTabela(object sender, RoutedEventArgs e)
        {
            var model = new TabelaPrecoFormularioModel(new TabelaPreco());
            new TabelaPrecoFormulario(model).ShowDialog();

            _model.AplicaPesquisa();
        }

        private void AplicaPesquisa_OnClick(object sender, RoutedEventArgs e)
        {
            _model.AplicaPesquisa();
        }

        private void DoubleClickDataGridRow(object sender, MouseButtonEventArgs e)
        {
            var tabelaPreco = _model.ObtemTabelaPreco();

            var model = new TabelaPrecoFormularioModel(tabelaPreco);
            new TabelaPrecoFormulario(model).ShowDialog();

            _model.AplicaPesquisa();
        }
    }
}
