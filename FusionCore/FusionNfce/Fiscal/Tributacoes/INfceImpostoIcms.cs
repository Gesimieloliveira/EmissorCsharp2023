using FusionCore.FusionAdm.Fiscal.FlagsImposto;

namespace FusionCore.FusionNfce.Fiscal.Tributacoes
{
    public interface INfceImpostoIcms
    {
        NfceItem Item { get; set; }
        TributacaoCstNfce CST { get; set; }
        OrigemMercadoria OrigemMercadoria { get; set; }
        string CodigoCsosn { get; set; }
        decimal AliquotaIcms { get; set; }
        decimal ReducaoBcIcms { get; set; }
        decimal BcIcms { get; set; }
        decimal ValorIcms { get; set; }
    }
}