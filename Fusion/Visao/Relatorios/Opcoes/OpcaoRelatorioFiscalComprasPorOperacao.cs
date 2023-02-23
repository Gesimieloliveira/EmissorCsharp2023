using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoRelatorioFiscalComprasPorOperacao
        : OpcaoRelatorioFixo<RClassificacaoFiscalDeComprasPorOperacao>
    {
        public override string Descricao { get; } = "Relatório de classificação fiscal de compras por operação";

        public override string Grupo { get; } = "Relatório Fiscal";

        protected override RClassificacaoFiscalDeComprasPorOperacao CriaRelatorio()
        {
            return  new RClassificacaoFiscalDeComprasPorOperacao(SessaoManager);
        }
    }
}