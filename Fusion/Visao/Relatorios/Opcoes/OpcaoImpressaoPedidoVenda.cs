using Fusion.FastReport.Dialogos;
using Fusion.FastReport.Relatorios.Sistema;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes.Sistema
{
    public class OpcaoImpressaoPedidoVenda : OpcaoRelatorioBase<RImpressaoPedidoVenda>
    {
        public override string Descricao { get; } = "Impressão pedido de venda";
        public override string Grupo { get; } = "Sistema";

        protected override RImpressaoPedidoVenda CriaRelatorio()
        {
            return new RImpressaoPedidoVenda(SessaoManager);
        }

        public override void Visualizar()
        {
            if (!InputBox.ShowInput("ID do Pedido", out int id))
            {
                return;
            }

            using (var r = CriaRelatorio())
            {
                r.ComPedidoId(id);
                r.Preparar();
                r.Visualizar();
            }
        }

        public override void EditarDesenho()
        {
            if (!InputBox.ShowInput("ID do Pedido", out int id))
            {
                return;
            }

            using (var r = CriaRelatorio())
            {
                r.ComPedidoId(id);
                r.EditarDesenho();
            }
        }

        public override void DevEditarDesenho(string arquivoFrx)
        {
            if (!InputBox.ShowInput("ID do Pedido", out int id))
            {
                return;
            }

            using (var r = CriaRelatorio())
            {
                r.ComPedidoId(id);
                r.EditarDesenho(arquivoFrx);
            }
        }
    }
}