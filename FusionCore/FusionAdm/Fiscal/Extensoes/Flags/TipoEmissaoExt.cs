using System;
using NFe.Classes.Informacoes.Identificacao.Tipos;
using TipoEmissaoFusion = FusionCore.FusionAdm.Fiscal.Flags.TipoEmissao;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.Flags
{
    public static class TipoEmissaoExt
    {
        public static TipoEmissao ToZeus(this TipoEmissaoFusion tipo)
        {
            switch (tipo)
            {
                case TipoEmissaoFusion.Normal:
                    return TipoEmissao.teNormal;
                case TipoEmissaoFusion.ContigenciaOfflineNFCe:
                    return TipoEmissao.teOffLine;
                case TipoEmissaoFusion.ContigenciaEPEC:
                    return TipoEmissao.teEPEC;
                case TipoEmissaoFusion.ContigenciaSVCAN:
                    return TipoEmissao.teSVCAN;
                case TipoEmissaoFusion.ContigenciaSVCRS:
                    return TipoEmissao.teSVCRS;
            }

            throw new InvalidOperationException("Tipo Emissão de documento fiscal SEFAZ é inválido");
        }
    }
}