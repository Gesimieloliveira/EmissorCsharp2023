using System.Windows;
using Fusion.Controladores.Menu;
using Fusion.Visao.PedidoDeVenda.Lista;
using FusionCore.FusionAdm.PedidoVenda;
using FusionCore.Repositorio.Dtos.Consultas.PedidoDeVenda;
using FusionWPF.Factories;
using MahApps.Metro.Controls;

namespace Fusion.Visao.PedidoDeVenda
{
    public class PedidoVendaController : Controlador
    {
        private PedidoVendaForm _pedidoForm;
        private GridPedidoVenda _pedidoGridMenu;

        public PedidoVendaController(MetroTabControl tabControl) : base(tabControl)
        {
        }

        public void AbrirFormulario()
        {
            if (_pedidoForm == null)
            {
                try
                {
                    _pedidoForm = new PedidoVendaForm();
                    _pedidoForm.Closed += (sender, args) => _pedidoForm = null;
                    _pedidoForm.Show();

                    return;
                }
                catch
                {
                    _pedidoForm = null;
                    throw;
                }
            }

            _pedidoForm.WindowState = WindowState.Maximized;
            _pedidoForm.Focus();
        }

        public void AbrirFormularioEdicao(PedidoVendaDTO dto)
        {
            AbrirFormulario();

            _pedidoForm.CarregarPedidoAsync(dto);
        }

        public void MenuListagemPedidoVenda()
        {
            var pvContexto = new GridPedidoVendaModel();

            _pedidoGridMenu = new GridPedidoVenda(pvContexto, this);

            AbrirJanelaEmAba("Pedidos de Venda", _pedidoGridMenu);
        }

        public void DialogListagemImportacao()
        {
            var contexto = new GridPedidoVendaModel
            {
                FiltroEstadoAtual = EstadoAtual.Finalizado
            };

            var control = new GridPedidoVenda(contexto, this);

            var fwindow = FusionWindowFactory.Criar(
                "Listagem pedidos para importação",
                control,
                new FusionWindowFactory.WSize(980, 550)
            );

            fwindow.ShowDialog();
        }

        public void AbrirJanelaOpcoes(PedidoVendaDTO dto)
        {
            var dialogo = new OpcoesPedidoVenda(dto, this);

            dialogo.ShowDialog();
        }
    }
}