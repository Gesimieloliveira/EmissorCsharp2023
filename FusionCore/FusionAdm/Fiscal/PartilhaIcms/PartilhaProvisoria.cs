using System;

namespace FusionCore.FusionAdm.Fiscal.PartilhaIcms
{
    public static class PartilhaProvisoria
    {
        public static decimal PercentualAnoCorrente
        {
            get
            {
                switch (DateTime.Now.Year)
                {
                    case 2016:
                        return 40.00M;
                    case 2017:
                        return 60.00M;
                    case 2018:
                        return 80.00M;
                    default:
                        return 100.00M;
                }
            }
        }
    }
}