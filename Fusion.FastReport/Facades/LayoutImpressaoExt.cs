using System;
using Fusion.FastReport.Relatorios.Sistema;
using FusionCore.FusionNfce.Preferencias;

namespace Fusion.FastReport.Facades
{
    public static class LayoutImpressaoExt
    {
        public static IRDanfeNfce DanfeNfce(this LayoutImpressao layoutImpressao)
        {
            switch (layoutImpressao)
            {
                case LayoutImpressao.Impressao80M:
                    return new RDanfeNfce();
                case LayoutImpressao.Impressao58M:
                    return new RDanfeNfce58mm();
                case LayoutImpressao.ImpressaoA4:
                    return new RDanfeNfceA4();
                default:
                    throw new ArgumentOutOfRangeException(nameof(layoutImpressao), layoutImpressao, null);
            }
        }
    }
}