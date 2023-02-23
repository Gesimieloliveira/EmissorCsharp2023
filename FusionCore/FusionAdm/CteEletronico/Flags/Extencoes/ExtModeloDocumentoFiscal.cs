using System;
using FusionCore.DFe.XmlCte;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.CteEletronico.Flags.Extencoes
{
    public static class ExtModeloDocumentoFiscal
    {
        public static FusionTipoDocumentoFiscalCTe ToXml(this ModeloDocumento modeloFiscal)
        {
            switch (modeloFiscal)
            {
                case ModeloDocumento.CTe:
                    return FusionTipoDocumentoFiscalCTe.CTe;
            }

            throw new ArgumentException("Modelo de documento fiscal invalido");
        }
    }
}