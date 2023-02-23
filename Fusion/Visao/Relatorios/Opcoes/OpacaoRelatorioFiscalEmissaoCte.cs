using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpacaoRelatorioFiscalEmissaoCTe : OpcaoRelatorioFixo<RRelatorioFiscalEmissaoCte>
    {
        public override string Descricao { get; } = "Relatório fiscal de emissões no CT-e";

        public override string Grupo { get; } = "Relatório Fiscal";

        protected override RRelatorioFiscalEmissaoCte CriaRelatorio()
        {
            return new RRelatorioFiscalEmissaoCte(SessaoManager, Descricao);
        }
    }
}