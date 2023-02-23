using FusionCore.FusionAdm.Fiscal.FlagsImposto;
using FusionCore.FusionAdm.Nfce;
using FusionCore.Tributacoes.Estadual;

namespace FusionCore.FusionNfce.Fiscal.Tributacoes
{
    public class NfceImpostoCsosnAdm
    {
        // ReSharper disable once UnusedMember.Local
        public int Id { get; set; }
        public NfceItemAdm Item { get; set; }
        public TributacaoCst CST { get; set; }
        public OrigemMercadoria OrigemMercadoria { get; set; }
        public decimal AliquotaIcms { get; set; }
        public decimal ReducaoBcIcms { get; set; }
        public decimal BcIcms { get; set; }
        public decimal ValorIcms { get; set; }

        public NfceImpostoCsosnAdm()
        {
        }
    }
}