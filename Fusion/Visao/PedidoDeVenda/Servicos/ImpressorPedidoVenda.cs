using System.IO;
using Fusion.FastReport.Relatorios.Sistema;
using FusionCore.Sessao;

namespace Fusion.Visao.PedidoDeVenda.Servicos
{
    public class ImpressorPedidoVenda
    {
        private readonly ISessaoManager _sessaoManager;

        public ImpressorPedidoVenda(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public void Imprimir(int id, string impressora, int? vias = null)
        {
            using (var r = new RImpressaoPedidoVenda(_sessaoManager))
            {
                r.ComPedidoId(id);
                r.Imprimir(impressora, vias);
            }
        }

        public void Visualizar(int id)
        {
            using (var r = new RImpressaoPedidoVenda(_sessaoManager))
            {
                r.ComPedidoId(id);
                r.Visualizar();
            }
        }

        public MemoryStream GerarPdf(int id)
        {
            using (var report = new RImpressaoPedidoVenda(_sessaoManager))
            {
                var pdf = new MemoryStream();

                report.ComPedidoId(id);
                report.ExportarPdf(pdf);

                return pdf;
            }
        }
    }
}