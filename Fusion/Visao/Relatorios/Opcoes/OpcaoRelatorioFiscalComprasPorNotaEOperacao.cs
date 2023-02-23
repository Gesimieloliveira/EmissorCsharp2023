using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoRelatorioFiscalComprasPorNotaEOperacao
        : OpcaoRelatorioFixo<RClassificacaoFiscalDeComprasPorNotaEOperacao>
    {
        public override string Descricao { get; } = "Relatório de classificação fiscal de compras por nota e operação";

        public override string Grupo { get; } = "Relatório Fiscal";

        protected override RClassificacaoFiscalDeComprasPorNotaEOperacao CriaRelatorio()
        {
            return  new RClassificacaoFiscalDeComprasPorNotaEOperacao(SessaoManager);
        }
    }
}