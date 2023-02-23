using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoRValorPorNcmAgrupadoPorIcms
        : OpcaoRelatorioFixo<RValorPorNcmAgrupadoPorIcms>
    {
        public override string Descricao => RValorPorNcmAgrupadoPorIcms.Descricao;
        public override string Grupo { get; } = "Relatório Fiscal";

        protected override RValorPorNcmAgrupadoPorIcms CriaRelatorio()
        {
            return new RValorPorNcmAgrupadoPorIcms(SessaoManager);
        }
    }

    public class OpcaoRValorPorNcmAgrupadoPorPisCofins
        : OpcaoRelatorioFixo<RValorPorNcmAgrupadoPorPisCofins>
    {
        public override string Descricao => RValorPorNcmAgrupadoPorPisCofins.Descricao;
        public override string Grupo { get; } = "Relatório Fiscal";

        protected override RValorPorNcmAgrupadoPorPisCofins CriaRelatorio()
        {
            return new RValorPorNcmAgrupadoPorPisCofins(SessaoManager);
        }
    }
}