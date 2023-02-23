using Fusion.Visao.Base.Grid;
using Fusion.Visao.Compras.Importacao;
using Fusion.Visao.Compras.NotaFiscal;
using MahApps.Metro.Controls;

namespace Fusion.Controladores.Menu
{
    internal class ComprasController : Controlador
    {
        private const string TituloAbaEntradas = "Listar Compras";
        private NotaFiscalCompraGridModel _notaFiscalCompraGridModel;

        public ComprasController(MetroTabControl tabControl) : base(tabControl)
        {
        }

        public void ListarEntradas()
        {
            _notaFiscalCompraGridModel = new NotaFiscalCompraGridModel();

            var view = new GridPadrao(_notaFiscalCompraGridModel);
            AbrirJanelaEmAba(TituloAbaEntradas, view);
        }

        public void NovaEntrada()
        {
            ShowDialog(new NotaFiscalCompraView());
            ReloadGridCompras();
        }

        private void ReloadGridCompras()
        {
            var abaAberta = GetTab(TituloAbaEntradas);

            if (abaAberta != null)
            {
                _notaFiscalCompraGridModel?.PopularLista();
            }
        }

        public void ImportarCompra()
        {
            ShowDialog(new ImportacaoCompraView());
            ReloadGridCompras();
        }
    }
}