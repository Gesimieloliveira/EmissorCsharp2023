using FusionCore.Exportacao.ItensBalanca;

namespace Fusion.Visao.ExportacaoBalanca
{
    public class OpcaoExportacao
    {
        public OpcaoExportacao(string descricao, ILayouotBalanca layouotBalanca)
        {
            Descricao = descricao;
            LayouotBalanca = layouotBalanca;
        }

        public string Descricao { get; }
        public ILayouotBalanca LayouotBalanca { get; }
    }
}