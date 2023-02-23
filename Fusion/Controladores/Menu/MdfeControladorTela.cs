using Fusion.Visao.MdfeEletronico;
using Fusion.Visao.MdfeEletronico.NaoEncerrados;
using MahApps.Metro.Controls;

namespace Fusion.Controladores.Menu
{
    public class MdfeControladorTela : Controlador
    {
        private readonly GridMdfe _grid;

        public MdfeControladorTela(MetroTabControl tabControl) : base(tabControl)
        {
            _grid = new GridMdfe();
        }

        public void GridMdfe()
        {
            AbrirJanelaEmAba("Gerenciar MDF-e", _grid);
        }

        public void MdfeNaoEncerrados()
        {
            var view = new ConsultaNaoEncerradosForm();
            view.ShowDialog();
            _grid.GridMdfeModel.AplicarPesquisa();
        }
    }
}