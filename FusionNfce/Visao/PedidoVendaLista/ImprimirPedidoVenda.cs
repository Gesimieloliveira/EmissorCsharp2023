using Fusion.FastReport.Relatorios.Sistema;
using FusionCore.Sessao;

namespace FusionNfce.Visao.PedidoVendaLista
{
    internal class ImprimirPedidoVenda
    {
        public void Imprimir(int idPedidoVenda)
        {
            using (var r = new RImpressaoPedidoVenda(new SessaoManagerNfceServidor()))
            {
                r.ComPedidoId(idPedidoVenda);
                r.Visualizar();
            }
        }
    }
}