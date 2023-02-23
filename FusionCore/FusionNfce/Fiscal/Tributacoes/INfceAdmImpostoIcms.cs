using FusionCore.FusionAdm.Fiscal.FlagsImposto;
using FusionCore.FusionAdm.Nfce;

namespace FusionCore.FusionNfce.Fiscal.Tributacoes
{
    public interface INfceAdmImpostoIcms
    {
        int Id { get; set; }
        NfceItemAdm Item { get; set; }
        TributacaoCstNfce CST { get; set; }
        OrigemMercadoria OrigemMercadoria { get; set; }
    }
}