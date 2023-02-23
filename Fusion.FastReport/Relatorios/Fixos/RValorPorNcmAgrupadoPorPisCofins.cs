using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RValorPorNcmAgrupadoPorPisCofins : RelatorioBase
    {
        public static string Descricao = "Relatório auxiliar de saidas por NCM agrupado por PIS/COFINS";

        public RValorPorNcmAgrupadoPorPisCofins(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemBytesFrx("FrValorPorNcmAgrupadoPorPisCofins.frx");
        }

        protected override void PrepararDados()
        {
            RegistrarDescricao(Descricao);
        }
    }
}