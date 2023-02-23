using System;
using System.Windows.Input;

namespace Fusion.Visao.Tef
{
    public partial class GridPosForm
    {
        private readonly GridPosFormModel _model;

        public GridPosForm(GridPosFormModel model)
        {
            _model = model;
            DataContext = _model;
            InitializeComponent();
        }

        private void EditarPosSelecionado(object sender, MouseButtonEventArgs e)
        {
            _model.Editar();
        }

        private void GridPosForm_OnContentRendered(object sender, EventArgs e)
        {
            _model.Pesquisar();
        }

        private void TBPesquisar_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            _model.Pesquisar();
            TBPesquisar.Focus();
        }
    }
}
