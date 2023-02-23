using System;
using FusionCore.DFe.XmlCte;

namespace FusionCore.FusionAdm.CteEletronico.Flags.Extencoes
{
    public static class ExtTipoCte
    {
        public static FusionTipoCTe ToXml(this TipoCte tipoCte)
        {
            switch (tipoCte)
            {
                case TipoCte.Normal:
                    return FusionTipoCTe.Normal;
                case TipoCte.ComplementoDeValores:
                    return FusionTipoCTe.ComplementoDeValores;
                case TipoCte.CteDeAnulacao:
                    return FusionTipoCTe.Anulacao;
                case TipoCte.CteDeSubstituicao:
                    return FusionTipoCTe.Substituto;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tipoCte), tipoCte, null);
            }
        }
    }
}