using Fusion.FastReport.Relatorios.Sistema;
using Fusion.Visao.Relatorios.Comum;
using FusionWPF.Dialogos;

namespace Fusion.Visao.Relatorios.Opcoes.Sistema
{
    public class OpcaoImpressaoPedidoVenda : OpcaoRelatorioSistema<RImpressaoPedidoVenda>
    {
        public override string Descricao { get; } = "Impressão pedido de venda";

        protected override RImpressaoPedidoVenda CriaRelatorio()
        {
            return new RImpressaoPedidoVenda(SessaoManager);
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

        public override void ExportarTemplate()
        {
            AcaoExportarTemplate();
        }

        public override void ImportarTemplate()
        {
            AcaoImportarTemplate();
        }

        public override void ExcluirRelatorio()
        {
            AcaoExcluirTemplateSalvo();
        }

        protected override void OnDevEditarDesenho(string arquivoFrx)
        {
            if (!InputBox.ShowInput("ID do Pedido", out int id))
            {
                return;
            }

            using (var r = CriaRelatorio())
            {
                r.ComPedidoId(id);
                r.DevEditarDesenho(arquivoFrx);
            }
        }
    }
}