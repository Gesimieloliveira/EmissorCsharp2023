using System;
using FusionCore.DFe.XmlCte;

namespace FusionCore.FusionAdm.CteEletronico.Flags.Extencoes
{
    public static class ExtTipoDocumentoTransporte
    {
        public static FusionTipoDocumentoTransAnt ToXml(this TipoDocumentoAnterior documentoAnterior)
        {
            switch (documentoAnterior)
            {
                case TipoDocumentoAnterior.ATRE:
                    return FusionTipoDocumentoTransAnt.ATRE;
                case TipoDocumentoAnterior.DTA:
                    return FusionTipoDocumentoTransAnt.DTA;
                case TipoDocumentoAnterior.ConhecimentoAereoInternacional:
                    return FusionTipoDocumentoTransAnt.ConhecimentoAereoInternacional;
                case TipoDocumentoAnterior.CartaDePorteInternacional:
                    return FusionTipoDocumentoTransAnt.ConhecimentoCartaDePorteInternacional;
                case TipoDocumentoAnterior.ConhecimentoAvulso:
                    return FusionTipoDocumentoTransAnt.ConhecimentoAvulso;
                case TipoDocumentoAnterior.TIF:
                    return FusionTipoDocumentoTransAnt.TIF;
                case TipoDocumentoAnterior.BL:
                    return FusionTipoDocumentoTransAnt.BL;
                default:
                    throw new ArgumentOutOfRangeException(nameof(documentoAnterior), documentoAnterior, null);
            }
        }
    }
}