using Fusion.Visao.Produto;
using MahApps.Metro.Controls;

namespace Fusion.Controladores.Menu
{
    public class EstoqueController : Controlador
    {
        private ProdutoGridModel _produtoGridModel;

        public EstoqueController(MetroTabControl tabControl) : base(tabControl)
        {
        }

        public void ListarProdutos()
        {
            _produtoGridModel = new ProdutoGridModel();
            var content = new ProdutoGridControl();

            AbrirJanelaEmAba("Produto", content, _produtoGridModel);
        }
    }
}