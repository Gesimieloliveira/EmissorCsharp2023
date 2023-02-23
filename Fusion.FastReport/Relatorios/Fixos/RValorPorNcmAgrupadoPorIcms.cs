using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RValorPorNcmAgrupadoPorIcms : RelatorioBase
    {
        public static readonly string Descricao = "Relatório auxiliar de saidas por NCM agrupado por ICMS";

        public RValorPorNcmAgrupadoPorIcms(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemBytesFrx("FrValorPorNcmAgrupadoPorIcms.frx");
        }

        protected override void PrepararDados()
        {
            RegistrarDescricao(Descricao);
        }
    }
}