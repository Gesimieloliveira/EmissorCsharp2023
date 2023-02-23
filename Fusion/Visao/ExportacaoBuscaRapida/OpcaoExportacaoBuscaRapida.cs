using FusionCore.Exportacao.ItensBuscaRapida;

namespace Fusion.Visao.ExportacaoBuscaRapida
{
    public class OpcaoExportacaoBuscaRapida
    {
        public OpcaoExportacaoBuscaRapida(string descricao, ILayoutBuscaRapida layoutBuscaRapida)
        {
            Descricao = descricao;
            LayoutBuscaRapida = layoutBuscaRapida;
        }

        public string Descricao { get; }
        public ILayoutBuscaRapida LayoutBuscaRapida { get; }
    }
}