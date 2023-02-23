using FusionCore.FusionAdm.Fiscal.FlagsImposto;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionNfce.Fiscal.Tributacoes
{
    public class NfceImpostoCsosn : Entidade, INfceImpostoIcms
    {
        private string _codigoCsosn;
        // ReSharper disable once UnusedMember.Local
        private int Id { get; set; }
        public NfceItem Item { get; set; }
        public TributacaoCstNfce CST { get; set; }
        public OrigemMercadoria OrigemMercadoria { get; set; }
        public decimal AliquotaIcms { get; set; }
        public decimal ReducaoBcIcms { get; set; }
        public decimal BcIcms { get; set; }
        public decimal ValorIcms { get; set; }
        

        public string CodigoCsosn
        {
            get { return CST.Id; }
            set { _codigoCsosn = value; }
        }


        public NfceImpostoCsosn()
        {
        }

        public NfceImpostoCsosn(TributacaoCstNfce cst, decimal aliquotaIcms, decimal reducaoBcIcms)
        {
            CST = cst;
            AliquotaIcms = aliquotaIcms;
            ReducaoBcIcms = reducaoBcIcms;
            BcIcms = 0;
            ValorIcms = 0;
            OrigemMercadoria = OrigemMercadoria.Nacional;
        }

        protected override int ReferenciaUnica => Id;
    }
}