using Fusion.Visao.Pessoa;
using MahApps.Metro.Controls;

namespace Fusion.Controladores.Menu
{
    public class PessoaController : Controlador
    {
        public PessoaController(MetroTabControl tabControl) : base(tabControl)
        {
        }

        public void Listagem()
        {
            var vm = new PessoaGridModel();
            var content = new PessoaGridControl(vm);

            AbrirJanelaEmAba("Pessoa", content);
        }
    }
}