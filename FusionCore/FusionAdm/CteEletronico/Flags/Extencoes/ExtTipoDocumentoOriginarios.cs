using System;
using FusionCore.DFe.XmlCte;

namespace FusionCore.FusionAdm.CteEletronico.Flags.Extencoes
{
    public static class ExtTipoDocumentoOriginarios
    {
        public static FusionTipoDocumentoOriginarioCTe ToXml(this TipoDocumento tipoDocumento)
        {
            switch (tipoDocumento)
            {
                case TipoDocumento.Outros:
                    return FusionTipoDocumentoOriginarioCTe.Outros;
                case TipoDocumento.Declaracao:
                    return FusionTipoDocumentoOriginarioCTe.Declaracao;
                case TipoDocumento.CfeSat:
                    return FusionTipoDocumentoOriginarioCTe.CFeSAT;
                case TipoDocumento.Nfce:
                    return FusionTipoDocumentoOriginarioCTe.NFCe;
                case TipoDocumento.Dutoviario:
                    return FusionTipoDocumentoOriginarioCTe.Dutoviario;
            }

            throw new InvalidOperationException("Tipo documento originário inválido");
        }
    }
}