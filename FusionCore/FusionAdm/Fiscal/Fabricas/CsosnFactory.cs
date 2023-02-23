using FusionCore.FusionNfce.Fiscal.Tributacoes;

namespace FusionCore.FusionAdm.Fiscal.Fabricas
{
    public static class CsosnFactory
    {
        public static INfceImpostoIcms CriaNfce(TributacaoCstNfce cst, decimal aliquotaIcms, decimal reducaoBcIcms)
        {
            return new NfceImpostoCsosn(cst, aliquotaIcms, reducaoBcIcms);
        }
    }
}